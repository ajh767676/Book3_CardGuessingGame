using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public void GoToIntro()
    {
        SceneManager.LoadScene("intro");
    }

    public void GoToPreferences()
    {
        SceneManager.LoadScene("preferences");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("game");
    }

    public void GoToExit()
    {
        SceneManager.LoadScene("exit");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}