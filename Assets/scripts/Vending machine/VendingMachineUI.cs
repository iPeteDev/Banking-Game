using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VendingMachineUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VendingMachine _machine;
    [SerializeField] private PlayerWallet   _wallet;
    [SerializeField] private PlayerDrinkHolder _drinkHolder;

    [Header("UI Elements")]
    [SerializeField] private GameObject   _panel;
    [SerializeField] private TextMeshProUGUI _cashText;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button[]     _drinkButtons;          // 2-3 buttons in Inspector
    [SerializeField] private Image[]      _drinkIcons;
    [SerializeField] private TextMeshProUGUI[] _drinkNameTexts;
    [SerializeField] private TextMeshProUGUI[] _drinkPriceTexts;

    private void Start()
    {
        _wallet.OnCashChanged += RefreshCashDisplay;
        SetVisible(false);
        BindButtons();
    }

    private void OnDestroy()
    {
        if (_wallet != null) _wallet.OnCashChanged -= RefreshCashDisplay;
    }

    // ── Read-only pattern: UI state controlled through SetVisible ───────────
    private bool _isVisible;
    public bool IsVisible
    {
        get => _isVisible;
        private set
        {
            _isVisible = value;
            _panel.SetActive(value);

            // Lock / unlock cursor
            Cursor.lockState = value ? CursorLockMode.None  : CursorLockMode.Locked;
            Cursor.visible   = value;

            if (value) RefreshUI();
        }
    }
    // ────────────────────────────────────────────────────────────────────────

    public void SetVisible(bool show) => IsVisible = show;

    private void BindButtons()
    {
        DrinkData[] drinks = _machine.Drinks;
        for (int i = 0; i < _drinkButtons.Length; i++)
        {
            if (i >= drinks.Length)
            {
                _drinkButtons[i].gameObject.SetActive(false);
                continue;
            }

            int index = i;  // capture for lambda
            _drinkButtons[i].onClick.RemoveAllListeners();
            _drinkButtons[i].onClick.AddListener(() => OnDrinkSelected(index));
        }
    }

    private void RefreshUI()
    {
        ClearMessage();
        RefreshCashDisplay(_wallet.CurrentCash);

        DrinkData[] drinks = _machine.Drinks;
        for (int i = 0; i < _drinkButtons.Length; i++)
        {
            if (i >= drinks.Length) continue;

            DrinkData d = drinks[i];
            if (_drinkIcons[i]      != null) _drinkIcons[i].sprite   = d.DrinkIcon;
            if (_drinkNameTexts[i]  != null) _drinkNameTexts[i].text = d.DrinkName;
            if (_drinkPriceTexts[i] != null) _drinkPriceTexts[i].text = $"₱{d.Price:F2}";

            // Grey out if player can't afford
            _drinkButtons[i].interactable = (_wallet.CurrentCash >= d.Price);
        }
    }

    private void RefreshCashDisplay(float cash)
    {
        if (_cashText != null) _cashText.text = $"Cash: ₱{cash:F2}";

        // Refresh affordability on all buttons
        DrinkData[] drinks = _machine.Drinks;
        for (int i = 0; i < _drinkButtons.Length && i < drinks.Length; i++)
            _drinkButtons[i].interactable = (cash >= drinks[i].Price);
    }

    private void OnDrinkSelected(int index)
    {
        DrinkData drink = _machine.Drinks[index];

        if (!_wallet.TrySpend(drink.Price))
        {
            ShowMessage("Not enough cash!", Color.red);
            return;
        }

        _drinkHolder.PickUpDrink(drink);
        ShowMessage($"Enjoy your {drink.DrinkName}!", Color.green);

        // Close after short delay
        Invoke(nameof(ClosePanel), 1.2f);
    }

    private void ClosePanel() => _machine.CloseUI();

    private void ShowMessage(string msg, Color color)
    {
        if (_messageText == null) return;
        _messageText.text  = msg;
        _messageText.color = color;
        _messageText.gameObject.SetActive(true);
    }

    private void ClearMessage()
    {
        if (_messageText != null) _messageText.gameObject.SetActive(false);
    }
}
