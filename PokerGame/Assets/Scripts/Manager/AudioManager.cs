using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSO _audioSo;
    
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        SetVolume();
    }


    private void SetVolume()
    {
        _audioSource.volume = _audioSo.SoundSlider;
    }
    
    
    
    
    
    
    
    
    
}