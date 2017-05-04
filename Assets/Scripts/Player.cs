using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private static Player instance = null;
    [SerializeField] private GameStates currentState = GameStates.PLAY;
    [SerializeField] private PlayerCamera cam;
    private List<BaseUnit> selectedUnits = new List<BaseUnit>();
    private static Rect selection = new Rect(0, 0, 0, 0);
    private Texture2D selectionVisual;
    private Vector3 startClick = -Vector3.one;
    private GameObject buildingObj = null;

    #region Properties
    public static Player Instance {get{return instance;} set{instance = value;}}
    public GameObject BuildingObj {get{return buildingObj;} set{buildingObj = value;}}
    public static Rect Selection {get{return selection;} set{selection = value;}}
    public List<BaseUnit> SelectedUnits {
        get {
            return selectedUnits;
        }

        set {
            selectedUnits = value;
        }
    }
    #endregion

    void Awake() {
        Instance = this;
        selectionVisual = Resources.Load("Images/GoldHighlight") as Texture2D;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        switch (currentState) {
            case GameStates.PLAY:
                if (Input.GetMouseButtonDown(0)) {
                    startClick = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0)) {
                    startClick = -Vector3.one;
                }
                if (Input.GetMouseButton(0)) {
                    selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
                    if (Selection.width < 0) {
                        selection.x += Selection.width;
                        selection.width = -Selection.width;
                    }
                    if (Selection.height < 0) {
                        selection.y += Selection.height;
                        selection.height = -Selection.height;
                    }
                }
                break;
            case GameStates.BUILD:
                if (buildingObj != null) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                    if (Physics.Raycast(ray, out hit, 200.0f, mask)) {
                        Vector3 tmpV3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, hit.distance);
                        tmpV3 = Camera.main.ScreenToWorldPoint(tmpV3);
                        tmpV3 = new Vector3(tmpV3.x, hit.point.y, tmpV3.z);
                        buildingObj.transform.position = tmpV3;
                    }
                }
                break;
        }
    }

    void CheckInput() {
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f) {
            cam.ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        }
        switch (currentState) {
            case GameStates.DEFAULT:
                break;
            case GameStates.PLAY:
                if (Input.GetMouseButtonDown(1)) {
                    //print ("Right-Clicked!");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                    if (Physics.Raycast(ray, out hit, 200, mask)) {
                        if (selectedUnits[0] != null) {
                            foreach (BaseUnit unit in selectedUnits) {
                                unit.MoveTo(hit.point);
                                //currentState = UnitState.TRANSIT;
                            }
                        }
                    }
                }
                break;
            case GameStates.BUILD:
                if (Input.GetMouseButton(0)) {
                    //Place building
                    buildingObj = null;
                    //Switch to play mode
                    SwitchState("PLAY");
                }
                if (Input.GetMouseButton(1)) {
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

    public void SwitchState(string stateName) {
        currentState = (GameStates)System.Enum.Parse(typeof(GameStates), stateName);
    }

    public static float InvertMouseY(float y) {
        return Screen.height - y;
    }

    private void OnGUI() {
        if (startClick != -Vector3.one) {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(Selection, selectionVisual);
        }
    }
}   
