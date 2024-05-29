using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSupply : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out AmmoManager ammoManager))
        {
            ammoManager.CurrentAmmo = ammoManager.maxAmmo;
        }
    }
}
