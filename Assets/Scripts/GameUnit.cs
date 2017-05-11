using UnityEngine;
using System.Collections;

public class GameUnit : MonoBehaviour {
    [SerializeField] private int team = 0;
    [SerializeField] private int health = 0;
    [SerializeField] private GameUnitTypes gUnitType = GameUnitTypes.NONE;

    public int Team {get{return team;} set{team = value;}}
    public int Health {get{return health;} set{health = value;}}
    public GameUnitTypes GUnitType {get{return gUnitType;} set{gUnitType = value;}}

    public virtual void Awake() {

    }

    // Use this for initialization
    public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}

    public virtual void ApplyDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
