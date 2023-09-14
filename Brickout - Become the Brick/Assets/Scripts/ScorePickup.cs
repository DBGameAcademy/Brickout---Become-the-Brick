using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScorePickup : MonoBehaviour
{

    GameManager gameManager;

    public float pickupDistance = 3.0f;
    public int baseValue = 10;
    public int currValue;

    public float rotationSpeed = 10f;

    public Vector2 spawnRange = new Vector2(8,3);

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.GameState == GameManager.State.Playing)
        {
            //transform.Rotate(0,0,1 *  rotationSpeed * Time.deltaTime);
            Pickup();
        }
    }

    public void Pickup()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, gameManager.Player.transform.position);

        if ( distanceFromPlayer <= pickupDistance)
        {
            Vector3 moveDir = new Vector3(gameManager.Player.transform.position.x - transform.position.x, gameManager.Player.transform.position.y - transform.position.y).normalized;
            
            transform.Translate(moveDir * rotationSpeed * Time.deltaTime);

            if(distanceFromPlayer <= 0.2f)
            {
                currValue += gameManager.GameTime.minutes * 10;
                gameManager.Player.AddPoints(currValue);
                //play sound or effects here

                SetRandomPosition();
            }
        }
    }

    public void SetRandomPosition()
    {
        transform.position = new Vector3(Random.Range(-spawnRange.x, spawnRange.x), Random.Range(-spawnRange.y, spawnRange.y));
    }

    public void ResetScorePickup()
    {
        SetRandomPosition();
        currValue = baseValue;
    }
}
