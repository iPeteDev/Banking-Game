using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Spawns the drink in the player's hand and shows a HUD icon.
/// Attach to the Player GameObject.
/// </summary>
public class PlayerDrinkHolder : MonoBehaviour
{
    [Header("Hold Point")]
    [SerializeField] private Transform _holdPoint;   // empty child in front of camera/hand

    [Header("HUD")]
    [SerializeField] private GameObject       _drinkHUD;       // small panel bottom-right
    [SerializeField] private Image            _hudIcon;
    [SerializeField] private TextMeshProUGUI  _hudDrinkName;

    // ── Private setter pattern ───────────────────────────────────────────────
    private DrinkData _currentDrink;
    public DrinkData CurrentDrink
    {
        get => _currentDrink;
        private set
        {
            _currentDrink = value;
            RefreshHUD();
        }
    }

    private bool _isHoldingDrink;
    public bool IsHoldingDrink
    {
        get => _isHoldingDrink;
        private set
        {
            _isHoldingDrink = value;
            if (_drinkHUD != null) _drinkHUD.SetActive(value);
        }
    }
    // ────────────────────────────────────────────────────────────────────────

    private GameObject _spawnedDrinkObject;

    private void Start()
    {
        IsHoldingDrink = false;
    }

    public void PickUpDrink(DrinkData drink)
    {
        // Remove previous drink if any
        DropDrink();

        CurrentDrink    = drink;
        IsHoldingDrink  = true;

        // Spawn 3D drink model at hold point
        if (drink.DrinkPrefab != null && _holdPoint != null)
        {
            _spawnedDrinkObject = Instantiate(drink.DrinkPrefab, _holdPoint.position,
                                              _holdPoint.rotation, _holdPoint);
        }
    }

    public void DropDrink()
    {
        if (_spawnedDrinkObject != null)
        {
            Destroy(_spawnedDrinkObject);
            _spawnedDrinkObject = null;
        }

        CurrentDrink   = null;
        IsHoldingDrink = false;
    }

    private void RefreshHUD()
    {
        if (_currentDrink == null) return;
        if (_hudIcon     != null) _hudIcon.sprite   = _currentDrink.DrinkIcon;
        if (_hudDrinkName != null) _hudDrinkName.text = _currentDrink.DrinkName;
    }
}
