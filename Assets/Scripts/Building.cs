using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
    [SerializeField] private bool canBePlaced = false;
    [SerializeField] private bool isPlaced = false;
    [SerializeField] private bool isBuilt = false;
    [SerializeField] private float productionRate = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Produce() {

    }

    void ValidatePlacement() {
        canBePlaced = true;
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in rens) {
            ren.material.color = Color.green;
        }
    }

    void InvalidatePlacement() {
        canBePlaced = false;
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in rens) {
            ren.material.color = Color.red;
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (isPlaced == false) {
            if (canBePlaced) {
                InvalidatePlacement();
            }
        }
    }

    void OnTriggerStay(Collider other) {
        Debug.Log(other.name);
        if (isPlaced == false) {
            if (canBePlaced) {
                InvalidatePlacement();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        Debug.Log(other.name);
        if (isPlaced == false) {
            if (!canBePlaced) {
                ValidatePlacement();
            }
        }
    }
}
