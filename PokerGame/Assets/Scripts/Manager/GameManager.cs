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
            for (int j = 0; j < 13; j++)
            {
                deck.Add(new CarteData(j, (Enseigne)i));
            }
        }

        Shuffle();
        Piger();
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


        if (handData[0].enseigne == handData[1].enseigne
            && handData[0].enseigne == handData[2].enseigne
            && handData[0].enseigne == handData[3].enseigne
            && handData[0].enseigne == handData[4].enseigne)
        {
            comboText.text = "Flush";
            return;
        }

        comboText.text = "High CARD";
    }
}