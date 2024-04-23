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

    [Header("Ammo")]
    [SerializeField] int currentAmmo;
    [SerializeField] int maxAmmo;

    [Header("Projectile")]
    [SerializeField] GameObject prefab;

    [Header("Component")]
    [SerializeField] Animator anim;
    [SerializeField] Slider slider;

    [SerializeField] Queue<Arrow> arrows = new();
    [SerializeField] int maxArrowsInScene;

    [SerializeField] Collider collider;
    void Update()
    {
        ChargeBow();
    }
    void ChargeBow()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isCharging = true;
            currentChargeTime = 0;
        }

        if (isCharging && currentAmmo > 0)
        {
            currentChargeTime += Time.deltaTime;
            power = chargeGraph.Evaluate(currentChargeTime);

            if (Input.GetButtonUp("Fire1"))
            {
                FireBow();
                power = 0;
                currentChargeTime = 0;
                isCharging = false;

                currentAmmo--;
            }
            anim.SetFloat("nTime", power / 2);
            slider.value = currentChargeTime / chargeTime;
        }
    }

    void FireBow()
    {
        GameObject arrowObj = Instantiate(prefab, transform.position, transform.rotation);
        Arrow arrow = arrowObj.GetComponent<Arrow>();

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
