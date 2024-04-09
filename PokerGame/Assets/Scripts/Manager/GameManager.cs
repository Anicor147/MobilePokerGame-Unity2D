using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SOScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Carte[] _main = new Carte[5];
    [SerializeField] private Carte[] _enemyMain = new Carte[5];
    [SerializeField] private TMP_Text _playerComboText;
    [SerializeField] private TMP_Text _enemyComboText;
    [SerializeField] private TMP_Text _winnerBox;
    [SerializeField] private TMP_Text _pScoreText;
    [SerializeField] private TMP_Text _eScoreText;
    [FormerlySerializedAs("_roundValue")] [SerializeField] private TMP_Text _roundValueText;
    [SerializeField] private TMP_Text _roundText;

    private List<CarteData> _deck = new List<CarteData>(52);
    private int _playerHandValue = 0;
    private int _indexCarte = 0;
    private int _enemyHandValue = 0;
    private int _playerScore = 0;
    private int _enemyScore = 0;
    private int round = 1;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                _deck.Add(new CarteData(j, (Enseigne)i));
            }
        }

        HideText();   
        Shuffle();
        //  Piger();
    }

    private void HideText()
    {
        _winnerBox.text = null;
        _playerComboText.text = null;
        _enemyComboText.text = null;
        _roundValueText.text = null;
        _roundText.text = null;
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
        _enemyHandValue = 0;
        _playerHandValue = 0;
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

        CheckCombo(ref _main, _playerComboText, ref _playerHandValue);

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

        CheckCombo(ref _enemyMain, _enemyComboText, ref _enemyHandValue);
        CheckComboValue();
    }


    private void CheckCombo(ref Carte[] gameDeck, TMP_Text comboText, ref int value)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        //Royal Flush

        if (IsRoyalFlush(handData))
        {
            value = 10;
            comboText.text = "Royal Flush" + value;
            return;
        }

        if (IsStraightFlush(handData))
        {
            value = 9;
            comboText.text = "Straight Flush : " + value;
            return;
        }

        //Carre
        if (IsFourOfAKind(handData))
        {
            value = 7;
            comboText.text = "Four Of A Kind : " + value;
            return;
        }

        //Full
        if (IsFullHouse(handData))
        {
            value = 6;
            comboText.text = "Full House : " + value;
            return;
        }
        //Flush

        if (IsFlush(handData))
        {
            value = 5;
            comboText.text = "Flush : " + value;
            return;
        }


        if (IsStraight(handData))
        {
            value = 4;
            comboText.text = "Straight : " + value;
            return;
        }

        // Three of a kind
        if (IsThreeOfAKind(handData))
        {
            value = 3;
            comboText.text = "Three Of A Kind : " + value;
            return;
        }


        // Two pair
        if (IsTwoPair(handData))
        {
            value = 2;
            comboText.text = "Two Pair : " + value;
            return;
        }

        // One Pair
        if (IsOnePair(handData))
        {
            value = 1;
            comboText.text = "One Pair : " + value;
            return;
        }

        if (!IsOnePair(handData) && !IsTwoPair(handData) && !IsThreeOfAKind(handData) && !IsFourOfAKind(handData) &&
            !IsColor(handData) &&
            !IsStraight(handData) && !IsFlush(handData) && !IsFullHouse(handData) && !IsRoyalFlush(handData) &&
            !IsStraightFlush(handData))
        {
            value = 0;
            comboText.text = "High Card : " + value;
        }
    }

    private void CheckComboValue()
    {
        if (_playerHandValue > _enemyHandValue)
        {
            _winnerBox.text = "Player Win !";
            _playerScore++;
            _pScoreText.text = _playerScore.ToString();
        }
        else if (_playerHandValue == _enemyHandValue)
        {
            _winnerBox.text = "Draw !";
        }
        else
        {
            _winnerBox.text = "Enemy Win !";
            _enemyScore++;
            _eScoreText.text = _enemyScore.ToString();
        }
       
        _roundText.text = "Round : ";
        _roundValueText.text = $"{round.ToString()}/10";
        round++;
    }

    private void CheckWinner()
    {
        //TODO Canvas de fin de la partie qui indique le gagnant
        if (round >= 10)
        {
            if (_playerScore > _enemyScore)
            {
                // Player Won
            }
            else
            {
                // Player Loose
            }
        }
    }

    #region Combinaison
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
#endregion
}