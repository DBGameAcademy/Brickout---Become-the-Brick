using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gameManager;
    public Transform deadlyArea;

    SpriteRenderer spriteRenderer;

    public Vector3 startPos = Vector3.zero;

    public int startLifes = 3;
    public int lifes;

    public float startSpeed = 3.0f;
    public float speed;

    public int pickupCount;
    public int points;

    public int startTotalScore = 0;
    public int totalScore;

    public Vector2 startSize = new Vector2(0.8f, 0.3f);
    public Vector2 size;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //player get killed if pass through the red line
        if(transform.position.y < deadlyArea.position.y)
        {
            KillPlayer();
        }
    }

    public void MoveLeft()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    public void MoveRight()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    public void MoveUp()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    public void MoveDown()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    public void OnHit()
    {
        lifes--;
        SetColor(lifes);
        SetSize(lifes);
        if (lifes <= 0)
        {
            KillPlayer();
        }

        //updating ui lifes.
        gameManager.UIController.UpdateUIText("LifesText", lifes.ToString());

        //add hits effects here
    }

    public void AddPoints(int p)
    {
        pickupCount++;
        points += p;
        gameManager.UIController.UpdateUIText("GameOverTextStats_pickups", pickupCount.ToString());
    }
    public void TotalScore()
    {
        totalScore = (int)((1+gameManager.GameTime.hours) * (1 + gameManager.GameTime.minutes) * ((1 + (points+1) * (gameManager.Kid.coinsSpent+1)/3)));
        gameManager.UIController.UpdateUIText("GameOverTextStats_totalScore", totalScore.ToString());

    }


    public void KillPlayer()
    {
        lifes = 0;
        gameManager.UIController.UpdateUIText("LifesText", lifes.ToString());

        //add death effect here


        gameManager.GameOver();
    }

    public void SetColor(int nC)
    {
        Color sColor = new Color();
        switch (nC)
        {
            case 0: { break; }
            case 1: { sColor = Color.red; break; }
            case 2: { sColor = Color.yellow; break; }
            case 3: { sColor = Color.green; break; }
            default: { sColor = Color.green; break; }
        }
        spriteRenderer.color =  sColor;
    }

    public void SetSize(int sizeN)
    {
        Vector2 nSize = startSize;
        switch (sizeN)
        {
            case 0: { nSize = Vector2.zero; break; }
            case 1: { nSize.x = 0.3f; nSize.y = startSize.y; break; }
            case 2: { nSize.x = 0.5f; nSize.y = startSize.y; break; }
            case 3: { nSize.x = startSize.x; nSize.y = startSize.y; break; }
            default: { nSize = startSize; break; }
        }
        spriteRenderer.size = nSize;
    }

    public void ResetPlayer()
    {

        transform.position = startPos;
        lifes = startLifes;
        speed = startSpeed;
        points = 0;
        pickupCount = 0;
        totalScore = 0;

        SetColor(lifes);
        SetSize(lifes);
        gameManager.UIController.UpdateUIText("LifesText", lifes.ToString());

    }
}
