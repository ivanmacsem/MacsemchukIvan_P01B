using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpd = 6f;
    private int basicMoveStage = 0;
    public float curTime = 0f;

    private Vector3 moveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        curTime += Time.deltaTime;
        curTime %= 10;
        if(curTime<4){
            basicMoveStage=0;
        }
        else if(curTime<5){
            basicMoveStage=1;
        }
        else if(curTime<9){
            basicMoveStage=2;
        }
        else if(curTime<10){
            basicMoveStage=3;
        }

        switch (basicMoveStage)
        {
            case 0:
                moveDirection=Vector3.forward;
            break;
            case 1:
                moveDirection=Vector3.left;
            break;
            case 2:
                moveDirection=Vector3.back;
            break;
            default:
                moveDirection=Vector3.right;
            break;
        }
        rb.velocity = new Vector3(moveDirection.x * moveSpd, moveDirection.y * moveSpd, 0);
    }
}
