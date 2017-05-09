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

    public void CreateBuilding(string name)
    {
        GameObject tmpGO = (GameObject)Resources.Load(buildingPath + name);
        tmpGO = Instantiate(tmpGO);
        Player.Instance.BuildingObj = tmpGO;
        Player.Instance.SwitchState("BUILD");
    }
}
