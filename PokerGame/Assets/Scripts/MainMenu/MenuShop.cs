using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class MenuShop : MonoBehaviour
{
    private List<CarteData> _deck = new List<CarteData>(52);
    [SerializeField] private List<Carte> _main;
    [SerializeField] private List<GameObject> _cardsObjects;
    [SerializeField] private Sprite _boughtImage;
    [SerializeField] private List<Button> _cardsButton;
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                _deck.Add(new CarteData(j, (Enseigne)i));
            }
        }

        for (int i = 0; i < _deck.Count; i++)
        {
            _main[i].SetData(_deck[i]);

            if (i >= _deck.Count)
            {
                i = 0;
            }
        }
        
        UpdateIndex();
    }

    public void ButtonIndex(int index)
    {
        _main[index].ShowData();
        Debug.Log($"The index is : {index}");
        _cardsObjects[index].gameObject.GetComponent<Image>().sprite = _boughtImage;
        _cardsObjects[index].gameObject.GetComponentInChildren<Button>().image.color = Color.grey;
        _cardsObjects[index].gameObject.GetComponentInChildren<Button>().interactable = false;
        
        _cardsButton.RemoveAt(index);
        _main.RemoveAt(index);
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

   
}