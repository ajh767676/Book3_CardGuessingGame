using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageCards : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform centerOfScreen;
    public Sprite[] cardFaces;

    public TMP_Text playerNameText;
    public TMP_Text timerText;

    private Tile firstCard;
    private bool checkingCards;
    private bool gameEnded;

    private int numberOfMatches;
    private int totalPairs;
    private float timeRemaining;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        int sampleRate = 44100;
        int sampleCount = sampleRate / 5;
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float time = (float)i / sampleRate;
            float fade = 1f - ((float)i / sampleCount);

            samples[i] =
                Mathf.Sin(2f * Mathf.PI * 880f * time)
                * fade * 0.25f;
        }

        AudioClip matchSound = AudioClip.Create(
            "MatchSound", sampleCount, 1, sampleRate, false);

        matchSound.SetData(samples, 0);
        audioSource.clip = matchSound;
    }

    private void Start()
    {
        string playerName =
            PlayerPrefs.GetString("PlayerName", "Player");

        int cardCount =
            PlayerPrefs.GetInt("CardCount", 20);

        timeRemaining =
            PlayerPrefs.GetInt("TimeLimit", 60);

        totalPairs = cardCount / 2;

        playerNameText.text = "Player: " + playerName;
        UpdateTimerText();

        CreateRow(1.4f, "Row1", totalPairs);
        CreateRow(-1.4f, "Row2", totalPairs);
    }

    private void Update()
    {
        if (gameEnded)
            return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerText();

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            FinishGame("lost");
        }
    }

    private void UpdateTimerText()
    {
        timerText.text =
            "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    private void CreateRow(
        float yPosition, string rowName, int numberOfCards)
    {
        List<int> cardValues = new List<int>();

        for (int i = 0; i < numberOfCards; i++)
            cardValues.Add(i);

        Shuffle(cardValues);

        float spacing = 1.3f;
        float startingX =
            -((numberOfCards - 1) * spacing) / 2f;

        for (int i = 0; i < cardValues.Count; i++)
        {
            int value = cardValues[i];

            Vector3 position = centerOfScreen.position +
                new Vector3(
                    startingX + (i * spacing),
                    yPosition,
                    0);

            GameObject newCard = Instantiate(
                tilePrefab, position, Quaternion.identity);

            newCard.name = rowName + "_Card_" + value;

            Tile tile = newCard.GetComponent<Tile>();
            tile.Configure(cardFaces[value], value);
        }
    }

    public void CardSelected(Tile selectedCard)
    {
        if (gameEnded ||
            checkingCards ||
            selectedCard.IsRevealed)
            return;

        if (firstCard != null &&
            firstCard.RowName == selectedCard.RowName)
            return;

        selectedCard.RevealCard();

        if (firstCard == null)
        {
            firstCard = selectedCard;
        }
        else
        {
            StartCoroutine(CheckCards(selectedCard));
        }
    }

    private IEnumerator CheckCards(Tile secondCard)
    {
        checkingCards = true;

        yield return new WaitForSeconds(1f);

        if (firstCard.CardValue == secondCard.CardValue)
        {
            audioSource.Play();

            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);

            numberOfMatches++;

            if (numberOfMatches == totalPairs)
            {
                FinishGame("won");
                yield break;
            }
        }
        else
        {
            firstCard.HideCard();
            secondCard.HideCard();
        }

        firstCard = null;
        checkingCards = false;
    }

    public void StopGame()
    {
        if (!gameEnded)
            FinishGame("stopped");
    }

    private void FinishGame(string result)
    {
        gameEnded = true;

        PlayerPrefs.SetInt(
            "CurrentScore", numberOfMatches);

        PlayerPrefs.SetString(
            "GameResult", result);

        PlayerPrefs.Save();

        SceneManager.LoadScene("exit");
    }

    private void Shuffle(List<int> values)
    {
        for (int i = values.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            int temporaryValue = values[i];
            values[i] = values[randomIndex];
            values[randomIndex] = temporaryValue;
        }
    }
}