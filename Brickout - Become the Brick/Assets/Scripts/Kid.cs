using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    GameManager gameManager;

    public Paddle Paddle;
    public BallSpawner BallSpawner;

    public int coinsSpent = 0;

    public int startCoins;
    public int coins;

    public int momMinCoins = 10;
    public int momMaxCoins = 30;

    public int startWaitTime = 5;
    public int waitTime;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        
    }

    public void AskMomCoins()
    {
        gameManager.UIController.ShowUIPanel(gameManager.UIController.kidPanel);
        StartCoroutine(KidTimer());
        gameManager.SetGameState(GameManager.State.None);
    }

    public void SetCoins(int n)
    {
        coins = n;
        gameManager.UIController.UpdateUIText("KidCoinText", coins.ToString());
    }

    IEnumerator KidTimer()
    {
        while (true)
        {
            waitTime--;
            gameManager.UIController.UpdateUIText("KidTimerText", waitTime.ToString());
            

            //addkids coins and manage difficulty progression
            if (waitTime <= 0)
            {
                gameManager.UIController.HideUIPanel(gameManager.UIController.kidPanel);
                SetCoins(Random.Range(momMinCoins, momMaxCoins + 1));

                //add powerups here

                gameManager.SetGameState(GameManager.State.Playing);
                waitTime = startWaitTime;
                StopCoroutine(KidTimer());
                break;
            }
            yield return new WaitForSeconds(1f);

        }




    }

    public void ResetKid()
    {
        coinsSpent = 0;
        waitTime = startWaitTime;
        coins = startCoins;
        Paddle.ResetPaddle();
        BallSpawner.ResetBallSpawner();
        gameManager.UIController.UpdateUIText("KidTimerText", waitTime.ToString());
        gameManager.UIController.UpdateUIText("KidCoinText", coins.ToString());
    }
}
