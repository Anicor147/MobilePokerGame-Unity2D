using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [FormerlySerializedAs("ShopCanvas")] [SerializeField] private Canvas _shopCanvas;
    [SerializeField] private Canvas _optionCanvas;

    private void Start()
    {
        _shopCanvas.enabled = false;
        _optionCanvas.enabled = false;
    }

    public void OpenShop()
    {
        _shopCanvas.enabled = true;
    }

    public void CloseShop()
    {
        _shopCanvas.enabled = false;
    }

    public void OpenSetting()
    {
        _optionCanvas.enabled = true;
    }

    public void CloseSetting()
    {
        _optionCanvas.enabled = false;
    }


}
