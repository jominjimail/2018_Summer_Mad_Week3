using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    [SerializeField]
    private GameObject[] MobjectPrefabs;
    [SerializeField]
    private GameObject SpellPrefab;
    [SerializeField]
    private GameObject[] AMobjectPrefabs;

    public GameObject GetMObject(int type)
    {
        GameObject newObject = Instantiate(MobjectPrefabs[type]);
        switch (type) {
            case 0:
                newObject.name = "SIU";
                break;
            case 1:
                newObject.name = "ZUCC";
                break;
            default:
                newObject.name = "monsterType" + type.ToString();
                break;
    }
        return newObject;
    }

    public GameObject GetAMObject(int type)
    {
        GameObject newObject = Instantiate(AMobjectPrefabs[type]);
        switch (type) {
            case 0:
                newObject.name = "ANTISIU";
                break;
            default:
                newObject.name = "antimonsterType" + type.ToString();
                break;
        }
        return newObject;
    }
    public GameObject GetSpell()
    {
        GameObject newObject = Instantiate(SpellPrefab);
        newObject.name = "spell";
        return newObject;
    }
}
