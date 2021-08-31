using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorClass : MonoBehaviour
{

    public GameObject battleManagerPrefab;
    public GameObject mapManagerPrefab;

    void Start()
    {
        if (BattleManager.instance==null)
        {
            Transform battleSub = transform.parent.GetChild(1).GetChild(1).GetChild(4).transform;
           Instantiate(battleManagerPrefab, battleSub);
            
        }

        if (MapManager.instance==null)
        {
            Transform mapSub = transform.parent.GetChild(1).GetChild(1).GetChild(3).transform;
            Instantiate(mapManagerPrefab, mapSub);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WeaponChosen(int weaponId)
    {
        BattleManager.instance.ChangeActiveWeapon(weaponId);
    }
}
