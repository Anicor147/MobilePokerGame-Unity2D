using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Carte[] main = new Carte[5];
    private List<CarteData> deck = new List<CarteData>(52);
    private int indexCarte = 0;
    [SerializeField] private TMP_Text comboText;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                deck.Add(new CarteData(j, (Enseigne)i));
            }
        }

        Shuffle();
     //   Piger();
    }

    public void Shuffle()
    {
        var temp = new List<CarteData>();

        while (deck.Count > 0)
        {
            int indexRandom = Random.Range(0, deck.Count);
            temp.Add(deck[indexRandom]);
            deck.RemoveAt(indexRandom);
        }

        deck = temp;
    }

    public void Piger()
    {
        for (int i = 0; i < 5; i++)
        {
            if (main[i].IsHeld)
            {
                continue;
            }

            main[i].SetData(deck[indexCarte++]);

            if (indexCarte >= deck.Count)
            {
                indexCarte = 0;
                Shuffle();
            }
        }

        foreach (var mainCarte in main)
        {
            if (mainCarte.IsHeld)
            {
                mainCarte.ClearHeld();
            }
        }

        CheckCombo();
    }


    private void CheckCombo()
    {
        List<CarteData> handData = main.Select(x => x.Data).ToList();
        handData.Sort();

        //Royal Flush

        if (IsRoyalFlush(handData))
        {
            comboText.text = "Royal Flush";
            return;
        }

        //Flush
        if (IsFlush(handData))
        {
            comboText.text = "Flush";
            return;
        }

        if (IsStraightFlush(handData))
        {
            comboText.text = "Straight Flush";
            return;
        }

        if (IsStraight(handData))
        {
            comboText.text = "Straight";
            return;
        }

        //Carre
        if (IsFourOfAKind(handData))
        {
            comboText.text = "Four Of A Kind";
            return;
        }

        //Full
        if (IsFullHouse(handData))
        {
            comboText.text = "Full";
            return;
        }

        //Color
        if (IsColor(handData))
        {
            comboText.text = "Color";
            return;
        }
         
        // Three of a kind
        if (IsThreeOfAKind(handData))
        {
            comboText.text = "Three Of A Kind";
            return;
        }

        // Two pair
        if (IsTwoPair(handData))
        {
            comboText.text = "Two Pair";
            return;
        }

        // One Pair
        if (IsOnePair(handData))
        {
            comboText.text = "One Pair";
            return;
        }

        comboText.text = "High CARD";
    }

    private bool IsFullHouse(List<CarteData> handData)
    {
        return (handData[0].valeur == handData[1].valeur && handData[2].valeur == handData[3].valeur &&
                handData[3].valeur == handData[4].valeur) ||
               (handData[0].valeur == handData[1].valeur && handData[1].valeur == handData[2].valeur &&
                handData[3].valeur == handData[4].valeur);
    }

    private bool IsFourOfAKind(List<CarteData> handData)
    {
        return (handData[0].valeur == handData[1].valeur && handData[1].valeur == handData[2].valeur &&
                handData[2].valeur == handData[3].valeur) ||
               (handData[1].valeur == handData[2].valeur && handData[2].valeur == handData[3].valeur &&
                handData[3].valeur == handData[4].valeur);
    }

    private bool IsRoyalFlush(List<CarteData> handData)
    {
        return IsStraightFlush(handData) && handData[0].valeur == 10;
    }

    private bool IsFlush(List<CarteData> handData)
    {
        return (handData[0].enseigne == handData[1].enseigne
                && handData[0].enseigne == handData[2].enseigne
                && handData[0].enseigne == handData[3].enseigne
                && handData[0].enseigne == handData[4].enseigne);
    }

    private bool IsColor(List<CarteData> handData)
    {
        return (handData[0].color == handData[1].color && handData[1].color == handData[2].color &&
                handData[2].color == handData[3].color && handData[3].color == handData[4].color);
    }

    private bool IsStraightFlush(List<CarteData> handData)
    {
        return IsStraight(handData) && IsColor(handData);
    }

    private bool IsStraight(List<CarteData> handData)
    {
        for (int i = 1; i < handData.Count; i++)
        {
            if (handData[i].valeur != handData[i - 1].valeur + 1)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsThreeOfAKind(List<CarteData> handData)
    {
        return (handData[0].valeur == handData[1].valeur && handData[1].valeur == handData[2].valeur) ||
               (handData[1].valeur == handData[2].valeur && handData[2].valeur == handData[3].valeur);
    }

    private bool IsTwoPair(List<CarteData> handData)
    {
        int pairCount = 0;
        for (int i = 0; i < handData.Count - 1; i++)
        {
            if (handData[i].valeur == handData[i + 1].valeur)
            {
                pairCount++;
                i++;
            }
        }

        return pairCount == 2;
    }

    private bool IsOnePair(List<CarteData> handData)
    {
        for (int i = 0; i < handData.Count - 1; i++)
        {
            if (handData[i].valeur == handData[i + 1].valeur)
            {
                return true;
            }
        }

        return false;
    }
}