using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    const float  maxSpawnTime = 5;
    public float ballSpawnTime = 5.0f;
    float spawnT;

    bool firstBallSpawned = false;
    int startBallsToSpawn = 1;
    int ballsToSpawn;

    public int maxBalls = 30;
    public List<Ball> ballList;


    GameManager gameManager;
    
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if(gameManager.GameState == GameManager.State.Playing)
        {
            spawnT -= Time.deltaTime;
            if (ballList.Count < maxBalls)
            {
                if (gameManager.GameTime.seconds == 0 && gameManager.GameTime.minutes == 0 && gameManager.GameTime.hours == 0)
                {
                    if(!firstBallSpawned)
                    {
                        SpawnBall();
                        firstBallSpawned = true;
                    }
                    
                }
                if(spawnT/gameManager.GameTime.minutes+1 < 0)
                {
                    if(gameManager.Kid.coins != 0)
                    {
                        spawnT = 0;
                        ballsToSpawn = startBallsToSpawn + gameManager.GameTime.minutes;
                        SpawnBall(ballsToSpawn);
                        spawnT = ballSpawnTime;
                        gameManager.Kid.SetCoins(--gameManager.Kid.coins);
                        gameManager.Kid.coinsSpent++;
                    }
                    else
                    {
                        if(ballList.Count <= 0)
                        {
                            gameManager.Kid.AskMomCoins();
                        }
                    }
                }
            }
        }

    }

    public void ResetBallSpawner()
    {

        firstBallSpawned=false;

        DeleteAllBalls();

        ballsToSpawn = startBallsToSpawn;
        ballSpawnTime = maxSpawnTime;
        spawnT = ballSpawnTime;

    }

    public void DeleteAllBalls()
    {   
        foreach(Ball ball in ballList.ToArray()) 
        {
            ballList.Remove(ball);
            Destroy(ball.gameObject);
        }
    }

    public void SpawnBall(int nBalls = 1, float time = 0.3f)
    {
        StartCoroutine(SpawnBallT(nBalls,time));
    }

    public IEnumerator SpawnBallT(int nBalls, float time)
    {
        while (nBalls > 0)
        {
            yield return new WaitForSeconds(time);
            nBalls--;

            GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            ballList.Add(ball.GetComponent<Ball>());
        }
        StopCoroutine(SpawnBallT(nBalls, time));
    }
}
