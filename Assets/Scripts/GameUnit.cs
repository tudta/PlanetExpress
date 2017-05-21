using UnityEngine;
using System.Collections;

public class GameUnit : MonoBehaviour {
    [SerializeField] private int team = 0;
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private GameUnitTypes gUnitType = GameUnitTypes.NONE;
    [SerializeField] private Sprite unitPortrait = null;
    [SerializeField] private string unitName = string.Empty;
    [SerializeField] private int metalCost = 0;
    [SerializeField] private int fuelCost = 0;
    [SerializeField] private int foodCost = 0;
    [SerializeField] private int popCost = 0;

    public int Team {get{return team;} set{team = value;}}
    public int Health {get{return currentHealth;} set{currentHealth = value;}}
    public int MaxHealth {get{return maxHealth;} set{maxHealth = value;}}
    public GameUnitTypes GUnitType {get{return gUnitType;} set{gUnitType = value;}}
    public Sprite UnitPortrait {get{return unitPortrait;} set{unitPortrait = value;}}
    public string UnitName {get{return unitName;} set{unitName = value;}}
    public int MetalCost {get{return metalCost;} set{metalCost = value;}}
    public int FuelCost {get{return fuelCost;} set{fuelCost = value;}}
    public int FoodCost {get{return foodCost;} set{foodCost = value;}}
    public int PopCost {get{return popCost;} set{popCost = value;}}

    public virtual void Awake() {

    }

    // Use this for initialization
    public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}

    public virtual void ApplyDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
