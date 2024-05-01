using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatsCalculator : MonoBehaviour
{
    [SerializeField] private Carte[] _main = new Carte[5];
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _question2Text;
    [SerializeField] private TMP_InputField _removeCardsValue;
    private string _combo;
    private List<CarteData> _deck = new List<CarteData>(52);
    private const int iteration = 100000;
    private int _indexCarte = 0;
    private bool _onePairSelected = false;
    private bool _twoPairSelected = false;
    private bool _treeOfAKindSelected = false;
    private bool _removeCards = false;
    private int value = 0;
    private bool _straightIsSelected;
    private bool _flushIsSelected;
    private bool _fullHouseSelected;
    private bool _fourOfAKindSelected;
    private bool _straightFlushIsSelected;
    private bool _royalFlushSelected;
    private int _currentComboValue;
    private int _comboValue;
    private int higherValue = 0;
    private int conversionValue;
    private bool q5 = false;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                _deck.Add(new CarteData(j, (Enseigne)i));
            }
        }
    }

    public List<CarteData> Shuffle(List<CarteData> _deck)
    {
        var temp = new List<CarteData>();

        while (_deck.Count > 0)
        {
            int indexRandom = Random.Range(0, _deck.Count);
            temp.Add(_deck[indexRandom]);
            _deck.RemoveAt(indexRandom);
        }

        _deck = temp;
        return _deck;
    }

    private void CheckProb()
    {
        value = 0;
        higherValue = 0;
        var shuffledDeck = new List<CarteData>(_deck);
        int.TryParse(_removeCardsValue.text, out conversionValue);


        if (_removeCards)
        {
            shuffledDeck.Sort();
            for (int i = 0; i < shuffledDeck.Count; i++)
            {
                if (shuffledDeck[i].valeur < conversionValue)
                {
                    Debug.Log($"{shuffledDeck[i].valeur} <= {conversionValue}");
                    shuffledDeck.RemoveAt(i);
                    i--;
                }

                // Debug.Log(shuffledDeck[i].valeur);
            }

            Debug.Log($"after remove {shuffledDeck.Count}");
        }

        for (int i = 0; i < iteration; i++)
        {
            //  var shuffledDeck = new List<CarteData>(_deck);
            shuffledDeck = Shuffle(shuffledDeck);
            var _mainCopy = _main.ToArray();

            if (!q5)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (_indexCarte < shuffledDeck.Count)
                    {
                        _mainCopy[j].SetData(shuffledDeck[_indexCarte++]);
                    }
                    else
                    {
                        _indexCarte = 0;
                    }
                }
            }
            else
            {
                _mainCopy = DrawCards();
            }


            if (_onePairSelected)
            {
                if (IsOnePair(_mainCopy))
                {
                    _comboValue = 2;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = "Pair";
                }
            }

            if (_twoPairSelected)
            {
                if (IsTwoPair(_mainCopy))
                {
                    _comboValue = 3;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = " 2 Pair";
                }
            }

            if (_treeOfAKindSelected)
            {
                if (IsThreeOfAKind(_mainCopy))
                {
                    _comboValue = 4;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = "Three Of A Kind";
                }
            }

            if (_straightIsSelected)
            {
                if (IsStraight(_mainCopy))
                {
                    _comboValue = 5;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = "Straight";
                }
            }

            if (_flushIsSelected)
            {
                if (IsFlush(_mainCopy))
                {
                    _comboValue = 6;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = "Flush";
                }
            }

            if (_fullHouseSelected)
            {
                if (IsFullHouse(_mainCopy))
                {
                    _comboValue = 7;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = "Full House";
                }
            }

            if ((_fourOfAKindSelected))
            {
                if (IsFourOfAKind(_mainCopy))
                {
                    _comboValue = 8;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = "Four Of A Kind";
                }
            }

            if (_straightFlushIsSelected)
            {
                if (IsStraightFlush(_mainCopy))
                {
                    _comboValue = 9;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = "Straight Flush";
                }
            }

            if (_royalFlushSelected)
            {
                if (IsRoyalFlush(_mainCopy))
                {
                    _comboValue = 10;
                    if (_comboValue > _currentComboValue)
                    {
                        higherValue++;
                    }

                    value++;
                    _combo = "Royal Flush";
                }
            }
        }

        ResetBools();
        var q2 = (double)higherValue / iteration * 100;
        var prob = (double)value / iteration * 100;
        _text.text = $"La probabilité d'avoir une {_combo} est de : {prob} %";
        _question2Text.text = $"La probabilité d'avoir une main suppérieure est de : {q2}";
    }

    private void ResetBools()
    {
        _onePairSelected = false;
        _twoPairSelected = false;
        _treeOfAKindSelected = false;
        _straightIsSelected = false;
        _flushIsSelected = false;
        _fullHouseSelected = false;
        _fourOfAKindSelected = false;
        _straightFlushIsSelected = false;
        _royalFlushSelected = false;
        _removeCards = false;
    }

    public void ActivateAllBools()
    {
        _onePairSelected = true;
        _twoPairSelected = true;
        _treeOfAKindSelected = true;
        _straightIsSelected = true;
        _flushIsSelected = true;
        _fullHouseSelected = true;
        _fourOfAKindSelected = true;
        _straightFlushIsSelected = true;
        _royalFlushSelected = true;
    }


    public void Q5Button()
    {
        DrawCards();
    }

    public Carte[] DrawCards()
    {
        q5 = true;
        var _shuffledDeck = new List<CarteData>(_deck);
        _shuffledDeck = Shuffle(_shuffledDeck);
        var _mainCopy = _main.ToArray();

        for (int i = 0; i < 5; i++)
        {
            if (_indexCarte < _shuffledDeck.Count)
            {
                if (!_mainCopy[i].IsHeld)
                {
                    _mainCopy[i].SetData(_shuffledDeck[_indexCarte]);
                    _shuffledDeck.RemoveAt(_indexCarte); 
                }
                else
                {
                    continue;
                }
            }
            else
            {
                _indexCarte = 0; 
                break;
            }
        }

        return _mainCopy;
    }

    public void RemoveCardsBool()
    {
        _removeCards = true;
    }


    public void OnePairIsSelected()
    {
        _onePairSelected = true;
        _currentComboValue = 2;
        CheckProb();
    }

    public void TwoPairIsSelected()
    {
        _twoPairSelected = true;
        _currentComboValue = 3;
        CheckProb();
    }

    public void ThreeOfAKindIsSelected()
    {
        _treeOfAKindSelected = true;
        _currentComboValue = 4;
        CheckProb();
    }

    public void StraightIsSelected()
    {
        _straightIsSelected = true;
        _currentComboValue = 5;
        CheckProb();
    }

    public void FlushIsSelected()
    {
        _flushIsSelected = true;
        _currentComboValue = 6;
        CheckProb();
    }

    public void FullHouseIsSelected()
    {
        _fullHouseSelected = true;
        _currentComboValue = 7;
        CheckProb();
    }

    public void FourOfAKindSelected()
    {
        _fourOfAKindSelected = true;
        _currentComboValue = 8;
        CheckProb();
    }

    public void StraightFlushSelected()
    {
        _straightFlushIsSelected = true;
        _currentComboValue = 9;
        CheckProb();
    }

    public void RoyalFlushSelected()
    {
        _royalFlushSelected = true;
        _currentComboValue = 10;
        CheckProb();
    }

    private bool IsRoyalFlush(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();

        return (handData[0].valeur == 0 && handData[1].valeur == 10 && handData[2].valeur == 11 &&
                handData[3].valeur == 12 && handData[4].valeur == 13
                &&
                handData[0].color == handData[1].color && handData[1].color == handData[2].color &&
                handData[2].color == handData[3].color && handData[3].color == handData[4].color);
    }

    private bool IsStraightFlush(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        return handData[0].valeur == handData[1].valeur - 1 &&
               handData[0].valeur == handData[2].valeur - 2 &&
               handData[0].valeur == handData[3].valeur - 3 &&
               handData[0].valeur == handData[4].valeur - 4 &&
               handData[0].enseigne == handData[1].enseigne &&
               handData[0].enseigne == handData[2].enseigne &&
               handData[0].enseigne == handData[3].enseigne &&
               handData[0].enseigne == handData[4].enseigne;
    }

    private bool IsColor(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        return (handData[0].color == handData[1].color && handData[1].color == handData[2].color &&
                handData[2].color == handData[3].color && handData[3].color == handData[4].color);
    }

    private bool IsFourOfAKind(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        return (handData[0].valeur == handData[1].valeur && handData[1].valeur == handData[2].valeur &&
                handData[2].valeur == handData[3].valeur) ||
               (handData[1].valeur == handData[2].valeur && handData[2].valeur == handData[3].valeur &&
                handData[3].valeur == handData[4].valeur);
    }


    private bool IsFullHouse(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        return (handData[0].valeur == handData[1].valeur &&
                handData[0].valeur == handData[2].valeur &&
                handData[3].valeur == handData[4].valeur) ||
               (handData[0].valeur == handData[1].valeur &&
                handData[2].valeur == handData[3].valeur &&
                handData[2].valeur == handData[4].valeur);
    }

    private bool IsFlush(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        return (handData[0].enseigne == handData[1].enseigne
                && handData[0].enseigne == handData[2].enseigne
                && handData[0].enseigne == handData[3].enseigne
                && handData[0].enseigne == handData[4].enseigne);
    }

    private bool IsStraight(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        for (int i = 1; i < handData.Count; i++)
        {
            if (handData[i].valeur != handData[i - 1].valeur + 1)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsThreeOfAKind(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();

        return (handData[0].valeur == handData[1].valeur && handData[1].valeur == handData[2].valeur) ||
               (handData[2].valeur == handData[3].valeur && handData[3].valeur == handData[4].valeur) ||
               (handData[1].valeur == handData[2].valeur && handData[2].valeur == handData[3].valeur);
    }


    private bool IsTwoPair(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        int pairCount = 0;
        int i = 0;
        while (i < handData.Count - 1)
        {
            if (handData[i].valeur == handData[i + 1].valeur)
            {
                pairCount++;
                i += 2;
            }
            else
            {
                i++;
            }
        }

        return pairCount == 2;
    }

    private bool IsOnePair(Carte[] gameDeck)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();

        int pairCount = 0;
        for (int i = 0; i < handData.Count - 1; i++)
        {
            if (handData[i].valeur == handData[i + 1].valeur)
            {
                pairCount++;
            }
        }

        return pairCount == 1;
    }
}