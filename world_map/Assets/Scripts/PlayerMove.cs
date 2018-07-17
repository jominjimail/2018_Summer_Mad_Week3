using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MapManage
{
    //-10.76 , -1.72
    // Use this for initialization
    public Vector3[] list;
    //public float curPosition;
    public int curindex;
    public int speed ;
    public Vector3 targetposition;
    public float fMove ;

    private Vector3 curr_destination;
    private int limit_progress;


    void Start()
    {
        //start home 에서 시작 home 의 x축을 받아온다.  
        list = MapManage.Instance.forxy;
        transform.position = new Vector3(list[0].x, transform.position.y, transform.position.z);
        limit_progress = 4;//ConstantManager.Manager.GetProgress()+1
        //curPosition = list[0].x;
        curr_destination = transform.position;
        //curindex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (curr_destination.x != transform.position.x) //입력을 받지 않는다.
        {
            move(curr_destination);
        }

        else // 입력을 받는다.
        {
            // 현위치 매번 update 마다 찾기 갱신
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].x == transform.position.x)
                {
                    curindex = i;
                }
            }

            // 현재 위치 애니메이션 추가

            if (Input.GetAxis("Horizontal") > 0 && curindex <limit_progress) //right
            {// 방향키 대로 캐릭터 이동 오른쪽으로
               
                if (list[curindex + 1] != null)
                {
                    curr_destination = list[curindex + 1];

                }


            }
            else if (Input.GetAxis("Horizontal") < 0 && curindex <= limit_progress) //left
            {// 방향키 대로 캐릭터 이동 왼쪽으로 
                
                if (list[curindex - 1] != null)
                {
                    curr_destination = list[curindex - 1];
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space)) //space
            {
                
                if (curr_destination.x == list[0].x)
                {
                    //Home 에서 space를 눌렀을 경우 아무런 액션 없음 
                }else
                {
                    //map 에서 space를 눌렀을 경우
                    //애니 메이션 추가
                    //게임 play scene 으로 전환

                    
                }
               
            }
            
        }
    }//update

    void move(Vector3 _target)
    {
        fMove = Time.deltaTime * speed;
        float newXPosition = _target.x;
        float newYPosition = _target.y;
        targetposition = new Vector3(newXPosition, newYPosition+1, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetposition, speed * Time.deltaTime);
        
    }

}//class
