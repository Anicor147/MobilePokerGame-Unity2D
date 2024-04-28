using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialCards", menuName = "SO/SpecialCards", order = 4)]
public class SpecialCardsSO : ScriptableObject 
{
    [SerializeField] private List<Sprite> _jokerCards;


    public List<Sprite> JokerCards
    {
        get => _jokerCards;
        set => _jokerCards = value;
    }
}
