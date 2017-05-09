using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private string buildingPath = "Prefabs/";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateBuilding(string name) {
        GameObject tmpGO = (GameObject)Resources.Load(buildingPath + name);
        tmpGO = Instantiate(tmpGO);
        Building tmpBuilding = tmpGO.GetComponent<Building>();
        Player.Instance.TarBuildingObj = tmpGO;
        Player.Instance.TarBuilding = tmpBuilding;
        Player.Instance.SwitchState("BUILD");
    }
}
