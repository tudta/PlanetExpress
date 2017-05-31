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
        if (!isPlaced) {
            ValidatePlacement();
        }
	}
	
	// Update is called once per frame
	public virtual void Update () {

	}

    public void ValidatePlacement() {
        CanBePlaced = true;
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in rens) {
            ren.material.color = Color.green;
        }
    }

    public void InvalidatePlacement() {
        CanBePlaced = false;
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in rens) {
            ren.material.color = Color.red;
        }
    }

    public virtual void PlaceBuilding() {
        if (CanBePlaced) {
            obstacle.enabled = true;
            IsPlaced = true;
            MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer ren in rens) {
                ren.material.color = Color.white;
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other) {
        if (!isPlaced && canBePlaced) {
            InvalidatePlacement();
        }
    }

    public virtual void OnTriggerStay(Collider other) {
        if (!isPlaced && canBePlaced) {
            InvalidatePlacement();
        }
    }

    public virtual void OnTriggerExit(Collider other) {
        if (!isPlaced && !canBePlaced) {
            ValidatePlacement();
        }
    }
}
