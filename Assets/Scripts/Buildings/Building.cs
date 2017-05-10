using UnityEngine;
using System.Collections;

public abstract class Building : MonoBehaviour {
    [SerializeField] private bool canBePlaced = false;
    [SerializeField] private bool isPlaced = false;
    [SerializeField] private bool isBuilt = false;

    public bool CanBePlaced {get{return canBePlaced;} set{canBePlaced = value;}}
    public bool IsPlaced {get{return isPlaced;} set{isPlaced = value;}}
    public bool IsBuilt {get{return isBuilt;} set{isBuilt = value;}}

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void Produce() {

    }

    void ValidatePlacement() {
        CanBePlaced = true;
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in rens) {
            ren.material.color = Color.green;
        }
    }

    void InvalidatePlacement() {
        CanBePlaced = false;
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in rens) {
            ren.material.color = Color.red;
        }
    }

    public void PlaceBuilding() {
        if (CanBePlaced)
        {
            IsPlaced = true;
            MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer ren in rens)
            {
                ren.material.color = Color.white;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (IsPlaced == false) {
            if (CanBePlaced) {
                InvalidatePlacement();
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (IsPlaced == false) {
            if (CanBePlaced) {
                InvalidatePlacement();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (IsPlaced == false) {
            if (!CanBePlaced) {
                ValidatePlacement();
            }
        }
    }
}
