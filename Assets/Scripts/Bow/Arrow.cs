using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    bool hasFired = false;
    [SerializeField] float startForce;
    public int dmg;
    [SerializeField] float destroyTime;

    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] Vector3 dirEfficiant;
    [SerializeField] TrailRenderer trailRenderer;

    Collider sender;

    bool isStuck;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    // Start is called before the first frame update
    public void Fire(Collider sender, float power)
    {
        hasFired = true;
        transform.parent = null;

        this.sender = sender;

        col.enabled = true;
        rb.isKinematic = false;

        trailRenderer.enabled = true;

        // Calculate collision point based on camera's direction and dirEfficiant
        Vector3 collisionPoint = cam.transform.position + cam.transform.forward * dirEfficiant.z + cam.transform.up * dirEfficiant.y + cam.transform.right * dirEfficiant.x;
        Vector3 direction = collisionPoint - transform.position;

        direction = direction.normalized;

        transform.rotation = Quaternion.LookRotation(direction);
        rb.AddForce(direction * startForce * power, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFired)
        {
            if (isStuck)
            {

            }
            else
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
                destroyTime -= Time.deltaTime;
                if (destroyTime <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isStuck)
        {
            if (sender == other) return;

            RaycastHit hit;
            if (Physics.Raycast(transform.position - transform.forward, rb.velocity, out hit))
            {
                if (hit.collider != other) return;

                rb.velocity = Vector3.zero;
                rb.isKinematic = true;

                isStuck = true;

                // Adjust the arrow's position to the exact hit point
                transform.position = hit.point;

                transform.SetParent(other.transform, true);

                if (other.gameObject.TryGetComponent(out IHitable hitableThing))
                {
                    hitableThing.OnHit(this);
                }
            }
        }
        else
        {
            if (other.gameObject.TryGetComponent(out AmmoManager ammoManager))
            {
                ammoManager.CurrentAmmo++;
                Destroy(gameObject);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward);
    }
}

public interface IHitable
{
    void OnHit(Arrow arrow);
}
