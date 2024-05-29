using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BowAttack : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float chargeTime;
    float currentChargeTime;
    [SerializeField] AnimationCurve chargeGraph;
    float power;
    bool isCharging;
    [SerializeField] float minCharging;

    [Header("Projectile")]
    [SerializeField] GameObject prefab;
    GameObject currentArrow;
    [SerializeField] Transform drawPoint;

    [Header("Component")]
    [SerializeField] Animator anim;

    [SerializeField] Queue<Arrow> arrows = new();
    [SerializeField] int maxArrowsInScene;

    [SerializeField] Collider collider;
    [SerializeField] AmmoManager ammoManager;

    [Header("UI")]
    [SerializeField] Slider slider;
    [Space(15)]
    [SerializeField] RectTransform crossHair;
    [SerializeField] Vector3 minSize, maxSize;
    void Update()
    {
        ChargeBow();
    }
    void ChargeBow()
    {
        if (Input.GetButtonDown("Fire1") && ammoManager.CurrentAmmo > 0)
        {
            isCharging = true;
            currentChargeTime = 0;

            currentArrow = Instantiate(prefab, transform.position, transform.rotation, drawPoint);
        }

        if (isCharging)
        {
            currentChargeTime += Time.deltaTime;
            power = chargeGraph.Evaluate(currentChargeTime);

            currentArrow.transform.position = drawPoint.position;
            currentArrow.transform.rotation = drawPoint.rotation;

            if (Input.GetButtonUp("Fire1"))
            {
                if (power >= minCharging)
                {
                    FireBow();
                    power = 0;
                    currentChargeTime = 0;
                    isCharging = false;

                    ammoManager.CurrentAmmo--;
                }
                else
                {
                    power = 0;
                    currentChargeTime = 0;
                    isCharging = false;
                    Destroy(currentArrow);
                }
            }
            anim.SetFloat("nTime", power / 2);
            slider.value = currentChargeTime / chargeTime;

            crossHair.sizeDelta = Vector3.Lerp(minSize, maxSize, currentChargeTime);
        }
        else
        {
            isCharging = false;
        }
    }

    void FireBow()
    {
        Arrow arrow = currentArrow.GetComponent<Arrow>();

        arrows.Enqueue(arrow);
        if (arrows.Count > maxArrowsInScene)
        {
            Arrow oldestArrow = arrows.Dequeue();
            if (oldestArrow != null)
            {
                Destroy(oldestArrow.gameObject);
            }
        }

        arrow.Fire(collider, power);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * power + transform.position);
    }
}
