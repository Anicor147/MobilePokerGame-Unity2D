using System;
using SOScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Enseigne
{
    Coeur,
    Pique,
    Carreau,
    Trefle,
    Joker
}

public class Carte : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text valeurText;
    [SerializeField] private TMP_Text enseigneText;
    [SerializeField] private TMP_Text heldText;
    [SerializeField] private CardsSO _cardsSo;
    [SerializeField] private SpecialCardsSO _specialCardsSo;
    [SerializeField] private Image cardImage;
    private bool isHeld;
    private Color color;

    public static event Action<Image> OnCardFrontChanged;
    public CarteData Data { get; private set; }

    public CardsSO CardsSo
    {
        get => _cardsSo;
        set => _cardsSo = value;
    }

    public bool IsHeld
    {
        get => isHeld;
        set => isHeld = value;
    }

    private void Awake()
    {
        heldText.enabled = false;
    }

    internal void SetData(CarteData carteData)
    {
        Data = carteData;
        switch (carteData.enseigne)
        {
            case Enseigne.Coeur:

                color = Color.red;
                cardImage.sprite = _cardsSo.spritesHearth[carteData.valeur - 1];
                carteData.color = color;
                break;

            case Enseigne.Pique:
                color = Color.black;

                cardImage.sprite = _cardsSo.spritesSpade[carteData.valeur - 1];
                carteData.color = color;
                break;

            case Enseigne.Carreau:
                color = Color.red;
                cardImage.sprite = _cardsSo.spritesDiamond[carteData.valeur - 1];
                carteData.color = color;
                break;

            case Enseigne.Trefle:
                cardImage.sprite = _cardsSo.spritesClover[carteData.valeur - 1];
                color = Color.black;
                carteData.color = color;
                break;

            case Enseigne.Joker:
                cardImage.sprite = _specialCardsSo.JokerCards[carteData.valeur];
                if (carteData.valeur == 0)
                {
                    color = Color.black;
                    carteData.color = color;
                }
                else
                {
                    color = Color.red;
                    carteData.color = color;
                }

                break;
        }

        OnCardFrontChanged?.Invoke(cardImage);
        enseigneText.color = color;
        valeurText.color = color;
    }

    internal void ShowData()
    {
        Debug.Log($" Enseigne : {Data.enseigne} Valeur : {Data.valeur}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.CurrentHeld >= 0)
            {
                isHeld = !isHeld;
                GameManager.Instance.CurrentHeld--;
                Debug.Log(GameManager.Instance.CurrentHeld);
                heldText.enabled = isHeld;
            }
        }
        else
        {
            isHeld = !isHeld;
            heldText.enabled = isHeld;
        }
    }

    public void ClearHeld()
    {
        GameManager.Instance.CurrentHeld = GameManager.Instance.MaxHeld;
        isHeld = false;
        heldText.enabled = false;
    }
}