using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamescript : Singleton<gamescript> {

    public static gamescript instance;

    //for backgroudn init
    public ObjectPool Pool { get; set; }
    public Portals TPortal { get; set; }


    public static Vector3 LeftSpawn = new Vector3(-15, 3, 0);
    public static Vector3 RightSpawn = new Vector3(15, 0, 0);

    //for UI
    private int gold;
    [SerializeField]
    private Text goldText;
    private int sHealth;
    [SerializeField]
    private Text sHealthText;
    [SerializeField]
    private Text WLText;


    //for number of Black Unit Types
    [SerializeField]
    private int[] bountyList;
    [SerializeField]
    private float antisiutimer;
        //for spawning, done periodically atm
        private float antisiustart;
        private float antisiucd;


    //for number of White Unit types
    public int TypesofMonsters;
    [SerializeField]
    private int[] costList;
    [SerializeField]
    private float[] CDTimes;
    private float[] LastCastCDTimes;

    public float[] getCDTimes()
    {
        return CDTimes;
    }


    // for managing gold. Updating through "Gold" allows the gold text to update along it
    private int Gold
    {
        get
        {
            return gold;
        }

        set
        {
            gold = value;
            this.goldText.text = "<color=black>$</color> " + value.ToString();
        }
    }

    public void GainGold(int gain)
    {
        Gold +=gain;
    }


    // Similarly for health
    public int SquidHealth
    {
        get
        {
            return sHealth;
        }

        set
        {
            sHealth = value;
            this.sHealthText.text = "<color=black>SquidBoi's Health: </color>" + value.ToString();
        }
    }


    
    

    //public gamescript Instance;

    public static Vector3 GetLSpawnLoc()
    {
        return LeftSpawn;
    }

    public static Vector3 GetRSpawnLoc()
    {
        return RightSpawn;
    }

    [SerializeField]
    private GameObject tPortalPrefab;
    [SerializeField]
    private GameObject sPortalPrefab;

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();

    }
    // Use this for initialization

    void Start() {
        SquidHealth = 4;
        instance = this;
        SpawnPortals();
        Gold = 5;

        TypesofMonsters = CDTimes.Length;
        LastCastCDTimes = new float[TypesofMonsters];
        for (int i =0; i<TypesofMonsters; i++)
        {
            LastCastCDTimes[i] = -CDTimes[i];
        }
        
        antisiucd = antisiutimer;
        antisiustart = - antisiucd;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > antisiustart + antisiucd)
        {
            antisiustart = Time.time;
            int lol = (int) Random.Range(0, 2);
            StartCoroutine(SpawnAMonster(lol));
        }
    }

    public void SpawnMonsterButton(int type)
    {
        if (Time.time > LastCastCDTimes[type] + CDTimes[type])
        {
            if (Gold >= costList[type])
            {
                Gold -= costList[type];
                LastCastCDTimes[type] = Time.time;
                StartCoroutine(SpawnMonster(type));
            }
        }
    }


    private IEnumerator SpawnMonster(int type)
    {
        if (type < TypesofMonsters)
        {
         
            CDManager.Instance.PutOnCD(type);
            Unit newmonster = Pool.GetMObject(type).GetComponent<Unit>();
            newmonster.Spawn();

            yield return new WaitForSeconds(2.5f);
            
        }
        
        //      else
        //            Debug.Log("Nahhh");
    }

    private IEnumerator SpawnAMonster(int type)
    {
    Unit newantimonster = Pool.GetAMObject(type).GetComponent<Unit>();
    newantimonster.Spawn();
    yield return new WaitForSeconds(2.5f);

    }

    public void bounty(int slaintype) {
        Gold += bountyList[slaintype];
    }

    private void SpawnPortals()
    {
        GameObject tmp = (GameObject)Instantiate(tPortalPrefab, LeftSpawn, Quaternion.identity);
        tmp.transform.localScale = new Vector3(2, 2, 1);
        tmp.name = "TPortal";
        GameObject tmp2 = (GameObject)Instantiate(sPortalPrefab, RightSpawn, Quaternion.identity);
        tmp2.transform.localScale = new Vector3(1, 1, 1);
        tmp2.name = "SPortal";
    }

    public void SquidHit()
    {
        if (SquidHealth>0)
            SquidHealth -= 1;
        if (SquidHealth == 0)
        {
            WLText.text = "YOU DEFEATED THE SQUID AND WON";
        }


    }
    public void TigerHit()
    {
        WLText.text = "YOUR TIGER HAS BEEN HIT AND NOW YOU LOSE";
    }
    public void PortalDie(bool iswhite)
    {
        if (iswhite)
            WLText.text = "YOUR TIGER HAS DIED AND NOW YOU LOSE";
        else
            WLText.text = "YOU DEFEATED THE SQUID AND WON";
    }
}
