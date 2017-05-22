using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private static Player instance = null;
    private GameManager gm = null;
    [SerializeField] private PlayerCamera cam;
    [SerializeField] private int team = 0;
    private Formation currentForm = new BoxFormation();
    private List<GameUnit> selectedUnits = new List<GameUnit>();
    [SerializeField] private int maxSelectionCount = 0;
    private GameUnit designatedUnit = null;
    private bool isSelecting = false;
    private Rect selection = new Rect(0, 0, 0, 0);
    private Texture2D selectionVisual;
    private Vector3 startClick = -Vector3.one;
    private GameObject tarBuildingObj;
    private Building tarBuilding;
    [SerializeField] private int techLevel = 1;
    [SerializeField] private int maxPop = 0;
    [SerializeField] private int currentPop = 0;
    [SerializeField] private int metalCount = 0;
    [SerializeField] private int fuelCount = 0;
    [SerializeField] private int foodCount = 0;

    #region Properties
    public static Player Instance {get{return instance;} set{instance = value;}}
    public int Team {get{return team;} set{team = value;}}
    public bool IsSelecting {get{return isSelecting;} set{isSelecting = value;}}
    public Rect Selection {get{return selection;} set{selection = value;}}
    public List<GameUnit> SelectedUnits {get{return selectedUnits;} set{selectedUnits = value;}}
    public int MaxSelectionCount {get{return maxSelectionCount;} set{maxSelectionCount = value;}}
    public GameUnit DesignatedUnit{get{return designatedUnit;} set{designatedUnit = value;}}
    public GameObject TarBuildingObj {get{return tarBuildingObj;} set{tarBuildingObj = value;}}
    public Building TarBuilding {get{return tarBuilding;} set{tarBuilding = value;}}
    public int MaxPop {get{return maxPop;} set{maxPop = value;}}
    public int CurrentPop {get{return currentPop;} set{currentPop = value;}}
    public int MetalCount {get{return metalCount;} set{metalCount = value;}}
    public int FuelCount {get{return fuelCount;} set{fuelCount = value;}}
    public int FoodCount {get{return foodCount;} set{foodCount = value;}}
    public int TechLevel {get{return techLevel;} set{techLevel = value;}}
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
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    if (Input.GetMouseButtonDown(0)) {
                        startClick = Input.mousePosition;
                        isSelecting = true;
                    }
                    if (Input.GetMouseButtonDown(1)) {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                        if (Physics.Raycast(ray, out hit, 200, mask)) {
                            if (selectedUnits.Count == 1) {
                                if (designatedUnit.Data.GetType() == typeof(OffensiveUnit) || designatedUnit.Data.GetType().IsSubclassOf(typeof(OffensiveUnit))) {
                                    OffensiveUnit unit = selectedUnits[0].GetComponent<OffensiveUnit>();
                                    unit.MoveTo(hit.point, UnitStates.TRANSIT);
                                }
                                else if (designatedUnit.Data.GetType() == typeof(UnitBuilding) || designatedUnit.Data.GetType().IsSubclassOf(typeof(UnitBuilding))) {
                                    UnitBuilding building = selectedUnits[0].GetComponent<UnitBuilding>();
                                    building.UnitRallyPoint.position = hit.point;
                                }
                            }
                            else if (selectedUnits.Count > 1) {
                                if (designatedUnit.Data.GetType() == typeof(OffensiveUnit) || designatedUnit.Data.GetType().IsSubclassOf(typeof(OffensiveUnit))) {
                                    if (currentForm != null && currentForm.MaxUnitCount >= selectedUnits.Count) {
                                        OffensiveUnit unit = null;
                                        for (int i = 0; i < selectedUnits.Count; i++) {
                                            unit = selectedUnits[i].GetComponent<OffensiveUnit>();
                                            if (unit != null) {
                                                unit.MoveTo(hit.point + currentForm.Positions[i], UnitStates.TRANSIT);
                                            }
                                        }
                                    }
                                }
                                else if (designatedUnit.Data.GetType() == typeof(UnitBuilding) || designatedUnit.Data.GetType().IsSubclassOf(typeof(UnitBuilding))) {
                                    UnitBuilding building = null;
                                    for (int i = 0; i < selectedUnits.Count; i++) {
                                        building = selectedUnits[i].GetComponent<UnitBuilding>();
                                        if (building != null) {
                                            building.UnitRallyPoint.position = hit.point;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Input.GetMouseButton(0)) {
                    if (isSelecting) {
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
                }
                if (Input.GetMouseButtonUp(0)) {
                    startClick = -Vector3.one;
                    isSelecting = false;
                }
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    if (selectedUnits.Count > 0 && designatedUnit.Data.GetType() == typeof(OffensiveUnit)) {
                        currentForm = new LineFormation();
                        OffensiveUnit unit = null;
                        Vector3 cenPos = Vector3.zero;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            cenPos += selectedUnits[i].transform.position;
                        }
                        cenPos /= selectedUnits.Count - 1;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            unit = selectedUnits[i].GetComponent<OffensiveUnit>();
                            if (unit != null) {
                                unit.MoveTo(cenPos + currentForm.Positions[i], UnitStates.TRANSIT);
                            }
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    if (selectedUnits.Count > 0) {
                        currentForm = new ZipperFormation();
                        OffensiveUnit unit = null;
                        Vector3 cenPos = Vector3.zero;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            cenPos += selectedUnits[i].transform.position;
                        }
                        cenPos /= selectedUnits.Count - 1;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            unit = selectedUnits[i].GetComponent<OffensiveUnit>();
                            if (unit != null) {
                                unit.MoveTo(cenPos + currentForm.Positions[i], UnitStates.TRANSIT);
                            }
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha3)) {
                    if (selectedUnits.Count > 0) {
                        currentForm = new BoxFormation();
                        OffensiveUnit unit = null;
                        Vector3 cenPos = Vector3.zero;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            cenPos += selectedUnits[i].transform.position;
                        }
                        cenPos /= selectedUnits.Count - 1;
                        for (int i = 0; i < selectedUnits.Count; i++) {
                            unit = selectedUnits[i].GetComponent<OffensiveUnit>();
                            if (unit != null) {
                                unit.MoveTo(cenPos + currentForm.Positions[i], UnitStates.TRANSIT);
                            }
                        }
                    }
                }
                break;
            case GameStates.BUILD:
                // Check if the mouse was clicked over a UI element
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    if (Input.GetMouseButtonDown(0)) {
                        //Place building
                        if (tarBuilding.CanBePlaced) {
                            tarBuilding.PlaceBuilding();
                            //Switch to play mode
                            SwitchState("PLAY");
                        }
                    }
                    if (Input.GetMouseButtonDown(1)) {
                        //Destroy building
                        Destroy(tarBuildingObj);
                        //Switch to play mode
                        SwitchState("PLAY");
                    }
                }
                break;
            case GameStates.PAUSE:
                break;
        }
    }

    public bool CanAfford(GameUnit unit) {
        if (unit.MetalCost <= metalCount && unit.FuelCost <= fuelCount && unit.FoodCost <= foodCount && currentPop + unit.PopCost <= maxPop) {
            return true;
        }
        else {
            return false;
        }
    }

    public void PurchaseUnit(GameUnit unit) {
        metalCount -= unit.MetalCost;
        fuelCount -= unit.FuelCost;
        foodCount -= unit.FoodCost;
        currentPop += unit.PopCost;
    }

    public void RefundUnit(GameUnit unit) {
        metalCount += unit.MetalCost;
        fuelCount += unit.FuelCost;
        foodCount += unit.FoodCost;
        currentPop -= unit.PopCost;
    }

    public void SetDesignatedUnit(GameUnit unit) {
        designatedUnit = unit;
    }

    public void SwitchState(string stateName) {
        gm.CurrentState = (GameStates)System.Enum.Parse(typeof(GameStates), stateName);
    }

    public float InvertMouseY(float y) {
        return Screen.height - y;
    }

    private void OnGUI() {
        if (startClick != -Vector3.one) {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(Selection, selectionVisual);
        }
    }
}   
