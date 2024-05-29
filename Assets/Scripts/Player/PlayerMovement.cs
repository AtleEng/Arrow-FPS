using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Basic Movement")]
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float groundDrag;
    Vector3 moveInput;
    Vector3 moveDirection;

    [Header("Sprint")]
    [SerializeField] float sprintModifier = 1.5f;
    [SerializeField] bool isSprinting;
    [Space(10)]
    [SerializeField] float currentStamina;
    [SerializeField] float maxStamina;
    [SerializeField] bool isStaminaDeplited;
    [Space(10)]
    [SerializeField] float recoverySpeed;
    [SerializeField] float deplitionSpeed;
    [Space(10)]
    [SerializeField] Slider staminaSlider;
    [SerializeField] Image sliderImage;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump = true;

    [Header("GroundCheck")]
    [SerializeField] Transform checkTransform;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;

    [Header("SlopeHandling")]
    [SerializeField] float maxSlopeAngle;
    [SerializeField] float slopeCheckRange;
    RaycastHit slopeHit;

    [SerializeField] Transform oriention;
    [HideInInspector] public MovementStates state = MovementStates.walking;
    public enum MovementStates
    {
        walking, airborn
    }
    Rigidbody rb;
    [SerializeField] GameObject playerMesh;
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        staminaSlider.maxValue = maxStamina;
    }
    void Update()
    {
        CheckWorld();

        MoveInputs();
        SpeedControll();
        StateHandler();

        if (isSprinting)
        {
            currentStamina -= deplitionSpeed * Time.deltaTime;
            if (currentStamina <= 0)
            {
                isStaminaDeplited = true;
                sliderImage.color = Color.red;
            }
        }
        else
        {
            currentStamina += recoverySpeed * Time.deltaTime;
            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                isStaminaDeplited = false;
                sliderImage.color = Color.white;
            }
        }
        staminaSlider.value = currentStamina;

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        moveDirection = oriention.forward * moveInput.y + oriention.right * moveInput.x;
        /*
            if (OnSlope())
            {
              rb.AddForce(GetSlopeMoveDiriection() * movementSpeed * 20f, ForceMode.Force);
            }
            */
        if (isGrounded)
        {
            rb.AddForce(moveDirection * movementSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void MoveInputs()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (moveInput.y > 0 && Input.GetKey(KeyCode.LeftShift) && !isStaminaDeplited)
        {
            moveInput *= sprintModifier;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (Input.GetButtonDown("Jump") && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();

            StartCoroutine(ResetJump(jumpCooldown));
        }
        print($"{rb.velocity}");
    }
    void CheckWorld()
    {
        isGrounded = Physics.CheckBox(checkTransform.position, checkTransform.localScale / 2, Quaternion.identity, groundLayer);
    }

    void StateHandler()
    {
        if (isGrounded)
        {
            state = MovementStates.walking;
        }
        else
        {
            state = MovementStates.airborn;
        }
    }
    void SpeedControll()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (isSprinting)
        {
            if (flatVelocity.magnitude > movementSpeed * sprintModifier)
            {
                Vector3 limitVelocity = flatVelocity.normalized * movementSpeed * sprintModifier;
                rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
            }
        }
        else
        {
            if (flatVelocity.magnitude > movementSpeed)
            {
                Vector3 limitVelocity = flatVelocity.normalized * movementSpeed;
                rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
            }
        }
    }
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    IEnumerator ResetJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        readyToJump = true;
    }
    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, slopeCheckRange))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    Vector3 GetSlopeMoveDiriection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (isGrounded) { Gizmos.color = Color.red; }

        Gizmos.DrawCube(checkTransform.position, checkTransform.localScale);
    }
}
