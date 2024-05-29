using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    [Header("Ammo")]
    int currentAmmo;
    public int CurrentAmmo
    {
        get
        {
            return currentAmmo;
        }
        set
        {
            currentAmmo = value;
            if (currentAmmo > maxAmmo)
            {
                currentAmmo = maxAmmo;
            }

            UpdateUI();
        }
    }
    public int maxAmmo;

    [Header("UI")]
    List<Image> uiImages = new();
    [SerializeField] GameObject uiPrefab;
    [SerializeField] Transform uiParent;
    [Space(10)]
    [SerializeField] Sprite fullImage;
    [SerializeField] Sprite emptyImage;

    void Start()
    {
        currentAmmo = maxAmmo;
        for (int i = 0; i < maxAmmo; i++)
        {
            GameObject newObject = Instantiate(uiPrefab, new Vector3(0, 0, 0), Quaternion.identity, uiParent);
            Image image = newObject.GetComponent<Image>();
            if (image != null)
            {
                uiImages.Add(image);
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < uiImages.Count; i++)
        {
            if (currentAmmo >= i + 1)
            {
                uiImages[i].sprite = fullImage;
            }
            else
            {
                uiImages[i].sprite = emptyImage;
            }
        }
    }
}
