using UnityEngine;
using System.Collections;

public class GameUnit : MonoBehaviour {
    [SerializeField] private int team = 0;
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private GameUnitTypes gUnitType = GameUnitTypes.NONE;
    [SerializeField] private Sprite unitPortrait = null;

    public int Team {get{return team;} set{team = value;}}
    public int Health {get{return currentHealth;} set{currentHealth = value;}}
    public int MaxHealth {get{return maxHealth;} set{maxHealth = value;}}
    public GameUnitTypes GUnitType {get{return gUnitType;} set{gUnitType = value;}}
    public Sprite UnitPortrait {get{return unitPortrait;} set{unitPortrait = value;}}

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
