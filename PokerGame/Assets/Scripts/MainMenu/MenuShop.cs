using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuShop : MonoBehaviour
{
    private List<CarteData> _deck = new List<CarteData>(52);
    private List<CarteData> _Sdeck = new List<CarteData>(2);
    [SerializeField] private List<Carte> _cards;
    [SerializeField] private List<Carte> _specialCards;
    [SerializeField] private List<GameObject> _cardsObjects;

    [SerializeField] private Sprite _boughtImage;
    [SerializeField] private List<Button> _cardsButton;

    [SerializeField] private SavedDeckSO _savedSODeck;

    private void Start()
    {
        MainMenuManager.closeShop += SaveCurrentDeck;


        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                _deck.Add(new CarteData(j, (Enseigne)i));
            }
        }

        for (int i = 0; i < _deck.Count; i++)
        {
            _cards[i].SetData(_deck[i]);

            if (i >= _deck.Count)
            {
                i = 0;
            }
        }

        for (int i = 0; i < _specialCards.Count; i++)
        {
            _Sdeck.Add(new CarteData(i , Enseigne.Joker));
        }

        for (int i = 0; i < _Sdeck.Count; i++)
        {
            _specialCards[i].SetData(_Sdeck[i]); 
            if (i >= _Sdeck.Count)
            {
                i = 0;
            }
        }
        
        
        UpdateIndex();
    }

    public void ButtonIndex(int index)
    {
        if (_deck.Count <= 5) return;
        _cardsObjects[index].gameObject.GetComponent<Image>().sprite = _boughtImage;
        _cardsObjects[index].gameObject.GetComponentInChildren<Button>().image.color = Color.grey;
        _cardsObjects[index].gameObject.GetComponentInChildren<Button>().interactable = false;

        _cardsObjects[index].gameObject.GetComponentInChildren<Button>().onClick.RemoveAllListeners();

        _cardsObjects.RemoveAt(index);
        _cardsButton.RemoveAt(index);
        _cards.RemoveAt(index);
        _deck.RemoveAt(index);
        UpdateIndex();
    }

    void UpdateIndex()
    {
        for (int i = 0; i < _cardsButton.Count; i++)
        {
            int value = i;
            _cardsButton[i].onClick.RemoveAllListeners();
            _cardsButton[i].onClick.AddListener(() => ButtonIndex(value));
        }
    }

    private void SaveCurrentDeck()
    {
        _savedSODeck.IsDeckModified = true;

        /*if (_savedSODeck.SavedDeck.Count > 0)
        {
            _savedSODeck.ClearDeck();
        }*/

        for (int i = 0; i < _deck.Count; i++)
        {
            _savedSODeck.SavedDeck.Add(_deck[i]);
        }
    }


    public void AddJoker(int index)
    {
        _deck.Add(new CarteData(index, Enseigne.Joker));
    }
}