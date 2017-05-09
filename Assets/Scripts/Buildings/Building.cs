using UnityEngine;
using System.Collections;

public abstract class Building : MonoBehaviour {
    [SerializeField] private bool canBePlaced = false;
    [SerializeField] private bool isPlaced = false;
    [SerializeField] private bool isBuilt = false;

    public bool CanBePlaced {get{return canBePlaced;} set{canBePlaced = value;}}

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
            isPlaced = true;
            MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer ren in rens)
            {
                ren.material.color = Color.white;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (isPlaced == false) {
            if (CanBePlaced) {
                InvalidatePlacement();
            }
        }
    }

    void OnTriggerStay(Collider other) {
        Debug.Log(other.name);
        if (isPlaced == false) {
            if (CanBePlaced) {
                InvalidatePlacement();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        Debug.Log(other.name);
        if (isPlaced == false) {
            if (!CanBePlaced) {
                ValidatePlacement();
            }
        }
    }
}
