using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private Image _backCard;

    [SerializeField] private GameObject[] _cardPos;
    [SerializeField] private GameObject[] _cards;
    private int index = 0;
    private Image _frontCard;
    public static event Action<bool> animationRunning; 
    

    public void Awake()
    {
        Carte.OnCardFrontChanged += image => SetFrontCard(image);
    }

    private void SetFrontCard(Image image)
    {
        _frontCard = image;
    }

    private void Start()
    {
        animationRunning?.Invoke(true);
        StartCoroutine(CardLocation());
    }

    private IEnumerator CardLocation()
    {
        if (index >= _cards.Length)
        {
            animationRunning?.Invoke(false); 
            yield break;
        }
        
       

        var startPosition = _cards[index].transform.position;
        var targetPosition = _cardPos[index].transform.position;

        float temps = 0;
        float duration = 0.5f; 

        while (temps < duration)
        {
            _cards[index].transform.position = Vector2.Lerp(startPosition, targetPosition, temps / duration);
            temps += Time.deltaTime;
            yield return null;
        }

        
        yield return new WaitForSeconds(0.1f);
        index++;
        StartCoroutine(CardLocation());
    }
}