using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    GameManager gameManager;
    BallSpawner ballSpawner;
    public Vector3 startPos = new Vector3(0, -4.2f, 0);
    public float startSpeed = 5f;
    public float speed;
    public float targetRange = 3f;
    public Ball currTargetBall;
    public float centerMargin = 0.7f;

    Vector2 xDir; //direction of x movement
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ballSpawner =  gameObject.GetComponentInChildren<BallSpawner>();
    }

    private void Update()
    {
        if(gameManager.GameState == GameManager.State.Playing)
        {
            //follow the currtargetball y speed is negative
            if (currTargetBall != null)
            {
                if (currTargetBall.direction.y > 0)
                {
                    currTargetBall = null;
                    MoveToCenter();
                }
                else
                {
                    MoveToBall(currTargetBall);
                }

            }
            else
            {
                MoveToCenter();
                for (int i = 0; i < ballSpawner.ballList.Count; i++)
                {
                    if (ballSpawner.ballList[i].direction.y < 0 && ballSpawner.ballList[i].transform.position.y < targetRange)
                    {
                        currTargetBall = ballSpawner.ballList[i];
                    }
                }
            }
        }

    }

    public void MoveToBall(Ball _b)
    {
        //move towards the ball
        xDir = new Vector2(_b.transform.position.x - transform.position.x, 0).normalized; //getting the direction
        transform.Translate(xDir * speed * Time.deltaTime);
    }

    public void MoveToCenter()
    {
        //moves towards the center
        if(transform.position.x < -centerMargin || transform.position.x > centerMargin)
        {
            xDir = new Vector2(0 - transform.position.x, 0).normalized; //getting the direction
            transform.Translate(xDir * speed * Time.deltaTime);
        }

    }

    public void OnHit()
    {
        currTargetBall = null;
    }

    public void ResetPaddle()
    {
        speed = startSpeed;
        transform.position = startPos;
    }
}
