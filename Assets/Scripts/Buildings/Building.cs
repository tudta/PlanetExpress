using UnityEngine;
using System.Collections;

public abstract class Building : MonoBehaviour {
    [SerializeField] private GameUnit gUnit = null;
    [SerializeField] private bool canBePlaced = false;
    [SerializeField] private bool isPlaced = false;
    [SerializeField] private bool isBuilt = false;
    [SerializeField] private NavMeshObstacle obstacle = null;

    public bool CanBePlaced {get{return canBePlaced;} set{canBePlaced = value;}}
    public bool IsPlaced {get{return isPlaced;} set{isPlaced = value;}}
    public bool IsBuilt {get{return isBuilt;} set{isBuilt = value;}}
    public GameUnit GUnit {get{return gUnit;} set{gUnit = value;}}

    // Use this for initialization
    public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {

	}

    private void ValidatePlacement() {
        CanBePlaced = true;
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in rens) {
            ren.material.color = Color.green;
        }
    }

    private void InvalidatePlacement() {
        CanBePlaced = false;
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in rens) {
            ren.material.color = Color.red;
        }
    }

    public void PlaceBuilding() {
        if (CanBePlaced) {
            obstacle.enabled = true;
            IsPlaced = true;
            MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer ren in rens) {
                ren.material.color = Color.white;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!isPlaced) {
            if (gUnit.Data.GetType() == typeof(ResourceBuilding) && !canBePlaced) {
                ResourceBuilding rBuilding = (ResourceBuilding)gUnit.Data;
                ResourceUnit rUnit = other.GetComponent<ResourceUnit>();
                if (rUnit != null && rUnit.RType == rBuilding.RType) {
                    ValidatePlacement();
                }
            }
            else {
                if (CanBePlaced) {
                    InvalidatePlacement();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!isPlaced) {
            if (gUnit.Data.GetType() == typeof(ResourceBuilding) && !canBePlaced) {
                ResourceBuilding rBuilding = (ResourceBuilding)gUnit.Data;
                ResourceUnit rUnit = other.GetComponent<ResourceUnit>();
                if (rUnit != null && rUnit.RType == rBuilding.RType) {
                    ValidatePlacement();
                }
            }
            else {
                if (CanBePlaced) {
                    InvalidatePlacement();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!isPlaced) {
            if (gUnit.Data.GetType() == typeof(ResourceBuilding) && canBePlaced) {
                ResourceBuilding rBuilding = (ResourceBuilding)gUnit.Data;
                ResourceUnit rUnit = other.GetComponent<ResourceUnit>();
                if (rUnit != null && rUnit.RType == rBuilding.RType) {
                    InvalidatePlacement();
                }
            }
            else {
                if (!CanBePlaced) {
                    ValidatePlacement();
                }
            }
        }
    }
}
