using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCalculator : MonoBehaviour
{
    private Carte[] _main = new Carte[5];
    private List<CarteData> _deck = new List<CarteData>(52);

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


    public bool OnePairIsSelected()
    {
        return true;
    }


    private bool IsOnePair(ref List<CarteData> handData)
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