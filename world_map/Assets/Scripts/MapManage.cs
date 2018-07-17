using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapManage : Singleton<MapManage> {
    [SerializeField]
    private GameObject Home;
    [SerializeField]
    private int numberofstages;

    [SerializeField]
    private int limit_progress;
    [SerializeField]
    private GameObject explosion;

    public Vector3[] forxy;
    
    // Use this for initialization
    void Start () {
        limit_progress = 4; // ConstantManager.Manager.GetProgress()+1
        forxy = new Vector3[numberofstages];
        forxy[0] = new Vector3(Home.transform.position.x, Home.transform.position.y , Home.transform.position.z);

        

        
        for (int i = 1; i <= 5; i++)
        {
            forxy[i] = new Vector3(GameObject.Find("G1_" + i).transform.position.x, GameObject.Find("G1_" + i).transform.position.y, GameObject.Find("G1_" + i).transform.position.z);
            Debug.Log("될까G:" + forxy[i]);
        }

        for (int i = 1; i <= 5; i++)
        {
            forxy[i+5] = new Vector3(GameObject.Find("F2_" + i).transform.position.x, GameObject.Find("F2_" + i).transform.position.y, GameObject.Find("F2_" + i).transform.position.z);
            Debug.Log("될까F:"+ forxy[i] );
        }

        explosion.transform.position = forxy[limit_progress];
        Debug.Log("될까 home:limit progress" + forxy[limit_progress] + "explotion " + explosion.transform.position);//-10.8 , -3.33

    }
	
	// Update is called once per frame
	void Update () {

	}

    



}
