using UnityEngine;

public class Tile : MonoBehaviour
{
    public Sprite originalSprite;
    public Sprite hiddenSprite;

    private SpriteRenderer spriteRenderer;
    private ManageCards gameManager;

    public int CardValue { get; private set; }
    public bool IsRevealed { get; private set; }

    public string RowName
    {
        get
        {
            if (gameObject.name.StartsWith("Row1"))
                return "Row1";

            return "Row2";
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<ManageCards>();
    }

    private void Start()
    {
        HideCard();
    }

    public void Configure(Sprite cardFace, int cardValue)
    {
        originalSprite = cardFace;
        CardValue = cardValue;
        HideCard();
    }

    public void HideCard()
    {
        spriteRenderer.sprite = hiddenSprite;
        IsRevealed = false;
    }

    public void RevealCard()
    {
        spriteRenderer.sprite = originalSprite;
        IsRevealed = true;
    }

    private void OnMouseDown()
    {
        gameManager.CardSelected(this);
    }
}