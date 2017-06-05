using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUnit : MonoBehaviour {
    [SerializeField] private GameUnitTypes gUnitType = GameUnitTypes.NONE;
    [SerializeField] private int team = 0;
    [SerializeField] private Sprite unitPortrait = null;
    [SerializeField] private string unitName = string.Empty;
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private float visionRadius = 0.0f;
    [SerializeField] private int unitTier = 0;
    [SerializeField] private int metalCost = 0;
    [SerializeField] private int fuelCost = 0;
    [SerializeField] private int foodCost = 0;
    [SerializeField] private int popCost = 0;
    [SerializeField] private Component data = null;
    private Player playerEnt = null;
    private bool isSelected = false;
    [SerializeField] private MeshRenderer ren = null;
    private List<MeshRenderer> rens = new List<MeshRenderer>();
    [SerializeField] private SkinnedMeshRenderer skRen = null;
    private List<SkinnedMeshRenderer> skRens = new List<SkinnedMeshRenderer>();
    private List<Color> ogColors = new List<Color>();

    public int Team {get{return team;} set{team = value;}}
    public int Health {get{return currentHealth;} set{currentHealth = value;}}
    public int MaxHealth {get{return maxHealth;} set{maxHealth = value;}}
    public GameUnitTypes GUnitType {get{return gUnitType;} set{gUnitType = value;}}
    public Sprite UnitPortrait {get{return unitPortrait;} set{unitPortrait = value;}}
    public string UnitName {get{return unitName;} set{unitName = value;}}
    public int MetalCost {get{return metalCost;} set{metalCost = value;}}
    public int FuelCost {get{return fuelCost;} set{fuelCost = value;}}
    public int FoodCost {get{return foodCost;} set{foodCost = value;}}
    public int PopCost {get{return popCost;} set{popCost = value;}}
    public Component Data {get{return data;} set{data = value;}}
    public int UnitTier {get{return unitTier;} set{unitTier = value;}}
    public float VisionRadius {get{return visionRadius;} set{visionRadius = value;}}
    public Player PlayerEnt {get{return playerEnt;} set{playerEnt = value;}}

    public virtual void Awake() {

    }

    // Use this for initialization
    public virtual void Start () {
        PlayerEnt = Player.Instance;
        GetColors();
    }
	
	// Update is called once per frame
	public virtual void Update () {
        //Check if player is selecting unit
        if (ren != null) {
            if (ren.isVisible && PlayerEnt.IsDragSelecting) {
                Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
                camPos.y = PlayerEnt.InvertMouseY(camPos.y);
                if (PlayerEnt.Selection.Contains(camPos) && team == Player.Instance.Team) {
                    if (!isSelected) {
                        SelectUnit();
                    }
                }
                else if (isSelected) {
                    UnselectUnit();
                }
            }
        }
        else if (skRen != null) {
            if (skRen.isVisible && PlayerEnt.IsDragSelecting) {
                Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
                camPos.y = PlayerEnt.InvertMouseY(camPos.y);
                if (PlayerEnt.Selection.Contains(camPos) && team == Player.Instance.Team) {
                    if (!isSelected) {
                        SelectUnit();
                    }
                }
                else if (isSelected) {
                    UnselectUnit();
                }
            }
        }
    }

    public void SelectUnit() {
        if (!PlayerEnt.SelectedUnits.Contains(this) && PlayerEnt.SelectedUnits.Count < PlayerEnt.MaxSelectionCount) {
            PlayerEnt.SelectedUnits.Add(this);
            isSelected = true;
            SetColors(Color.green);
            UIManager.Instance.AddUnitToGroup(this);
            if (PlayerEnt.SelectedUnits.Count == 1) {
                PlayerEnt.DesignatedUnit = this;
            }
        }
    }

    public void UnselectUnit() {
        if (PlayerEnt.SelectedUnits.Contains(this)) {
            PlayerEnt.SelectedUnits.Remove(this);
            isSelected = false;
            RevertColors();
            UIManager.Instance.RemoveUnitFromGroup(this);
            if (PlayerEnt.DesignatedUnit == this) {
                PlayerEnt.DesignatedUnit = null;
            }
            if (data.GetType() == typeof(WorkerUnit)) {
                WorkerUnit worker = (WorkerUnit)data;
                worker.InBuildMenu = false;
            }
        }
    }

    private void GetColors() {
        if (ren != null) {
            rens.AddRange(GetComponentsInChildren<MeshRenderer>());
            for (int i = 0; i < rens.Count; i++) {
                ogColors.Add(rens[i].material.color);
            }
        }
        else if (skRen != null) {
            skRens.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
            for (int i = 0; i < skRens.Count; i++) {
                ogColors.Add(skRens[i].material.color);
            }
        }
    }

    public void SetColors(Color col) {
        if (ren != null) {
            for (int i = 0; i < rens.Count; i++) {
                rens[i].material.color = col;
            }
        }
        else if (skRen != null) {
            for (int i = 0; i < skRens.Count; i++) {
                skRens[i].material.color = col;
            }
        }
    }

    public void RevertColors() {
        if (ren != null) {
            for (int i = 0; i < rens.Count; i++) {
                rens[i].material.color = ogColors[i];
            }
        }
        else if (skRen != null) {
            for (int i = 0; i < skRens.Count; i++) {
                skRens[i].material.color = ogColors[i];
            }
        }
    }

    public virtual void ApplyDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            if (team == Player.Instance.Team) {
                Player.Instance.CurrentPop -= popCost;
            }
            Destroy(gameObject);
        }
    }
}
