using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyHitBox : MonoBehaviour
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
}

    
    


