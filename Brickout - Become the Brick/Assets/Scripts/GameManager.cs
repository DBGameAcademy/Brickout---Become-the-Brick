using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioController AudioController;
    public InputController InputController;
    public UIController UIController;

    public Player Player;
    public Kid Kid;
    public ScorePickup ScorePickup;
    public enum State
    {
        None,
        Playing,
        Paused,
        GameOver,
    }
    
    private State gameState; //<---private var
    //Game state Property
    public State GameState { get { return gameState; } } //<---property field 

    public struct Timer
    {
        public int seconds;
        public int minutes;
        public int hours;
    }
    Timer gameTime;
    public Timer GameTime { get { return gameTime; } }

    private void Start()
    {
        UIController.ShowUIPanel(UIController.newGamePanel);
    }

    private void Update()
    {

    }

    IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (gameState == State.Playing)
            {
                //update the timer. 
                gameTime.seconds += 1;
                if (gameTime.seconds > 59)
                {
                    gameTime.seconds = 0;
                    gameTime.minutes++;
                    if (gameTime.minutes > 59)
                    {
                        gameTime.minutes = 0;
                        gameTime.hours++;
                    }
                }

                //update the UI
                UIController.UpdateUIText("TimeText", TimeToString());
            }
        }
    }

    public string TimeToString()
    {
        //formatting the time text to send to UI
        string s = gameTime.seconds.ToString();
        string m = gameTime.minutes.ToString();
        string h = gameTime.hours.ToString();
        
        if (gameTime.seconds < 10)
        {
            s = "0" + s;
        }
        if (gameTime.minutes < 10)
        {
            m = "0" + m;
        }
        if (gameTime.hours < 10)
        {
            h = "0" + h;
        }
        string timeString = $"{h}:{m}:{s}";
        return timeString;
    }

    public void ResetTimer()
    {
        //reset the timer
        gameTime.hours = 0; gameTime.minutes = 0; gameTime.seconds = 0;
        UIController.UpdateUIText("TimeText", TimeToString());
    }

    public void SetTimer(int _hours, int _minutes, int _seconds)
    {
        //Sets The timer with the given params.
        gameTime.hours = _hours;
        gameTime.minutes = _minutes;
        gameTime.seconds = _seconds;

        UIController.UpdateUIText("TimeText", TimeToString());
    }

    public void SetGameState(State _state)
    {
        gameState = _state;
    }

    public void PauseGame()
    {
        //Pause the game
        gameState = State.Paused;
        UIController.ShowUIPanel(UIController.pausePanel);
        Time.timeScale = 0; 
    }
    public void UnpauseGame()
    {
        //Unpause the game
        gameState = State.Playing;
        UIController.HideUIPanel(UIController.pausePanel);
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        StopAllCoroutines();
        gameState = State.Playing;
        UnpauseGame();

        //hide ui panels
        UIController.HideUIPanel(UIController.newGamePanel);
        UIController.HideUIPanel(UIController.pausePanel);
        UIController.HideUIPanel(UIController.gameOverPanel);

        //start music
        AudioController.PlayMusic(AudioController.musicList[0]);

        //starts the game
        ResetTimer();
        Kid.ResetKid();
        Player.ResetPlayer();
        ScorePickup.ResetScorePickup();

        StartCoroutine(UpdateTimer());
    }

    

    public void GameOver()
    {
        //GameOver
        gameState = State.GameOver;
        UIController.UpdateUIText("GameOverTextStats_coins", Kid.coinsSpent.ToString());
        UIController.UpdateUIText("GameOverTextStats_time", TimeToString());
        Player.TotalScore();
        UIController.ShowUIPanel(UIController.gameOverPanel);
    }
}
