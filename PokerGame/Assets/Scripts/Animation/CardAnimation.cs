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

    [SerializeField] private GameObject[] _eCardPos;
    [SerializeField] private GameObject[] _eCards;

    private int _index = 0;
    private int _eindex = 0;
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
        StartCoroutine(ECardLocation());
    }

    private IEnumerator CardLocation()
    {
        if (_index >= _cards.Length)
        {
            animationRunning?.Invoke(false);
            yield break;
        }


        var startPosition = _cards[_index].transform.position;
        var targetPosition = _cardPos[_index].transform.position;

        float temps = 0;
        float duration = 0.5f;

        while (temps < duration)
        {
            _cards[_index].transform.position = Vector2.Lerp(startPosition, targetPosition, temps / duration);
            temps += Time.deltaTime;
            yield return null;
        }


        yield return new WaitForSeconds(0.1f);
        _index++;
        StartCoroutine(CardLocation());
    }

    private IEnumerator ECardLocation()
    {
        if (_eindex >= _cards.Length)
        {
            yield break;
        }

        var startPosition = _eCards[_eindex].transform.position;
        var targetPosition = _eCardPos[_eindex].transform.position;

        float temps = 0;
        float duration = 0.5f;

        while (temps < duration)
        {
            _eCards[_eindex].transform.position = Vector2.Lerp(startPosition, targetPosition, temps / duration);
            temps += Time.deltaTime;
            yield return null;
        }


        yield return new WaitForSeconds(0.1f);
        _eindex++;
        StartCoroutine(ECardLocation());
    }
}