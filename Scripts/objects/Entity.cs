using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Entity : MonoBehaviour
{
    public Color32 myRed = new Color32(130, 63, 63, 255);
    public Color32 myBlue = new Color32(26, 76, 102, 255);
    public Color32 myGreen = new Color32(111, 120, 67, 255);

    private Sprite[] healthFrameImages = new Sprite[2];
    private Image healthbarFrame;
    private GameObject healthbarFilling;

    public float currentHP;
    public float currentShield;

    public enum Who
    {
        Capital,
        Support
    }

    public Who shipType;
    public Inventory inv;

    public float[] cooldowns;

    public RectTransform activeCross;
    public RectTransform activeTrajectory;
    public RectTransform activeWeapon;
    public Weapons activeWeaponItem;
    public byte activeWeaponIndex;

    public List<Animator> shotEffects;

    private void Start()
    {
        shotEffects = new List<Animator>();
    }

    public void SetupHealth(bool isShieldThere)
    {
        currentHP = inv.shipEquipment[0].power;
        if (isShieldThere)
            currentShield = inv.shipEquipment[3].power;
    }

    public void UpdateHealthShield(bool initial = false)
    {
        if (initial)
        {
            Sprite[] allBarImg = Resources.LoadAll<Sprite>("Sprites/Entities/bar");
            healthFrameImages[0] = allBarImg.Single(s => s.name == "bar_1");
            healthFrameImages[1] = allBarImg.Single(s => s.name == "bar_2");
            Transform hb = transform.Find("healthbar");


            healthbarFrame = hb.GetChild(1).GetComponent<Image>();
            healthbarFilling = hb.GetChild(0).gameObject;

            if (inv.shipEquipment[3]!=null)
                SetupHealth(true);
            else
                SetupHealth(false);
        }
        
        
            if (inv.shipEquipment[3] != null) //Установлен какой-то щит
            {

                Equipment shield = inv.shipEquipment[3];

                if (currentShield > 0)
                {

                    healthbarFrame.sprite = healthFrameImages[1];
                    healthbarFilling.GetComponent<Image>().color = myBlue;
                    float value = currentShield / shield.power;
                    healthbarFilling.transform.localScale = new Vector3(value, 1, 1);
                }
                else
                {
                    healthbarFrame.sprite = healthFrameImages[0];
                    healthbarFilling.GetComponent<Image>().color = myRed;
                    if (currentHP > 0)
                    {
                        float value = currentHP / inv.shipEquipment[0].power;
                        healthbarFilling.transform.localScale = new Vector3(value, 1, 1);
                    }
                    else
                        healthbarFilling.transform.localScale = new Vector3(0, 1, 1);
                }
            }
            else //нет щита
            {

                healthbarFrame.sprite = healthFrameImages[0];
                healthbarFilling.GetComponent<Image>().color = myRed;
                if (currentHP > 0)
                {
                    float value = currentHP / inv.shipEquipment[0].power;
                    healthbarFilling.transform.localScale = new Vector3(value, 1, 1);
                }
                else
                    healthbarFilling.transform.localScale = new Vector3(0, 1, 1);
            }

        



    }

}
