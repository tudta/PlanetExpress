using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private static Player instance = null;
    [SerializeField] private GameStates currentState = GameStates.DEFAULT;
    private GameObject buildingObj = null;

    public static Player Instance
    {
        get
        {
            return instance;
        }

        set
        {
            if (instance == null)
            {
                instance = value;
            }
        }
    }

    public GameObject BuildingObj
    {
        get
        {
            return buildingObj;
        }

        set
        {
            buildingObj = value;
        }
    }

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        switch (currentState)
        {
            case GameStates.PLAY:
                break;
            case GameStates.BUILD:
                if (buildingObj != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    LayerMask mask = 1 << 8;
                    if (Physics.Raycast(ray, out hit, 100.0f, mask))
                    {
                        print(hit.point);
                        Vector3 tmpV3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, hit.distance);
                        tmpV3 = Camera.main.ScreenToWorldPoint(tmpV3);
                        tmpV3 = new Vector3(tmpV3.x, hit.point.y, tmpV3.z);
                        buildingObj.transform.position = tmpV3;
                    }
                }
                break;
        }
    }

    void CheckInput()
    {
        switch (currentState)
        {
            case GameStates.DEFAULT:
                break;
            case GameStates.PLAY:
                break;
            case GameStates.BUILD:
                if (Input.GetMouseButton(0))
                {
                    //Place building
                    buildingObj = null;
                    //Switch to play mode
                    SwitchState("PLAY");
                }
                if (Input.GetMouseButton(1))
                {
                    //Destroy building
                    Destroy(buildingObj);
                    //Switch to play mode
                    SwitchState("PLAY");
                }
                break;
            case GameStates.PAUSE:
                break;
        }
    }

    public void SwitchState(string stateName)
    {
        currentState = (GameStates)System.Enum.Parse(typeof(GameStates), stateName);
    }

}   
