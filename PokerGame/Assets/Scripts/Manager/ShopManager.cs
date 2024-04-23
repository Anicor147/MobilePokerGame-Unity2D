using System;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class ShopManager : MonoBehaviour
    {
        private static ShopManager instance;
        public static Action increaseHeld;
        private int _remainingHeldShopCharges = 3;
        [SerializeField] private SavedDataSO _savedDataSo;
        [SerializeField] private TMP_Text _goldText;
        private int _goldValue = 0;

        public ShopManager Instance
        {
            get => instance;
        }

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
        }

        private void Start()
        {
            if (_savedDataSo.Gold != 0)
            {
                _goldValue = _savedDataSo.Gold;
                _goldText.text = _goldValue.ToString();
            }

            GameManager.IncreaseGold += IncreaseGoldValue;
        }

        void IncreaseGoldValue()
        {
            _goldValue += 10 ;
            _goldText.text = _goldValue.ToString();
            _savedDataSo.Gold = _goldValue;
        }

        public void IncreaseHeldCharges()
        {
            if (_remainingHeldShopCharges > 0)
            {
                Debug.Log("Button is press ,should increase");
                increaseHeld?.Invoke();
                _remainingHeldShopCharges--;

                _goldValue -= 10;
                if (_goldValue < 0) _goldValue = 0;
                _goldText.text = _goldValue.ToString();
                _savedDataSo.Gold = _goldValue;
            }
            else
            {
                // grey button
            }
        }

        private void OnDisable()
        {
            GameManager.IncreaseGold += IncreaseGoldValue;
        }
    }
}