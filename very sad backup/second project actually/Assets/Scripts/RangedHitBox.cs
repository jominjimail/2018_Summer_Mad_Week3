using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedHitBox : MonoBehaviour
{

    [SerializeField]
    private GameObject unitObject;

    Unit myUnit;
    void Start()
    {
        myUnit = unitObject.GetComponent<Unit>();
    }
    public Unit getUnitFromHitbox()
    {
        return myUnit;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        myUnit.MyRangeHitSomething(other);
    }
}
