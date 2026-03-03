using UnityEngine;

[CreateAssetMenu(fileName = "NewDrink", menuName = "Vending Machine/Drink Data")]
public class DrinkData : ScriptableObject
{
    [Header("Drink Info")]
    [SerializeField] private string _drinkName;
    [SerializeField] private string _description;
    [SerializeField] private float _price;
    [SerializeField] private Sprite _drinkIcon;
    [SerializeField] private GameObject _drinkPrefab; // the held drink object

    // Read-only access via private setters
    public string DrinkName    => _drinkName;
    public string Description  => _description;
    public float  Price        => _price;
    public Sprite DrinkIcon    => _drinkIcon;
    public GameObject DrinkPrefab => _drinkPrefab;
}
