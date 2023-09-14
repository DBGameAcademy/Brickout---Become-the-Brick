using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//This script should be a attached to a child object of gameManager!

public class InputController : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = transform.parent.GetComponent<GameManager>();
    }

    private void Start()
    {
        if (gameManager == null)
        {
             Debug.LogError("[InputController]: Where the fuck is the Game Manager?\nGM should be the parent of InputController!");
        }
       
    }
    void Update()
    {
        if(gameManager!=null)
        {
            switch (gameManager.GameState)
            {
                case GameManager.State.None: { break; }
                case GameManager.State.Playing: 
                    { 
                        //Player Movements
                        if(Input.GetKey(KeyCode.A) ||  Input.GetKey(KeyCode.LeftArrow))
                        {
                            gameManager.Player.MoveLeft();
                        }
                        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                        {
                            gameManager.Player.MoveRight();
                        }
                        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                        {
                            gameManager.Player.MoveDown();
                        }
                        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                        {
                            gameManager.Player.MoveUp();
                        }

                        //UI
                        if(Input.GetKeyDown(KeyCode.Escape))
                        {
                            gameManager.PauseGame();
                        }

                        break; 
                    }
                case GameManager.State.Paused: {
                        
                        if(Input.anyKeyDown && !(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)))
                        {
                            //getting any keyboard keys
                            gameManager.UnpauseGame();
                        }

                        break; 
                    }
                case GameManager.State.GameOver: { break; }
                default: { break; } //do nothing

            }
        }
    }
}
