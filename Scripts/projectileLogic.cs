using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLogic : MonoBehaviour
{
    private Animator anim;
    public float speed;
    public Coroutine flyingCoroutin;
    public string aim;
    public Vector2 dest;
    public Weapons.ProjectileType proj_type;
    public float delay;
    public float shieldDMG;
    public float hullDMG;
    public GameObject intoWho;

    private Coroutine dealDamage = null;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (proj_type == Weapons.ProjectileType.Beam)
        {
            anim.enabled = false;
            if (aim != null)
            {
                transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
                transform.GetChild(0).GetComponent<Animator>().SetTrigger("beamexplode");
            }
        }

    }

    private void Update()
    {
        if (proj_type != Weapons.ProjectileType.Beam)
        {

            if ((transform.position.y > dest.y - 0.5f) && (transform.position.y < dest.y + 0.5f))
            {
                GetComponent<BoxCollider2D>().enabled = true;
                //Debug.Log(name + " " + transform.position.y  + " " + dest.y);
            }

            if ((transform.position.y > 370f) || (Mathf.Abs(transform.position.x) > 150f))
            {
                BattleManager.instance.StopAndDestroyProj(gameObject, anim, flyingCoroutin);
            }
        }
        else //Если снаряд это луч
        {
            if (GetComponent<RectTransform>().sizeDelta.y > dest.y - 20f)
            {
                BattleManager.instance.StopAndDestroyProj(gameObject, anim, flyingCoroutin, delay);
                if (aim!=null)
                {
                    if (dealDamage == null)
                        dealDamage = StartCoroutine(DealDamage(shieldDMG, hullDMG, intoWho));
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (intoWho!=null)
        if ((collision.tag == intoWho.tag) && (collision.name==aim))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            //Debug.Log("БАБУХ");
            anim.SetTrigger("explode");
            BattleManager.instance.StopAndDestroyProj(gameObject, anim, flyingCoroutin);
            if (dealDamage==null)
                dealDamage = StartCoroutine(DealDamage(shieldDMG, hullDMG, intoWho));

        }
    }



    private IEnumerator DealDamage(float shield, float hull, GameObject toWho)
    {

        yield return null;
        if (toWho!=null)
        if (toWho.tag == "enemy")
        {
            Enemy enemyScript = toWho.GetComponent<Enemy>();

            if (enemyScript.currentShield > 0)
            {
                if (shield > enemyScript.currentShield)
                {
                    enemyScript.currentShield = 0;
                    enemyScript.currentHP -= hull;
                }
                else
                {
                    enemyScript.currentShield -= 1;
                }

                if (enemyScript.currentShield <= 0)
                {
                    enemyScript.currentShield = 0;
                    Debug.Log("Enemy's shield is down");
                }
            }
            else
            {
                enemyScript.currentHP -= hull;
                if (enemyScript.currentHP <=0)
                {
                    enemyScript.currentHP = 0;
                    Debug.Log("Enemy has been destroyed");
                }
            }


            enemyScript.UpdateHealthShield(false);
        }
        else
        {
            Player playerScript = toWho.GetComponent<Player>();

            if (playerScript.currentShield > 0)
            {
                if (shield > playerScript.currentShield)
                {
                    playerScript.currentShield = 0;
                    playerScript.currentHP -= hull;
                }
                else
                {
                    playerScript.currentShield -= 1;
                }

                if (playerScript.currentShield <= 0)
                {
                    playerScript.currentShield = 0;
                    Debug.Log("Player's shield is off");
                }
            }
            else
            {
                playerScript.currentHP -= hull;
                if (playerScript.currentHP <= 0)
                {
                    playerScript.currentHP = 0;
                    Debug.Log("Player has been defeated");
                }
            }

            playerScript.UpdateHealthShield(false);
        }

    }




}
