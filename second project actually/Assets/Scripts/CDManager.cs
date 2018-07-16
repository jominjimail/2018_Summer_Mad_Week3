using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDManager : Singleton<CDManager>
{

    [SerializeField]
    private Image [] CDImages;
    private int NumberOfCooldowns;

    private bool [] coolingDown;
    private bool [] doit;
    
    private float [] waitingTime;

    // Update is called once per frame
    void Start()
    {
        NumberOfCooldowns = gamescript.Instance.TypesofMonsters;
        coolingDown = new bool[NumberOfCooldowns];
        doit = new bool[NumberOfCooldowns];
        waitingTime = gamescript.Instance.getCDTimes();
    }
    void Update()
    {
        for (int i = 0; i < NumberOfCooldowns; i++) { 
            if (doit[i])
            {
                CDImages[i].fillAmount = 1;
                doit[i] = false;
            }
            if (coolingDown[i] == true)
            {
                //Reduce fill amount over 30 seconds
                CDImages[i].fillAmount -= 1.0f / waitingTime[i] * Time.deltaTime;
                if (CDImages[i].fillAmount < 0.0001)
                    coolingDown[i] = false;
            }
        }
    }

    public void PutOnCD(int cdme)
    {
        doit[cdme] = true;
        coolingDown[cdme] = true;
    }
}
