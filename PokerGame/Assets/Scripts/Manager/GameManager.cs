using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using SOScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Carte[] _main = new Carte[5];
    [SerializeField] private Carte[] _enemyMain = new Carte[5];
    [SerializeField] private TMP_Text _playerComboText;
    [SerializeField] private TMP_Text _enemyComboText;
    [SerializeField] private TMP_Text _winnerBox;
    [SerializeField] private TMP_Text _pScoreText;
    [SerializeField] private TMP_Text _eScoreText;
    [SerializeField] private SavedDeckSO _savedDeckSo;

    [FormerlySerializedAs("_roundValue")] [SerializeField]
    private TMP_Text _roundValueText;

    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private Canvas _winScreen;
    [SerializeField] private TMP_Text _winnerText;
    public static GameManager Instance;
    public static Action IncreaseGold;
    private List<CarteData> _deck = new List<CarteData>(52);

    private List<CarteData> _eDeck = new List<CarteData>(52);
    private int _playerHandValue = 0;
    private int _indexCarte = 0;
    private int _indexECarte = 0;
    private int _enemyHandValue = 0;
    private int _playerScore = 0;
    private int _enemyScore = 0;
    private int round = 1;
    private bool _animationRunning;
    private bool _doubleScore;
    private bool _doubleGold;
    private int _maxHeld = 1;
    private int _currentHeld;

    public int MaxHeld
    {
        get => _maxHeld;
        set => _maxHeld = value;
    }

    public int CurrentHeld
    {
        get => _currentHeld;
        set => _currentHeld = value;
    }


    private void Awake()
    {
        _winScreen.enabled = false;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        Debug.Log(_savedDeckSo.IsDeckModified);
        if (_savedDeckSo.IsDeckModified)
        {
            for (int i = 0; i < _savedDeckSo.SavedDeck.Count; i++)
            {
                _deck.Add(_savedDeckSo.SavedDeck[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    _eDeck.Add(new CarteData(j, (Enseigne)i));
                    Debug.Log(_eDeck[i]);
                }
            }

         // _savedDeckSo.IsDeckModified = false;
        }
        else
        {
            Debug.Log(" not modified");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    _deck.Add(new CarteData(j, (Enseigne)i));
                    _eDeck.Add(new CarteData(j, (Enseigne)i));
                }
            }
        }


        ShopManager.increaseHeld += IncreaseMaxHeld;
        ShopManager.doubleScore += value => _doubleScore = value;
        CardAnimation.animationRunning += b => _animationRunning = b;
        ShopManager.resetEScore += ResetEnemyScore;
        _currentHeld = _maxHeld;

        HideText();
        Shuffle(ref _deck);
        Shuffle(ref _eDeck);
        //  Piger();
    }

    private void ResetEnemyScore()
    {
        _enemyScore = 0;
        _eScoreText.text = _enemyScore.ToString();
    }

    private void IncreaseMaxHeld()
    {
        if (_currentHeld <= 0 || _currentHeld < _maxHeld)
        {
            _currentHeld++;
            _maxHeld++;
        }
        else
        {
            _maxHeld++;
            _currentHeld = _maxHeld;
        }

        Debug.Log(_maxHeld);
    }

    private void HideText()
    {
        _winnerBox.text = null;
        _playerComboText.text = null;
        _enemyComboText.text = null;
        _roundValueText.text = null;
        _roundText.text = null;
    }

    public void Shuffle(ref List<CarteData> _deck)
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
        if (_animationRunning) return;

        _enemyHandValue = 0;
        _playerHandValue = 0;
        for (int i = 0; i < 5; i++)
        {
            if (_main[i].IsHeld)
            {
                continue;
            }

            if (_indexCarte < _deck.Count)
            {
                _main[i].SetData(_deck[_indexCarte++]);
            }
            else
            {
                _indexCarte = 0;
                Shuffle(ref _deck);
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
            _enemyMain[i].SetData(_eDeck[_indexECarte++]);
            if (_indexECarte >= _eDeck.Count)
            {
                _indexECarte = 0;
                Shuffle(ref _eDeck);
            }
        }
        

        CheckCombo(ref _enemyMain, _enemyComboText, ref _enemyHandValue);
        CheckComboValue(ref _playerHandValue, ref _enemyHandValue);
    }

    private void CheckCombo(ref Carte[] gameDeck, TMP_Text comboText, ref int value)
    {
        List<CarteData> handData = gameDeck.Select(x => x.Data).ToList();
        handData.Sort();
        
        CheckForJokers(handData);
        
        if (IsRoyalFlush(handData))
        {
            SetComboText("Royal Flush", 10, ref value, comboText);
            return;
        }

        if (IsStraightFlush(handData))
        {
            SetComboText("Straight Flush", 9, ref value, comboText);
            return;
        }

        if (IsFourOfAKind(handData))
        {
            SetComboText("Four Of A Kind", 7, ref value, comboText);
            return;
        }

        if (IsFullHouse(handData))
        {
            SetComboText("Full House", 6, ref value, comboText);
            return;
        }

        if (IsFlush(handData))
        {
            SetComboText("Flush", 5, ref value, comboText);
            return;
        }

        if (IsStraight(handData))
        {
            SetComboText("Straight", 4, ref value, comboText);
            return;
        }

        if (IsThreeOfAKind(handData))
        {
            SetComboText("Three Of A Kind", 3, ref value, comboText);
            return;
        }

        if (IsTwoPair(handData))
        {
            SetComboText("Two Pair", 2, ref value, comboText);
            return;
        }

        if (IsOnePair(handData))
        {
            SetComboText("One Pair", 1, ref value, comboText);
            return;
        }

        SetComboText("High Card", 0, ref value, comboText);
    }

    private void SetComboText(string comboName, int baseValue, ref int value, TMP_Text comboText)
    {
        value = baseValue;
        if (_doubleScore)
        {
            Debug.Log("is doubled");
            value *= 2;
            if (value != 0)
                comboText.text = $"{comboName} : {value} Its Doubled!!!";
            else
            {
                comboText.text = $"{comboName} : {value}";
            }
        }
        else
        {
            comboText.text = $"{comboName} : {value}";
        }

        _doubleScore = false;
    }

    private void CheckComboValue(ref int playerValue, ref int enemyValue)
    {
        if (_playerHandValue > _enemyHandValue)
        {
            _winnerBox.text = "Player Win !";
            _playerScore += playerValue;
            _pScoreText.text = _playerScore.ToString();
        }
        else if (_playerHandValue == _enemyHandValue)
        {
            _winnerBox.text = "Draw !";
        }
        else
        {
            _winnerBox.text = "Enemy Win !";
            _enemyScore += enemyValue;
            _eScoreText.text = _enemyScore.ToString();
        }

        _roundText.text = "Round : ";
        _roundValueText.text = $"{round.ToString()}/10";
        if (round >= 10)
        {
            CheckWinner();
            return;
        }

        round++;
    }

    private void CheckWinner()
    {
        _winScreen.enabled = true;
        if (_playerScore > _enemyScore)
        {
            IncreaseGold?.Invoke();
            _winnerText.text = "Player win!";
        }
        else
        {
            _winnerText.text = "Enemy win!";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }


    #region Combinaison

    private void CheckForJokers(List<CarteData> handData)
    {
        foreach (var card in handData)
        {
            if (card.enseigne == Enseigne.Joker)
            {
                if (card.color == Color.black)
                {
                    IncreaseGold?.Invoke();
                }
                else if (card.color == Color.red)
                {
                    _enemyScore -= 5;
                    _eScoreText.text = _enemyScore.ToString(); 
                }
                
            }
        }        
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

    #endregion

    private void OnDisable()
    {
        CardAnimation.animationRunning -= b => _animationRunning = b;
    }
}