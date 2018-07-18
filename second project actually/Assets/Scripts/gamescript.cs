using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamescript : Singleton<gamescript> {

    public static gamescript instance;

    //for backgroudn init
    public ObjectPool Pool { get; set; }
    public Portals TPortal { get; set; }


    public static Vector3 LeftSpawn = new Vector3(-14, -10.2f, 0);
    public static Vector3 RightSpawn = new Vector3(16, -10.2f, 0);


    //for UI
    private int gold;
    [SerializeField]
    private Text goldText;
    private int sHealth;

    [SerializeField]
    private Text WLText;

    //Victory
    private bool Victory = false;


    //for number of Black Unit Types
    [SerializeField]
    private int[] bountyList;
    
        //for spawning, done periodically atm
        private float antisiustart;
        private float antisiucd;

    private float pgoldstart;
    private float pgoldcd;


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

        //Debug.Log("You have chosen difficulty " + ConstantManager.Instance.difficulty + ". Good Luck!");

        instance = this;
        SpawnPortals();
        Gold = 150;

        pgoldcd = 0.2f;
        pgoldstart = -pgoldcd;

        TypesofMonsters = CDTimes.Length;
        LastCastCDTimes = new float[TypesofMonsters];
        for (int i =0; i<TypesofMonsters; i++)
        {
            LastCastCDTimes[i] = -CDTimes[i];
        }


        antisiucd = 10 - (int) (ConstantManager.Manager.GetDifficulty() / 2);
        antisiustart = - antisiucd;
    }


    
    // Update is called once per frame
    void Update() {
        if (!Victory && Time.time > antisiustart + antisiucd)
        {
            antisiustart = Time.time;
            int lol = (int) Random.Range(0, 2);
            StartCoroutine(SpawnAMonster(lol));
        }

        PassiveGold();
    }

    private void PassiveGold()
    {
        if (Time.time > pgoldstart + pgoldcd)
        {
            Gold += 1;
            pgoldstart = Time.time;
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

    public void CastMySpell()
    {
        
        if (Time.time > LastCastCDTimes[TypesofMonsters-1] + CDTimes[TypesofMonsters-1])
        {
            LastCastCDTimes[TypesofMonsters - 1] = Time.time;
            FreezeWind spell = Pool.GetSpell().GetComponent<FreezeWind>();
            spell.Spawn();
            CDManager.Instance.PutOnCD(TypesofMonsters-1);
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
        Debug.Log("bounty multiplied by " + (ConstantManager.Manager.GetDifficulty() / 4 + 1));
        Gold += bountyList[slaintype] * (ConstantManager.Manager.GetDifficulty()/4 +1);
    }

    private void SpawnPortals()
    {
        GameObject tmp = (GameObject)Instantiate(tPortalPrefab, LeftSpawn, Quaternion.identity);
        tmp.name = "TPortal";
        GameObject tmp2 = (GameObject)Instantiate(sPortalPrefab, RightSpawn, Quaternion.identity);
        tmp2.name = "SPortal";
    }


    
    public void PortalDie(bool iswhite)
    {
        if (iswhite)
        {
            WLText.text = "YOU LOSE";
            StartCoroutine(GoBackToSelection());
        }
        else
        {
            WLText.text = "<color=white>YOU WON</color>";
            Victory = true;
            ConstantManager.Manager.SetProgress(ConstantManager.Manager.GetDifficulty());
            StartCoroutine(GoBackToSelection());
        }
    }

    private IEnumerator GoBackToSelection()
    {
        yield return new WaitForSeconds(2f);
        ConstantManager.Manager.SelectionScreen(false);
    }
}
