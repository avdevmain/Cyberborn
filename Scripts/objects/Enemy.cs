using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    private int weaponsAmount;



   // private Vector2 standardPlayerPos = new Vector2(3.68f, 251.7f); //Поворот WeaponSlot'a и height у Trajectory
    private Vector2 standardPlayerPosTrue = new Vector2(-0.099845f, - 2.652128f);



    private Vector2 pointToShoot;

    private List<bool> reachedX;
    private List<bool> reachedY;

    public float radius;

    private List<Vector2> greatZones;


    public Coroutine[] shootingCoroutines;

    private void Start()
    {
        
            greatZones = new List<Vector2>();

            reachedX = new List<bool>();
            reachedY = new List<bool>();

        InstantiateArray();
        
    }

    private void Update()
    {


        for (int i=0; i<weaponsAmount; i++)
        {
            if (cooldowns[i]>0)
            {
                cooldowns[i] -= BattleManager.difficulty * Time.deltaTime; // Обычное убавление кулдауна на оружии, если оно есть)
            }
            else //СТРЕЛЯЮ БЛЯЯЯЯ
            {
                TakeAim(i);
            }
        }
    }

    public void InstantiateArray()
    {

        weaponsAmount = 0;
        for (int i =0; i<4; i++)
        {
            if (inv.weaponItem[i] != null)
            {
                weaponsAmount++;
                
                reachedX.Add(false);
                reachedY.Add(false);
            }
        }
        cooldowns = new float[weaponsAmount];
        shootingCoroutines = new Coroutine[weaponsAmount];
        

    }

    private void TakeAim(int weaponIndex)
    {
        Debug.Log("Противник прицеливается из " + weaponIndex + " оружия");


        if (pointToShoot == Vector2.zero) //Первый раз когда метод вызвался
        {
            pointToShoot = standardPlayerPosTrue;

            activeWeaponItem = inv.weaponItem[weaponIndex];
            activeWeapon = transform.GetChild(weaponIndex).GetComponent<RectTransform>();
            activeTrajectory = activeWeapon.GetChild(0).GetComponent<RectTransform>();
            activeTrajectory.GetComponent<Image>().enabled = true;
            activeCross = activeTrajectory.GetChild(0).GetComponent<RectTransform>();
            activeCross.GetComponent<Image>().enabled = true;
        }


        if ((!reachedY[weaponIndex]) &&(activeCross.position.y > pointToShoot.y))
        {
            activeTrajectory.sizeDelta = new Vector2(activeTrajectory.sizeDelta.x, activeTrajectory.sizeDelta.y + 200 * BattleManager.difficulty * Time.deltaTime);

            if ((activeCross.position.y > pointToShoot.y - 0.1f) && (activeCross.position.y < pointToShoot.y + 0.1f))
            {

                reachedY[weaponIndex] = true;
            }
        }

        if (!reachedX[weaponIndex])
        {


            if (activeCross.position.x < pointToShoot.x)
                activeWeapon.localEulerAngles = new Vector3(0, 0, activeWeapon.localEulerAngles.z + 3f * BattleManager.difficulty * Time.deltaTime);
            else
                activeWeapon.localEulerAngles = new Vector3(0, 0, activeWeapon.localEulerAngles.z - 3f * BattleManager.difficulty * Time.deltaTime);

            if ((activeCross.position.x > pointToShoot.x - 0.015f) && (activeCross.position.x < pointToShoot.x + 0.015f))
            {

                reachedX[weaponIndex] = true;
            }
        }

        if (reachedX[weaponIndex] && reachedY[weaponIndex])
        {

            if (shootingCoroutines[weaponIndex] == null)
                BattleManager.instance.StartCreateProjectiles(activeCross.GetComponent<crosshairLogic>().entityUnder, gameObject, activeWeaponItem, activeWeapon, activeTrajectory, activeWeaponIndex, activeCross);


        }
    }


    public void Reload(int weaponIndex, Weapons weaponItem, RectTransform activeTrajectory)
    {
        cooldowns[weaponIndex] = weaponItem.cooldown;
        activeTrajectory.GetComponent<Image>().enabled = false;
        activeTrajectory.GetChild(0).GetComponent<Image>().enabled = false;
        activeTrajectory.sizeDelta = new Vector2(activeTrajectory.sizeDelta.x, 0);

        pointToShoot = Vector2.zero;

        shootingCoroutines[weaponIndex] = null;
        activeWeaponItem = null;
        activeWeapon = null;
        activeTrajectory = null;
        activeCross.GetComponent<Image>().enabled = false;
        activeCross = null;
        reachedX[weaponIndex] = false;
        reachedY[weaponIndex] = false;

    }


}
