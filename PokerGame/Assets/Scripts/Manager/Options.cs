using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioSO _audioSo;
    [SerializeField] private Slider _audioSlider;

    private void Start()
    {
        _audioSlider.value = 0.1f;
    }

    private void Update()
    {
        if ( _audioSo.SoundSlider != _audioSlider.value)
        {
            _audioSo.SoundSlider = _audioSlider.value; 
        }
    }
}
