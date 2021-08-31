using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : Entity
{
    Vector3 mouse_pos;
    Vector3 object_pos;
    float angle; float power;

    private Vector2 initialMousePos;
    private Vector2 initialClickPos;


    //Баланс различной валюты, информация об открытых вещах

    private void Start()
    {

    }

    void Update()
    {
        if (activeWeapon != null)
        {

            if (Input.GetMouseButtonDown(0))
            {
                initialClickPos = Input.mousePosition;
                activeTrajectory.GetComponent<Image>().enabled = true;
                activeTrajectory.GetChild(0).GetComponent<Image>().enabled = true;

            }
            else
            if (Input.GetMouseButton(0))
            {
                angle = (Input.mousePosition.x - initialClickPos.x) / 3;
                power = (initialClickPos.y - Input.mousePosition.y) * 2;
                if (Mathf.Abs(angle) < 46)
                    activeWeapon.eulerAngles = new Vector3(0, 0, angle);
                if ((power < 270) && (power > 0))
                {
                    activeTrajectory.sizeDelta = new Vector2(activeTrajectory.sizeDelta.x, power);
                }
            }
            else
            if ((Input.GetMouseButtonUp(0)) && (power != 0)) //Чел отпустил палец
            {
                activeTrajectory.GetComponent<Image>().enabled = false;
                activeTrajectory.GetChild(0).GetComponent<Image>().enabled = false;

                //СТАРТ КОРУТИНЫ СО СТРЕЛЬБОЙ

                BattleManager.instance.StartCreateProjectiles(activeCross.GetComponent<crosshairLogic>().entityUnder, gameObject, activeWeaponItem, activeWeapon, activeTrajectory, activeWeaponIndex, activeCross);

                angle = 0;
                power = 0;
                activeTrajectory.sizeDelta = new Vector2(activeTrajectory.sizeDelta.x, 0);
                activeWeapon = null;
                activeTrajectory = null;
                activeWeaponItem = null;
            }
        }
    }

    public void ChangeActiveWeapon(int weaponIndex, Weapons whichWeapon)
    {

        if (activeWeapon != null)
        {
            activeTrajectory.GetComponent<Image>().enabled = false; //Отключение объекта траектории старого оружия (становится неактивным)
            activeTrajectory.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
        activeWeaponIndex = (byte)weaponIndex;
        activeWeapon = transform.GetChild(weaponIndex).GetComponent<RectTransform>();
        activeTrajectory = activeWeapon.transform.GetChild(0).GetComponent<RectTransform>();
        activeTrajectory.GetComponent<Image>().enabled = true; //Включение объекта траектории
        activeCross = activeTrajectory.GetChild(0).GetComponent<RectTransform>();
        activeCross.GetComponent<Image>().enabled = true;
        activeCross.GetComponent<Image>().color = Color.red;

        activeWeaponItem = whichWeapon;


    }

    public void DisableWeapons()
    {
        if (activeWeapon!=null)
        {
            activeWeapon = null;
            activeTrajectory.GetComponent<Image>().enabled = false;
            activeTrajectory.transform.GetChild(0).GetComponent<Image>().enabled = false;
            activeTrajectory = null;
            activeCross = null;
            activeWeapon = null;
            activeWeaponItem = null;
        }
    }

}
