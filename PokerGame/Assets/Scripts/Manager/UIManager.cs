using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas ShopCanvas;


    private void Start()
    {
        ShopCanvas.enabled = false;
    }

    public void OpenShop()
    {
        ShopCanvas.enabled = true;
    }

    public void CloseShop()
    {
        ShopCanvas.enabled = false;
    }


}
