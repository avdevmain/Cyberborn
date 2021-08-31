using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{


    

    ///////////////// INVENTORY STUFF /////////////////
    public Image[] itemImages = new Image[numItemSlots];
    public Item[] items = new Item[numItemSlots];
    public Text[] titles = new Text[numItemSlots];
    public Text[] descriptions = new Text[numItemSlots];

    public const int numItemSlots = 4;
    ///////////////////////////////////////////////////

    ///////////////// USABLE STUFF //////////////////

    public Image[] usableImages = new Image[usableSlots];
    public Item[] usableItems = new Item[usableSlots];

    public const int usableSlots = 2;
    ///////////////////////////////////////////////////

    ///////////////// EQUIPPED STUFF //////////////////

    public Image[] equipImages = new Image[otherItemSlots];
    public Equipment[] shipEquipment = new Equipment[otherItemSlots];

    public static int[] equipmentCurrentHP = new int[otherItemSlots];

    public const int otherItemSlots = 8;
    ///////////////////////////////////////////////////

    public Image[] weaponsImages = new Image[weaponSlots];
    public Weapons[] weaponItem = new Weapons[weaponSlots];

    public const int weaponSlots = 4;
    ////////////////////////////////////////////////////

    public void AddItem(Item itemToAdd)
    {   
        for (int i =0; i<items.Length; i++)
        {
            if (items[i] ==null)
            {
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.icon;
                itemImages[i].enabled = true;
                titles[i].text = itemToAdd.item_name;
                descriptions[i].text = itemToAdd.description;
                return;
            }
        }
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                titles[i].text = null;
                descriptions[i].text = null;
                return;
            }
        }
    }


}
