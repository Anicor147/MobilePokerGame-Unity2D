using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SavedDeck", menuName = "SO/SaveDeck", order = 3)]
public class SavedDeckSO : ScriptableObject
{
    private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;

    private bool _isDeckModified;
    private List<CarteData> _savedDeck = new List<CarteData>();

    public bool IsDeckModified
    {
        get => _isDeckModified;
        set => _isDeckModified = value;
    }

    public List<CarteData> SavedDeck
    {
        get => _savedDeck;
        set => _savedDeck = value;
    }

    public void ClearDeck()
    {
        _savedDeck.Clear();
    }


}