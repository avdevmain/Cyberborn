using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{


    public static BattleManager instance;

    private List<Weapons> weaponsInGame;
 //   private List<Animator> shotEffects;



    private Transform playerShip;
    private Transform playerSupLeft;
    private Transform playerSupRight;

    private Transform weaponPanel;

    private Transform enemyShip;
    private Transform enemySupLeft;
    private Transform enemySupRight;

    private GameObject blankprojectilePrefab;
    private GameObject projectilePrefab;
    private GameObject beamprojectilePrefab;
    private List<GameObject> projectiles;
    private List<GameObject> blankProjectiles;
   // private List<GameObject> realProjectiles;

    public static float difficulty;




    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            enemyShip = transform.parent.GetChild(2);
            playerShip = transform.parent.GetChild(5);
            weaponPanel = transform.parent.GetChild(6);
            weaponsInGame = new List<Weapons>();

            projectiles = new List<GameObject>();
          //  blankProjectiles = new List<GameObject>();
          //  realProjectiles = new List<GameObject>();
            projectilePrefab = Resources.Load<GameObject>("Prefabs/projectile");
            beamprojectilePrefab = Resources.Load<GameObject>("Prefabs/beamprojectile");
            blankprojectilePrefab = Resources.Load<GameObject>("Prefabs/blankprojectile");

            difficulty = 1f; //НОРМАЛЬНАЯ СЛОЖНОСТЬ
            
            SetupBattleField();
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame


    public void StartCreateProjectiles(GameObject toWho, GameObject who, Weapons weapon, RectTransform weaponObj, RectTransform trajectory, int weaponIndex, RectTransform cross)
    {
        if (who.tag == "enemy")
        who.GetComponent<Enemy>().shootingCoroutines[weaponIndex] = StartCoroutine(CreateProjectiles(toWho, who, weapon, weaponObj, trajectory, weaponIndex, cross));
        else
            StartCoroutine(CreateProjectiles(toWho, who, weapon, weaponObj, trajectory, weaponIndex, cross));

    }

    public void SetupBattleField()
    {
        Player playerScript = playerShip.GetComponent<Player>();
        Enemy enemyScript = enemyShip.GetComponent<Enemy>();

        playerSupLeft = transform.parent.GetChild(3) ;
        //playerSupRight = transform.parent.GetChild(4).GetChild(0);
       // enemySupLeft = transform.parent.GetChild(0).GetChild(0);
       // enemySupRight = transform.parent.GetChild(1).GetChild(0);


        for (int i = 0; i < 4; i++) //Загрузка оружия
        {
            if (playerScript.inv.weaponItem[i] != null)
            {
                weaponPanel.GetChild(i).gameObject.SetActive(true);

                weaponPanel.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerShip.GetComponent<Player>().inv.weaponItem[i].icon;
                weaponPanel.GetChild(i).GetChild(0).GetComponent<Image>().enabled = true;


                playerShip.GetChild(i).GetChild(2).GetComponent<Image>().sprite = playerShip.GetComponent<Player>().inv.weaponItem[i].icon;
                playerShip.GetChild(i).GetChild(2).GetComponent<Image>().enabled = true;


                Weapons weapontoAdd = playerScript.inv.weaponItem[i];
                weaponsInGame.Add(weapontoAdd);
                playerScript.shotEffects.Add(playerShip.GetChild(i).GetChild(1).GetComponent<Animator>()); //Это заменяются только эффекты при самом произведении выстрела (бубух от самого оружия)
                playerScript.shotEffects[i].runtimeAnimatorController = weapontoAdd.projectileAOC;

                

                if (weaponsInGame[i].projectile_Type==Weapons.ProjectileType.Scatter)
                {
                    GameObject pricel = playerShip.GetChild(i).GetChild(0).GetChild(0).gameObject; //ПРИЦЕЛ
                    pricel.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Entities/boxhair");
                    pricel.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
                }
            }

            if (enemyShip.GetComponent<Enemy>().inv.weaponItem[i]!= null)
            {
                enemyShip.GetChild(i).GetChild(2).GetComponent<Image>().sprite = enemyShip.GetComponent<Enemy>().inv.weaponItem[i].icon;
                enemyShip.GetChild(i).GetChild(2).GetComponent<Image>().enabled = true;

                enemyScript.shotEffects.Add(enemyShip.GetChild(i).GetChild(1).GetComponent<Animator>()); //Это заменяются только эффекты при самом произведении выстрела (бубух от самого оружия)
                enemyScript.shotEffects[i].runtimeAnimatorController = enemyScript.inv.weaponItem[i].projectileAOC;
            }
        }

        enemyShip.GetComponent<Enemy>().InstantiateArray();

        //Загрузка щита
        playerShip.GetComponent<Player>().UpdateHealthShield(true);
        enemyShip.GetComponent<Enemy>().UpdateHealthShield(true);

    }



    public void ChangeActiveWeapon(int weaponIndex)
    {
        if (weaponIndex>=0)
            playerShip.GetComponent<Player>().ChangeActiveWeapon(weaponIndex, weaponsInGame[weaponIndex]);
        else
        {
            playerShip.GetComponent<Player>().DisableWeapons();

            if (weaponIndex == -2)
            {
                if (playerSupLeft.gameObject.activeSelf)
                    playerSupLeft.GetComponent<Player>().ChangeActiveWeapon(0, playerSupLeft.GetComponent<Player>().inv.weaponItem[0]);
            }
            else
            {
                if (playerSupRight.gameObject.activeSelf)
                    playerSupRight.GetComponent<Player>().ChangeActiveWeapon(0, playerSupLeft.GetComponent<Player>().inv.weaponItem[0]);
            }
        }
        
    }



    public void StopAndDestroyProj(GameObject who, Animator anim, Coroutine flyCorout, float delay = 0)
    {
        if (flyCorout != null)
        {
            StopCoroutine(flyCorout);
        }
        StartCoroutine(WaitForSecondAndDestroy(who, anim, delay));
    }

    private void CreateProjectile(GameObject toWho, GameObject who, Weapons weapon, RectTransform weaponObj, int weaponIndex, RectTransform trajectory, string aim, Vector2 dest, float angleDev = 0, float rangeDev = 0)
    {

        GameObject newTrajectory = Instantiate(trajectory.gameObject, transform.parent);
        newTrajectory.GetComponent<RectTransform>().position = trajectory.position;
        newTrajectory.GetComponent<RectTransform>().eulerAngles = trajectory.eulerAngles;
        newTrajectory.GetComponent<RectTransform>().eulerAngles = new Vector3(newTrajectory.GetComponent<RectTransform>().eulerAngles.x, newTrajectory.GetComponent<RectTransform>().eulerAngles.y, newTrajectory.GetComponent<RectTransform>().eulerAngles.z + angleDev);

        dest = new Vector2(dest.x, dest.y + rangeDev);
        newTrajectory.GetComponent<Image>().enabled = false;
        newTrajectory.GetComponent<RectTransform>().sizeDelta = trajectory.sizeDelta * 1.44f;
        Destroy(newTrajectory.transform.GetChild(0).gameObject);


        if (weapon.projectile_Type==Weapons.ProjectileType.Beam)
        {
            beamprojectilePrefab.GetComponent<Image>().sprite = weapon.projectileIcon;// 1
            projectiles.Add(Instantiate(beamprojectilePrefab, newTrajectory.transform));
        }
        else
        {
            projectilePrefab.GetComponent<Image>().sprite = weapon.projectileIcon;// 1
            projectiles.Add(Instantiate(projectilePrefab, newTrajectory.transform));

        }

        if (who.tag == "enemy")
            enemyShip.GetComponent<Enemy>().shotEffects[weaponIndex].SetTrigger("shot");
        else
            playerShip.GetComponent<Player>().shotEffects[weaponIndex].SetTrigger("shot");
        

        GameObject thisProjectile = projectiles[(projectiles.Count - 1)];

        thisProjectile.transform.localPosition = new Vector2(thisProjectile.transform.localPosition.x - 1f,thisProjectile.transform.localPosition.y + 10f);
        thisProjectile.GetComponent<Animator>().runtimeAnimatorController = weapon.projectileAOC;
        if (weapon.projectile_Type!=Weapons.ProjectileType.Beam)
            thisProjectile.GetComponent<projectileLogic>().flyingCoroutin = StartCoroutine(MakeProjectileFly(projectiles[projectiles.Count - 1], weapon.projectileSpeed, dest,weapon.projectile_Type));//4 3,4 - застявляют его лететь
        thisProjectile.GetComponent<projectileLogic>().dest = dest;
        thisProjectile.GetComponent<projectileLogic>().aim = aim;
        thisProjectile.GetComponent<projectileLogic>().proj_type = weapon.projectile_Type;
        thisProjectile.GetComponent<projectileLogic>().shieldDMG = weapon.shieldPiercing;
        thisProjectile.GetComponent<projectileLogic>().hullDMG = weapon.damage;
        thisProjectile.GetComponent<projectileLogic>().intoWho = toWho;
        if (weapon.projectile_Type == Weapons.ProjectileType.Beam)
        {
            thisProjectile.GetComponent<projectileLogic>().delay = weapon.projectileDelay;
            thisProjectile.GetComponent<RectTransform>().sizeDelta = new Vector2(thisProjectile.GetComponent<RectTransform>().sizeDelta.x, thisProjectile.GetComponent<RectTransform>().sizeDelta.y + newTrajectory.GetComponent<RectTransform>().sizeDelta.y - 19f);
            
        }

       // thisProjectile.transform.SetParent(thisProjectile.transform.parent.parent.parent);



        thisProjectile.GetComponent<BoxCollider2D>().size = thisProjectile.GetComponent<RectTransform>().sizeDelta;


        StartCoroutine(ThrustersWorkOnShot());
    }

    public IEnumerator CreateProjectiles(GameObject toWho, GameObject who, Weapons weapon, RectTransform weaponObj, RectTransform trajectory, int weaponIndex, RectTransform cross)
    {
        string aim;
       // float destinationY = trajectory.sizeDelta.y + 10f; //3
        Vector2 destination = cross.transform.position;

        crosshairLogic currentCross = who.transform.GetChild(weaponIndex).GetChild(0).GetChild(0).GetComponent<crosshairLogic>();

        if ((currentCross.currentAim == null) && (currentCross.inHull))
            aim = "Hull";
        else
         aim = currentCross.currentAim;





        if (weapon.projectile_Type == Weapons.ProjectileType.Scatter)
        {
            projectilePrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(6,6);
            for (int i = 0; i < weapon.projectileAmount - 1; i++)
            {
                yield return null;
                float angleDev = Random.Range(-10, 10);
                float rangeDev = Random.Range(-0.2f,0.2f);

                CreateProjectile(toWho, who, weapon, weaponObj,weaponIndex, trajectory, aim, destination, angleDev, rangeDev);
                if (i == weapon.projectileAmount - 2)
                {
                    if (who.tag != "enemy")
                        StartCoroutine(Reload(weapon, weaponPanel.GetChild(weaponIndex).GetChild(1).GetChild(0).gameObject, weaponIndex));
                    else
                        who.GetComponent<Enemy>().Reload(weaponIndex, weapon, trajectory);
                    StartCoroutine(ResetWeaponAngle(weaponObj, weapon.cooldown/2));
                }

            }
        }
        else
        if (weapon.projectile_Type == Weapons.ProjectileType.Beam)
        {
            projectilePrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(3,6);
            CreateProjectile(toWho ,who, weapon, weaponObj, weaponIndex, trajectory, aim, destination);
            if (who.tag!="enemy")
                StartCoroutine(Reload( weapon, weaponPanel.GetChild(weaponIndex).GetChild(1).GetChild(0).gameObject, weaponIndex, weapon.projectileDelay));
            else
                who.GetComponent<Enemy>().Reload(weaponIndex, weapon, trajectory);
            StartCoroutine(ResetWeaponAngle(weaponObj, weapon.cooldown/2));
        }
        else
        {
            projectilePrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(12, 12);

            CreateProjectile(toWho, who, weapon, weaponObj,weaponIndex, trajectory, aim, destination);
            yield return null;
            if (weapon.projectile_Type == Weapons.ProjectileType.Burst)
                for (int i = 0; i < weapon.projectileAmount - 1; i++)
                {
                    yield return new WaitForSecondsRealtime(weapon.projectileDelay);
                    CreateProjectile(toWho, who, weapon, weaponObj,weaponIndex, trajectory, aim, destination);
                    if (i == weapon.projectileAmount - 2)
                    {
                        if (who.tag!="enemy")
                            StartCoroutine(Reload(weapon, weaponPanel.GetChild(weaponIndex).GetChild(1).GetChild(0).gameObject, weaponIndex));
                        else
                            who.GetComponent<Enemy>().Reload(weaponIndex, weapon, trajectory);
                        StartCoroutine(ResetWeaponAngle(weaponObj,weapon.cooldown/2));
                    }

                }
            else
            {
                if (who.tag!="enemy")
                    StartCoroutine(Reload(weapon, weaponPanel.GetChild(weaponIndex).GetChild(1).GetChild(0).gameObject, weaponIndex));
                else
                    who.GetComponent<Enemy>().Reload(weaponIndex, weapon, trajectory);
                StartCoroutine(ResetWeaponAngle(weaponObj, weapon.cooldown/2));

            }
        }
    }



    private IEnumerator ThrustersWorkOnShot()
    {
        for (int i = 0; i < playerShip.GetChild(3).childCount; i++)
        {
            playerShip.GetChild(3).GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(4, 10);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < playerShip.GetChild(3).childCount; i++)
        {
            playerShip.GetChild(3).GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(4, 4);
        }
    }

    private IEnumerator WaitForSecondAndDestroy(GameObject who, Animator anim, float delay = 0)
    {
        if (delay == 0)
        {
            yield return new WaitForSecondsRealtime(anim.GetCurrentAnimatorStateInfo(0).length);
            if (who != null)
            {
                Destroy(who.transform.parent.gameObject);
                projectiles.Remove(who);
                //Destroy(who);
            }

        }
        else
        {
            yield return new WaitForSecondsRealtime(delay);
            if (who!=null)
            {
                Destroy(who.transform.parent.gameObject);
                projectiles.Remove(who);
            }
           // 
           // 
           // Destroy(who);
        }
    }

    private IEnumerator Reload(Weapons weapon,GameObject cooldownImg, int weaponIndex, float timeDelay = 0)
    {
        
            yield return new WaitForSecondsRealtime(timeDelay);
            weaponPanel.GetChild(weaponIndex).GetComponent<Button>().interactable = false;
            if (cooldownImg != null)
            {
                cooldownImg.GetComponent<Image>().color = Color.red;
                cooldownImg.transform.localScale = new Vector3(1, 0, 1);
                float timer = 0;

                while (cooldownImg.transform.localScale.y < 1)
                {
                    yield return null;
                    timer += 1 * Time.deltaTime;
                    float value = timer / weapon.cooldown;
                    cooldownImg.transform.localScale = new Vector3(1, value, 1);

                    if (cooldownImg.transform.localScale.y > 0.99)
                    {
                        cooldownImg.transform.localScale = new Vector3(1, 1, 1);
                        cooldownImg.GetComponent<Image>().color = Color.green;
                        weaponPanel.GetChild(weaponIndex).GetComponent<Button>().interactable = true;
                    }
                }
            }
            
        

    }

    private IEnumerator MakeProjectileFly(GameObject projectile, float speed, Vector2 destination, Weapons.ProjectileType proj_type)
    {
        projectile.GetComponent<Animator>().SetTrigger("fly");

        int upordown = 1;

        if (proj_type != Weapons.ProjectileType.Beam)
        {
            float distDiff = 0;

            if (destination.x != 0)
            {
                distDiff = (destination.x - projectile.transform.position.x) / Mathf.Abs(projectile.transform.position.y - destination.y);

                upordown = 1;
                if (projectile.transform.position.y > destination.y + 0.1f)
                    upordown = -1;
            }

            while (projectile != null)
            {
                yield return null;

                if (projectile != null)
                {
                   projectile.transform.position = new Vector2(projectile.transform.position.x + (speed * distDiff * Time.deltaTime), projectile.transform.position.y + speed * upordown * Time.deltaTime);
                }


            }

        }

    }

    private IEnumerator ResetWeaponAngle(RectTransform weapon, float timeDelay)
    {

        yield return new WaitForSecondsRealtime(timeDelay);
        while (weapon.localEulerAngles.z!=0)
        {
            yield return null;

            if ((weapon.localEulerAngles.z < 0 ) || (weapon.localEulerAngles.z > 45))
            {
                weapon.localEulerAngles = new Vector3(weapon.localEulerAngles.x, weapon.localEulerAngles.y, weapon.localEulerAngles.z + 0.2f);
            }
            else
            {
                weapon.localEulerAngles = new Vector3(weapon.localEulerAngles.x, weapon.localEulerAngles.y, weapon.localEulerAngles.z - 0.2f);
            }


            if ((weapon.localEulerAngles.z > -2) && (weapon.localEulerAngles.z < 2))
                weapon.localEulerAngles = new Vector3(0,0,0);
        }

    }
}
