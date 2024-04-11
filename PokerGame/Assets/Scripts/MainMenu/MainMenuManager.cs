using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas _optionCanvas;
    private bool closedOptions = true;

    

    private void Awake()
    {
        _optionCanvas.enabled = false;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenOptionMenu()
    {
        _optionCanvas.enabled = true;
        if (!closedOptions)
        {
            _optionCanvas.enabled = false;
            closedOptions = true;
            return;
        }

        closedOptions = false;
    }
}