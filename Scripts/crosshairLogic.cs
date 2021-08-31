using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crosshairLogic : MonoBehaviour
{
    public  string currentAim;
    public  bool inHull;
    public GameObject entityUnder;

    private void Start()
    {
        GetComponent<Image>().color = Color.red;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag == "Player")
        {
            if (collision.tag == "enemy")
            {
                entityUnder = collision.transform.parent.parent.gameObject;

                if (collision.name == "Hull")
                {
                    inHull = true;
                    GetComponent<Image>().color = Color.cyan;
                }
                else
                {
                    GetComponent<Image>().color = Color.yellow;
                    currentAim = collision.name;
                }

            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                entityUnder = collision.transform.parent.parent.gameObject;

                if (collision.name == "Hull")
                {
                    inHull = true;
                    GetComponent<Image>().color = Color.cyan;
                }
                else
                {
                    GetComponent<Image>().color = Color.yellow;
                    currentAim = collision.name;
                }

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Hull")
        {
            inHull = false;

            if ((!inHull) && (currentAim == null))
            {
                GetComponent<Image>().color = Color.red;
                entityUnder = null;
            }
            else
            {
                GetComponent<Image>().color = Color.yellow;
            }

        }
        else
        {
            currentAim = null;

            if ((!inHull) && (currentAim == null))
            {
                GetComponent<Image>().color = Color.red;
                entityUnder = null;
            }
            else
            {
                GetComponent<Image>().color = Color.cyan;
            }


        }


    }
}
