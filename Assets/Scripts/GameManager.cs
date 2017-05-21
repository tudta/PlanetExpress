﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [SerializeField] private GameStates currentState = GameStates.PLAY;
    private string buildingPath = "Prefabs/";
    private Player player = null;

    public static GameManager Instance {get{return instance;} set{instance = value;}}
    public GameStates CurrentState {get{return currentState;} set{currentState = value;}}
    
    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        player = Player.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateBuilding(string name) {
        //GameObject tmpGO = null;
        GameObject tmpGO = (GameObject)Resources.Load(buildingPath + name);
        Building tmpBuilding = tmpGO.GetComponent<Building>();
        if (player.CanAfford(tmpBuilding)) {
            tmpGO = Instantiate(tmpGO, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            tmpBuilding = tmpGO.GetComponent<Building>();
            player.TarBuildingObj = tmpGO;
            player.TarBuilding = tmpBuilding;
            player.SwitchState("BUILD");
        }
    }
}
