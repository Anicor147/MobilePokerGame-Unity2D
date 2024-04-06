using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("main")] [SerializeField] private Carte[] _main = new Carte[5];
    [FormerlySerializedAs("enemyMain")] [SerializeField] private Carte[] _enemyMain = new Carte[5];
    private List<CarteData> _deck = new List<CarteData>(52);
    private int _indexCarte = 0;
    private int _playerHandValue = 0;
    private int _enemyHandValue = 0;
    [FormerlySerializedAs("playerComboText")] [FormerlySerializedAs("comboText")] [SerializeField] private TMP_Text _playerComboText;
    [FormerlySerializedAs("enemyComboText")] [SerializeField] private TMP_Text _enemyComboText;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                _deck.Add(new CarteData(j, (Enseigne)i));
            }
        }

        Shuffle();
        //  Piger();
    }

    public void Shuffle()
    {
        var temp = new List<CarteData>();

        while (_deck.Count > 0)
        {
            int indexRandom = Random.Range(0, _deck.Count);
            temp.Add(_deck[indexRandom]);
            _deck.RemoveAt(indexRandom);
        }

        _deck = temp;
    }

    public void Piger()
    {
        for (int i = 0; i < 5; i++)
        {
            if (_main[i].IsHeld)
            {
                continue;
            }

            _main[i].SetData(_deck[_indexCarte++]);

            if (_indexCarte >= _deck.Count)
            {
                _indexCarte = 0;
                Shuffle();
            }
        }

        foreach (var mainCarte in _main)
        {
            if (mainCarte.IsHeld)
            {
                mainCarte.ClearHeld();
            }
        }

        CheckCombo(ref _main , _playerComboText);

        //Shuffle();
        for (int i = 0; i < 5; i++)
        {
            _enemyMain[i].SetData(_deck[_indexCarte++]);
            if (_indexCarte >= _deck.Count)
            {
                _indexCarte = 0;
                Shuffle();
            }
        }

        CheckCombo(ref _enemyMain, _enemyComboText);
    }


    private void CheckCombo(ref Carte[] gameDeck , TMP_Text comboText)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
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

        comboText.text = "High Card";
        
        
        
        
        
        
        
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