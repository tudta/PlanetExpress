using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private static Player instance = null;
    private GameManager gm = null;
    [SerializeField] private PlayerCamera cam;
    [SerializeField] private int team = 0;
    private Formation currentForm = new BoxFormation();
    private List<BaseUnit> selectedUnits = new List<BaseUnit>();
    [SerializeField] private int maxSelectionCount = 0;
    private static Rect selection = new Rect(0, 0, 0, 0);
    private Texture2D selectionVisual;
    private Vector3 startClick = -Vector3.one;
    private GameObject tarBuildingObj;
    private Building tarBuilding;

    #region Properties
    public static Player Instance {get{return instance;} set{instance = value;}}
    public int Team {get{return team;} set{team = value;}}
    public static Rect Selection {get{return selection;} set{selection = value;}}
    public List<BaseUnit> SelectedUnits {get{return selectedUnits;} set{selectedUnits = value;}}
    public int MaxSelectionCount {get{return maxSelectionCount;} set{maxSelectionCount = value;}}
    public GameObject TarBuildingObj {get{return tarBuildingObj;} set{tarBuildingObj = value;}}
    public Building TarBuilding {get{return tarBuilding;} set{tarBuilding = value;}}
    #endregion

    void Awake() {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        gm = GameManager.Instance;
        selectionVisual = Resources.Load("Images/GoldHighlight") as Texture2D;
    }
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        switch (gm.CurrentState) {
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
                if (tarBuildingObj != null) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                    if (Physics.Raycast(ray, out hit, 200.0f, mask)) {
                        Vector3 tmpV3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, hit.distance);
                        tmpV3 = Camera.main.ScreenToWorldPoint(tmpV3);
                        tmpV3 = new Vector3(tmpV3.x, hit.point.y, tmpV3.z);
                        tarBuildingObj.transform.position = tmpV3;
                    }
                }
                break;
        }
    }

    void CheckInput() {
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f) {
            cam.ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        }
        switch (gm.CurrentState) {
            case GameStates.DEFAULT:
                break;
            case GameStates.PLAY:
                if (Input.GetMouseButtonDown(1)) {
                    //print ("Right-Clicked!");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                    if (Physics.Raycast(ray, out hit, 200, mask)) {
                        if (selectedUnits.Count == 1) {
                            selectedUnits[0].MoveTo(hit.point, UnitStates.TRANSIT);
                        }
                        if (selectedUnits.Count > 1) {
                            if (currentForm != null && currentForm.MaxUnitCount >= selectedUnits.Count) {
                                for (int i = 0; i < selectedUnits.Count; i++) {
                                    selectedUnits[i].MoveTo(hit.point + currentForm.Positions[i], UnitStates.TRANSIT);
                                }
                            }
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    if (selectedUnits.Count > 0) {
                        currentForm = new LineFormation();
                        Vector3 cenPos = Vector3.zero;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            cenPos += selectedUnits[i].transform.position;
                        }
                        cenPos /= selectedUnits.Count - 1;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            selectedUnits[i].MoveTo(cenPos + currentForm.Positions[i], UnitStates.TRANSIT);
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    if (selectedUnits.Count > 0) {
                        currentForm = new ZipperFormation();
                        Vector3 cenPos = Vector3.zero;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            cenPos += selectedUnits[i].transform.position;
                        }
                        cenPos /= selectedUnits.Count - 1;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            selectedUnits[i].MoveTo(cenPos + currentForm.Positions[i], UnitStates.TRANSIT);
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha3)) {
                    if (selectedUnits.Count > 0) {
                        currentForm = new BoxFormation();
                        Vector3 cenPos = Vector3.zero;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            cenPos += selectedUnits[i].transform.position;
                        }
                        cenPos /= selectedUnits.Count - 1;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            selectedUnits[i].MoveTo(cenPos + currentForm.Positions[i], UnitStates.TRANSIT);
                        }
                    }
                }
                break;
            case GameStates.BUILD:
                if (Input.GetMouseButton(0)) {
                    //Place building
                    if (tarBuilding.CanBePlaced) {
                        tarBuilding.PlaceBuilding();
                        //Switch to play mode
                        SwitchState("PLAY");
                    }
                }
                if (Input.GetMouseButton(1)) {
                    //Destroy building
                    Destroy(tarBuildingObj);
                    //Switch to play mode
                    SwitchState("PLAY");
                }
                break;
            case GameStates.PAUSE:
                break;
        }
    }

    public void SwitchState(string stateName) {
        gm.CurrentState = (GameStates)System.Enum.Parse(typeof(GameStates), stateName);
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
