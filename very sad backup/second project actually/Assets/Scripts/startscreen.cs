using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startscreen : MonoBehaviour {
    public void startgame (int lol)
    {
        if (lol <= ConstantManager.Manager.GetProgress() + 1)
            ConstantManager.Manager.SelectionScreen(true);
        else
            Debug.Log("trying to do " + lol + " when progress only is " + ConstantManager.Manager.GetProgress());
    }
}
