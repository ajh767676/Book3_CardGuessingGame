using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour
{
    public TMP_Text resultText;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public GameObject saveScoreButton;

    private int currentScore;
    private int highScore;
    private string playerName;

    private void Start()
    {
        playerName =
            PlayerPrefs.GetString("PlayerName", "Player");

        currentScore =
            PlayerPrefs.GetInt("CurrentScore", 0);

        highScore =
            PlayerPrefs.GetInt("HighScore", 0);

        string result =
            PlayerPrefs.GetString("GameResult", "stopped");

        if (result == "won")
        {
            resultText.text =
                "Great job, " + playerName +
                "! You matched all the cards!";
        }
        else if (result == "lost")
        {
            resultText.text =
                "Time ran out, " + playerName + ".";
        }
        else
        {
            resultText.text =
                playerName + " ended the game early.";
        }

        scoreText.text =
            "Your Score: " + currentScore;

        highScoreText.text =
            "Highest Score: " + highScore;

        saveScoreButton.SetActive(
            currentScore > highScore);
    }

    public void SaveHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;

            PlayerPrefs.SetInt(
                "HighScore", highScore);

            PlayerPrefs.SetString(
                "HighScorePlayer", playerName);

            PlayerPrefs.Save();

            highScoreText.text =
                "Highest Score: " + highScore +
                " by " + playerName;

            saveScoreButton.SetActive(false);
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("preferences");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}