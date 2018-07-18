using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portals : MonoBehaviour {
    [SerializeField]
    private bool IsWhite;
    [SerializeField]
    private int max_health;
    private int curr_health;

    [SerializeField]
    private Text HealthText;

    private int Current_Health
    {
        get
        {
            return curr_health;
        }

        set
        {
            curr_health = value;
            this.HealthText.text = value.ToString();
        }
    }

    // Use this for initialization
    void Start () {
        Current_Health = max_health;
	}
    public void OnHit(int damage)
    {
        if (Current_Health > damage)
        {
            Current_Health -= damage;
        }
        else {
            gamescript.instance.PortalDie(IsWhite);

        }
    }

}
