using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    private Sprite BlueHex;
    private Sprite RedHex;
    private Sprite GoldHex;
    private Sprite WhiteHex;
    private Sprite PlayerHex;
    private Sprite EmptyHex;
    private Sprite ExitHex;
    private Sprite aim_icon;
    private Sprite[] icons;
    private HexagonList[] lines;

    private GameObject flower_prefab;
    private GameObject map_flower;

    private int plI;
    private int plJ;
    private int actI;
    private int actJ;

    //Всего 30 зон
    int enemyChance = 65; int enemyMax = 24;
    int friendChance = 14; int friendMax = 4;
    int shopChance = 8; int shopMax = 2;
    int emptyChance = 5; int emptyMax = 4;
    int blockChance = 8; int blockMax = 8;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;

            lines = new HexagonList[8];
            for (int i = 0; i < 8; i++)
            {
                lines[i] = new HexagonList();
                lines[i].zones = new GameObject[5];
                lines[i].info = new ZoneInfo[5];
                for (int j = 0; j < 5; j++)
                {
                    lines[i].info[j] = new ZoneInfo();
                    lines[i].zones[j] = transform.parent.GetChild(1).GetChild(i).GetChild(j).gameObject;
                }
            }

            BlueHex = Resources.Load<Sprite>("Sprites/Menu/Space_map/blue_hex");
            RedHex = Resources.Load<Sprite>("Sprites/Menu/Space_map/red_hex");
            GoldHex = Resources.Load<Sprite>("Sprites/Menu/Space_map/golden_hexagon");
            WhiteHex = Resources.Load<Sprite>("Sprites/Menu/Space_map/white_hex");
            PlayerHex = Resources.Load<Sprite>("Sprites/Menu/Space_map/player_hex");
            EmptyHex = Resources.Load<Sprite>("Sprites/Menu/Space_map/empty_hex");
            ExitHex = Resources.Load<Sprite>("Sprites/Menu/Space_map/exit_hex");

            aim_icon = Resources.Load<Sprite>("Sprites/Menu/Space_map/aim");
            icons = Resources.LoadAll<Sprite>("Sprites/Menu/Space_map/icons");

            flower_prefab = Resources.Load<GameObject>("Prefabs/Map_flower_1");


            GenerateMap();
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap()
    {
        int startNum = Random.Range(0, 4);
        lines[7].info[startNum].type = ZoneInfo.zoneType.start;
        lines[7].zones[startNum].GetComponent<Image>().sprite = PlayerHex;
        lines[7].zones[startNum].transform.GetChild(0).gameObject.SetActive(true);
        lines[7].zones[startNum].transform.GetChild(0).GetComponent<Image>().sprite = icons[0];
        plI = 7;
        plJ = startNum;

        int lineNum = Random.Range(0, 2);
        int exitNum = Random.Range(1, 5);
        lines[lineNum].info[exitNum].type = ZoneInfo.zoneType.exit;
        lines[lineNum].zones[exitNum].GetComponent<Image>().sprite = ExitHex;
        //lines[lineNum].zones[exitNum].transform.GetChild(0).gameObject.SetActive(true);
        //lines[lineNum].zones[exitNum].transform.GetChild(0).GetComponent<Image>().sprite = icons[6];

        lines[7].info[4].type = ZoneInfo.zoneType.blocked;
        lines[6].info[0].type = ZoneInfo.zoneType.blocked; 
        lines[5].info[4].type = ZoneInfo.zoneType.blocked;
        lines[4].info[0].type = ZoneInfo.zoneType.blocked;
        lines[3].info[4].type = ZoneInfo.zoneType.blocked;
        lines[2].info[0].type = ZoneInfo.zoneType.blocked;
        lines[1].info[4].type = ZoneInfo.zoneType.blocked;
        lines[0].info[0].type = ZoneInfo.zoneType.blocked;

        int currentEnemy = 0; int currentFriend = 0; int currentEmpty = 0; int currentShop = 0; int currentBlocked = 0;

        for (int i = 0; i < 8;i++)
        {
            for (int j = 0; j<5; j++)
            {
                while (lines[i].info[j].type == ZoneInfo.zoneType.none)
                {
                    int ASS = Random.Range(0,101);
                    if (ASS<=enemyChance)
                    {
                        if (currentEnemy<enemyMax)
                        {
                            lines[i].info[j].type = ZoneInfo.zoneType.enemy;
                            lines[i].zones[j].GetComponent<Image>().sprite = RedHex;
                            currentEnemy++;
                        }
                    }
                    else
                    if (ASS<=enemyChance+friendChance)
                    {
                        if (currentFriend<friendMax)
                        {
                            lines[i].info[j].type = ZoneInfo.zoneType.friend;
                            lines[i].zones[j].GetComponent<Image>().sprite = BlueHex;
                            currentFriend++;
                        }
                    }
                    else
                    if (ASS<=enemyChance+friendChance+shopChance)
                    {
                        if (currentShop<shopMax)
                        {
                            lines[i].info[j].type = ZoneInfo.zoneType.shop;
                            lines[i].zones[j].GetComponent<Image>().sprite = GoldHex;
                            currentShop++;
                        }
                    }
                    else
                    if (ASS<=enemyChance+friendChance+shopChance+emptyChance)
                    {
                        if (currentEmpty<emptyMax)
                        {
                            lines[i].info[j].type = ZoneInfo.zoneType.empty;
                            lines[i].zones[j].GetComponent<Image>().sprite = WhiteHex;
                            currentEmpty++;
                        }
                    }
                    else
                    {
                        if (currentBlocked<blockMax)
                        {
                            lines[i].info[j].type = ZoneInfo.zoneType.blocked;
                            lines[i].zones[j].GetComponent<Image>().sprite = EmptyHex;
                            //lines[i].zones[j].GetComponent<Image>().color = new Color32(255,255,255,20);
                            currentBlocked++;
                        }
                    }

                }
            }
        }

    }


    public void FlyToZone(int i, int j)
    {
        if (lines[i].info[j].type != ZoneInfo.zoneType.blocked)
        {
            if (Mathf.Abs(i - plI) <= 1)
            {
                if (plI > i) //Игрок ниже пункта назначения
                {
                    if ((j - plJ ==0) || (j-plJ==1))
                    {
                        Debug.Log("Летим");
                        CreateAim(i, j);
                    }
                }
                else
                if (plI < i)//Игрок выше пункта назначения
                {
                    if ((plJ - j == 0) || (plJ - j == 1))
                    {
                        Debug.Log("Летим");
                        CreateAim(i, j);
                    }
                }
                else //Игрок на одном уровне 
                {
                    if (Mathf.Abs(j - plJ) == 1)
                    {
                        Debug.Log("Летим");
                        CreateAim(i, j);
                    }
                }

            }
        }

    }

    private void CreateAim(int i, int j)
    {
        if ((actI!=0) && (actJ!=0))
        {
            lines[actI].zones[actJ].transform.GetChild(0).gameObject.SetActive(false);
            lines[actI].zones[actJ].transform.GetChild(0).GetComponent<Image>().sprite = null;
        }
        lines[i].zones[j].transform.GetChild(0).gameObject.SetActive(true);
        lines[i].zones[j].transform.GetChild(0).GetComponent<Image>().sprite = aim_icon;
        actI = i;
        actJ = j;

        if (map_flower != null)
            Destroy(map_flower);

        map_flower = Instantiate(flower_prefab, transform.parent);
        map_flower.transform.position = lines[i].zones[j].transform.position;

        
        



    }
}

public class HexagonList
{
    public GameObject[] zones;
    public ZoneInfo[] info;

}

public class ZoneInfo
{
    public bool cleared = false;
    public zoneType type = zoneType.none;
    public enum zoneType
    {
        none,
        start,
        enemy,
        exit,
        shop,
        friend,
        empty,
        blocked
    }
}

