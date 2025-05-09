using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() 
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();    
    }

    public int GetPlayerLives()
    {
        return playerLives;
    }
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            StartCoroutine(ResetGameSession());
            // ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString(); 
    }

    void TakeLife()
    {
        playerLives--;
        // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    IEnumerator ResetGameSession()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
