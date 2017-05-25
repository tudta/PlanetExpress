using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//WORKER CLASS: OFFENSIVEUNIT + WORKER FUNCTIONALITY (BUILDING, ETC.)
public class WorkerUnit : OffensiveUnit {
    [SerializeField] private List<Building> buildingUnits = new List<Building>();
    private bool inBuildMenu = false;

    public List<Building> BuildingUnits {get{return buildingUnits;} set{buildingUnits = value;}}
    public bool InBuildMenu {get{return inBuildMenu;} set{inBuildMenu = value;}}

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToggleBuildMenu() {
        if (inBuildMenu) {
            inBuildMenu = false;
        }
        else {
            inBuildMenu = true;
        }
    }
}
