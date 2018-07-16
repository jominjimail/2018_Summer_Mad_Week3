﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    //shows which side
    [SerializeField]
    private bool IsWhite;
    public bool IsPortal;

    //shows the state
    public bool IsMoving { get; set; }
    public bool IsFighting { get; set; }
    public bool HitPortal { get; set; }
    public bool IsAlive { get; set; }

    //for movement
    [SerializeField]
    private float speed;
    private Vector3 currentDestination;

    //for fighting
    private Unit opponent;
    private List<Unit> nextopponents;

    [SerializeField]
    private int max_health;
    private int curr_health;

    [SerializeField]
    private float attackspeed;
    [SerializeField]
    private int damage;
    
    [SerializeField]
    private float attackanimationtime;

    private float lastattackedtime;
    [SerializeField]
    private int MyType;

    //for display
    [SerializeField]
    private Text HealthText;


    void Start()
    {
        lastattackedtime = -attackspeed;
        nextopponents = new List<Unit>();
        Current_Health = max_health;
        HitPortal = false;
        IsAlive = true;
    }

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


    public void Spawn()
    {
        IsPortal = false;
        opponent = null;
        if (IsWhite)
        {
            currentDestination = gamescript.GetRSpawnLoc();
            transform.position = gamescript.GetLSpawnLoc();
        }
        else
        {
            currentDestination = gamescript.GetLSpawnLoc();
            transform.position = gamescript.GetRSpawnLoc();
        }
        IsMoving = true;
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));
    }

    private void Update()
    {
        if (IsAlive)
        {
            if (IsMoving)
                Move();
            if (IsFighting && opponent != null)
                Fight(opponent);
            else if (nextopponents.Count > 0)
            {
                opponent = nextopponents[0];
                nextopponents.Remove(opponent);
                IsFighting = true;
                Fight(opponent);
            }
            else
            {
                if (IsMoving == false)
                {
                    IsMoving = true;
                }
            }
        }
    }

    private void Move()
    {
        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, speed * Time.deltaTime);
        }

        
    }

    private void Fight(Unit opp)
    {
        
        if (curr_health < 0)
        {
            Debug.Log("This shouldn't happen Code:1 ");
        }
        // when time is just above attackspeed, the attack animation should start
        // when time is exceeds attackspeed + attackanimationtime, the damage should happen.
        if (Time.time > lastattackedtime + attackspeed)
        {
            lastattackedtime = Time.time;
            StartCoroutine(Attack(opp));
        }
    }

    private IEnumerator Attack(Unit opp)
    {
        yield return new WaitForSeconds(attackanimationtime);
        if (IsAlive && opp.IsAlive)
        {
            if (!opp.onHit(damage))
            {
                IsFighting = false;
            }
        }
    }

    public bool onHit(int damage)
    {
        if (curr_health > 0)
        {
            if (curr_health <= damage)
            {
                StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));
                IsAlive = false;
                IsFighting = false;
                if (IsPortal)
                    gamescript.instance.PortalDie(IsWhite);
                else if (!IsWhite)
                {
                    gamescript.Instance.bounty(MyType);
                }
                
                return false;
            }
            Current_Health -= damage;
            return true;
        }
        return false;

    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        float Progress = 0;

        while (Progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, Progress);
            Progress += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;

        if (remove)
            Destroy(gameObject);
    }

    public void MyRangeHitSomething(Collider2D other)
    {
        if (IsAlive)
        {
            string portaltag;
            string enemytag;
            if (IsWhite)
            {
                portaltag = "SPortal";
                enemytag = "AntiMonsters";
            }
            else
            {
                portaltag = "TPortal";
                enemytag = "Monsters";
                
            }

            // to Keep or not??
            // Hit the thing or what man
            if (other.tag == portaltag)
            {
                Unit enemy = other.GetComponent<Unit>();
                IsMoving = false;
                IsFighting = true;
                HitPortal = true;
                if (opponent == null)
                {
                    opponent = enemy;
                }
                else
                {
                    nextopponents.Add(enemy);
                }
            }

            if (other.tag == enemytag) {
                Unit enemy = other.GetComponent<BodyHitBox>().getUnitFromHitbox();
                
                if (enemy.IsAlive)
                {
                    IsMoving = false;
                    IsFighting = true;
                    if (opponent == null)
                        opponent = enemy;
                    else {
                        if (HitPortal)
                        {
                            HitPortal = false;
                            nextopponents.Add(opponent);
                            opponent = enemy;
                        }
                        else
                        {
                            nextopponents.Add(enemy);
                        }
                     }
                }
            }
        }
    }


}
