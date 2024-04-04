using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Enseigne
{
    Coeur,
    Pique,
    Carreau,
    Trefle
}

public class Carte : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text valeurText;
    [SerializeField] private TMP_Text enseigneText;
    [SerializeField] private TMP_Text heldText;
    private bool isHeld;

    private Color color; 
    public CarteData Data { get; private set; }

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

        string valeurString = carteData.valeur.ToString();

        switch (carteData.valeur)
        {
            case 1:
                valeurString = "A";
                break;
            case 11:
                valeurString = "J";
                break;
            case 12:
                valeurString = "Q";
                break;
            case 13:
                valeurString = "K";
                break;
        }


        switch (carteData.enseigne)
        {
            case Enseigne.Coeur:

                color = Color.red;
                carteData.color = color;
                enseigneText.text = "\u2665";
                break;

            case Enseigne.Pique:
                color = Color.black;
                carteData.color = color;
                enseigneText.text = "\u2660";
                break;

            case Enseigne.Carreau:
                color = Color.red;
                carteData.color = color;
                enseigneText.text = "\u2666";
                break;

            case Enseigne.Trefle:
                color = Color.black;
                carteData.color = color;
                enseigneText.text = "\u2663";
                break;
        }

        enseigneText.color = color;
        valeurText.color = color;

        //valeurText.text = carteData.valeur.ToString();
        valeurText.text = valeurString;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isHeld = !isHeld;
        heldText.enabled = isHeld;
    }

    public void ClearHeld()
    {
        isHeld = false;
        heldText.enabled = false;
    }
}