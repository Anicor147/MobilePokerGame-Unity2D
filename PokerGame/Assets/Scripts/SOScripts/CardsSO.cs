using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SOScripts
{
    [CreateAssetMenu(fileName = "CardSprite", menuName = "SO/CardSprite", order = 1)]
    public class CardsSO : ScriptableObject
    {
        [SerializeField] public List<Sprite> spritesClover;
        [SerializeField] public List<Sprite> spritesDiamond;
        [SerializeField] public List<Sprite> spritesHearth;
        [SerializeField] public List<Sprite> spritesSpade;
    }
}
