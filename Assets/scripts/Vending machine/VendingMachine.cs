using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float _interactRange = 2.5f;
    [SerializeField] private KeyCode _interactKey = KeyCode.E;

    [Header("Drinks Available")]
    [SerializeField] private DrinkData[] _drinks;   // assign 2-3 DrinkData assets here

    [Header("UI")]
    [SerializeField] private VendingMachineUI _vendingUI;

    // ── private setters (read-only access pattern) ──────────────────────────
    private bool _playerInRange;
    public bool PlayerInRange
    {
        get => _playerInRange;
        private set
        {
            _playerInRange = value;
            // Show / hide "Press E" prompt
            if (_promptUI != null) _promptUI.SetActive(value);
        }
    }

    private bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
        private set
        {
            _isOpen = value;
            if (_vendingUI != null) _vendingUI.SetVisible(value);
        }
    }

    public DrinkData[] Drinks => _drinks;   // read-only array reference
    // ────────────────────────────────────────────────────────────────────────

    private Transform _player;
    private GameObject _promptUI;   // optional "Press E" world-space text

    private void Awake()
    {
        // Try to find the prompt child object (optional)
        Transform t = transform.Find("PressEPrompt");
        if (t != null) _promptUI = t.gameObject;
    }

    private void Start()
    {
        // Auto-find player by tag
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) _player = p.transform;

        IsOpen = false;
    }

    private void Update()
    {
        if (_player == null) return;

        float dist = Vector3.Distance(transform.position, _player.position);
        PlayerInRange = (dist <= _interactRange);

        if (PlayerInRange && Input.GetKeyDown(_interactKey))
        {
            ToggleUI();
        }

        // Close with Escape
        if (IsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    public void ToggleUI() => IsOpen = !IsOpen;
    public void CloseUI()  => IsOpen = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactRange);
    }
}
