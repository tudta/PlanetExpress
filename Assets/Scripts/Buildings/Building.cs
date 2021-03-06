﻿using UnityEngine;
using System.Collections;

public abstract class Building : GameUnit {
    [SerializeField] private bool canBePlaced = false;
    [SerializeField] private bool isPlaced = false;
    [SerializeField] private bool isBuilt = false;

    public bool CanBePlaced {get{return canBePlaced;} set{canBePlaced = value;}}
    public bool IsPlaced {get{return isPlaced;} set{isPlaced = value;}}
    public bool IsBuilt {get{return isBuilt;} set{isBuilt = value;}}

    // Use this for initialization
    public override void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {

	}

    private void Produce() {

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

    private void OnTriggerEnter(Collider other) {
        if (IsPlaced == false) {
            if (CanBePlaced) {
                InvalidatePlacement();
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (IsPlaced == false) {
            if (CanBePlaced) {
                InvalidatePlacement();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (IsPlaced == false) {
            if (!CanBePlaced) {
                ValidatePlacement();
            }
        }
    }
}
