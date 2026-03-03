using UnityEngine;
using System;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private float _startingCash = 50f;

    private float _currentCash;

    // Read-only access — private setter pattern
    public float CurrentCash
    {
        get => _currentCash;
        private set
        {
            _currentCash = Mathf.Max(0f, value);   // can't go below 0
            OnCashChanged?.Invoke(_currentCash);
        }
    }

    // Event so UI can react automatically
    public event Action<float> OnCashChanged;

    private void Awake()
    {
        CurrentCash = _startingCash;
    }

    /// <summary>Returns true and deducts amount if player can afford it.</summary>
    public bool TrySpend(float amount)
    {
        if (_currentCash < amount) return false;
        CurrentCash -= amount;
        return true;
    }

    public void AddCash(float amount) => CurrentCash += amount;
}
