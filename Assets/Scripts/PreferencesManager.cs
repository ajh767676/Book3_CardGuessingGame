using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreferencesManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Dropdown cardCountDropdown;
    public TMP_Dropdown timeDropdown;

    private readonly int[] cardOptions = { 8, 12, 16, 20 };
    private readonly int[] timeOptions = { 30, 60, 90, 120, 180 };

    public void SavePreferencesAndStart()
    {
        string playerName = playerNameInput.text.Trim();

        if (playerName == "")
        {
            playerName = "Player";
        }

        int cardCount = cardOptions[cardCountDropdown.value];
        int timeLimit = timeOptions[timeDropdown.value];

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("CardCount", cardCount);
        PlayerPrefs.SetInt("TimeLimit", timeLimit);
        PlayerPrefs.Save();

        SceneManager.LoadScene("game");
    }
}