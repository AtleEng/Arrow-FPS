using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHitable
{
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;

    Rigidbody rB;

    void Start()
    {
        rB = GetComponent<Rigidbody>();
    }
    public void OnHit(Arrow arrow)
    {
        currentHealth -= arrow.dmg;
        //rB.AddForce(arrow.transform.forward * arrow.dmg);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
