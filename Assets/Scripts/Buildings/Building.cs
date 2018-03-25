using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Building : MonoBehaviour {
    [SerializeField] private GameUnit gUnit = null;
    [SerializeField] private bool canBePlaced = false;
    [SerializeField] private bool isPlaced = false;
    [SerializeField] private bool isBuilt = false;
    [SerializeField] private UnityEngine.AI.NavMeshObstacle obstacle = null;
    private Collider groundCol = null;
    private List<Collider> colliders = new List<Collider>();

    public bool CanBePlaced {get{return canBePlaced;} set{canBePlaced = value;}}
    public bool IsPlaced {get{return isPlaced;} set{isPlaced = value;}}
    public bool IsBuilt {get{return isBuilt;} set{isBuilt = value;}}
    public GameUnit GUnit {get{return gUnit;} set{gUnit = value;}}

    // Use this for initialization
    public virtual void Start () {
        groundCol = GameObject.Find("Terrain").GetComponent<Collider>();
        if (!isPlaced) {
            ValidatePlacement();
        }
	}
	
	// Update is called once per frame
	public virtual void Update () {

	}

    public void ValidatePlacement() {
        CanBePlaced = true;
        gUnit.SetColors(Color.green);
    }

    public void InvalidatePlacement() {
        CanBePlaced = false;
        gUnit.SetColors(Color.red);
    }

    public virtual void PlaceBuilding() {
        if (CanBePlaced) {
            StartCoroutine(AdjustForTerrain());
            obstacle.enabled = true;
            IsPlaced = true;
            gUnit.RevertColors();
        }
    }
    
    public IEnumerator AdjustForTerrain() {
        while (colliders.Contains(groundCol)) {
            transform.Translate(transform.up * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(0);
        }
    }

    public void Demolish() {
        gUnit.PlayerEnt.FuelCount += (int)(gUnit.FuelCost * 0.15f);
        gUnit.PlayerEnt.FoodCount += (int)(gUnit.FoodCost * 0.15f);
        gUnit.PlayerEnt.MetalCount += (int)(gUnit.MetalCost * 0.15f);
        gUnit.UnselectUnit();
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other) {
        colliders.Add(other);
        if (!isPlaced && canBePlaced && !groundCol) {
            InvalidatePlacement();
        }
    }

    public virtual void OnTriggerStay(Collider other) {
        if (!colliders.Contains(other)) {
            colliders.Add(other);
        }
        if (!isPlaced && canBePlaced && !groundCol) {
            InvalidatePlacement();
        }
    }

    public virtual void OnTriggerExit(Collider other) {
        colliders.Remove(other);
        if (!isPlaced && !canBePlaced && !groundCol) {
            ValidatePlacement();
        }
    }
}
