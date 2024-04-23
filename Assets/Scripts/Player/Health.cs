using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] Slider healthBar;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDmg(1);
        }
    }

    void TakeDmg(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth < 0)
        {
            currentHealth = maxHealth;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = currentHealth;
    }
}
