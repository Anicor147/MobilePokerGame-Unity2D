using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SavedData", menuName = "SO/SaveData", order = 2)]
public class SavedDataSO : ScriptableObject
{
    private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
   [SerializeField] private int _gold;

 public int Gold
    {
        get => _gold;
        set => _gold = value;
    }
}