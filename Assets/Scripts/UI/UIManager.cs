using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    [SerializeField] private List<Button> buildingButtons = new List<Button>();
    [SerializeField] private GameObject groupInfoPanel = null;
    [SerializeField] private GameObject cmdPanel = null;
    private List<UnitTile> unitTiles = new List<UnitTile>();
    private GameManager gm = null;
    private Player player = null;
    [SerializeField] private Text metalCount = null;
    [SerializeField] private Text fuelCount = null;
    [SerializeField] private Text foodCount = null;
    [SerializeField] private Text currentTechText = null;
    [SerializeField] private Text popCountText = null;
    [SerializeField] private Image designatedUnitPortrait = null;
    [SerializeField] private Slider designatedUnitSlider = null;
    [SerializeField] private Text designatedUnitHealthText = null;
    [SerializeField] private Text designatedUnitName = null;
    [SerializeField] private Text designatedUnitPopCost = null;
    [SerializeField] private Text designatedUnitDamage = null;
    [SerializeField] private Text designatedUnitAttackSpeed = null;
    [SerializeField] private Text designatedUnitAttackRange = null;
    [SerializeField] private Text designatedUnitMoveSpeed = null;
    [SerializeField] private Text designatedUnitVisionRadius = null;
    [SerializeField] private Text designatedUnitTier = null;
    [SerializeField] private Sprite defaultCmdSprite = null;
    [SerializeField] private List<Button> cmdBtns = new List<Button>();

    #region Properties
    public static UIManager Instance {get{return instance;} set{instance = value;}}
    public List<Button> CmdBtns {get{return cmdBtns;} set{cmdBtns = value;}}
    #endregion

    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateResourcePanel();
        UpdateTechPanel();
        UpdatePopCount();
        UpdateUnitInfoPanel();
        UpdateGroupInfoPanel();
        UpdateCommandPanel();
	}

    private void Init() {
        gm = GameManager.Instance;
        player = Player.Instance;
        unitTiles.AddRange(groupInfoPanel.GetComponentsInChildren<UnitTile>());
        cmdBtns.AddRange(cmdPanel.GetComponentsInChildren<Button>());
    }

    public void ToggleBuildMenu() {
        foreach (Button button in buildingButtons) {
            if (button.gameObject.activeSelf) {
                button.gameObject.SetActive(false);
            }
            else {
                button.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateResourcePanel() {
        metalCount.text = "Metal: " + player.MetalCount;
        fuelCount.text = "Fuel: " + player.FuelCount;
        foodCount.text = "Food: " + player.FoodCount;
    }

    public void UpdateTechPanel() {
        currentTechText.text = "Current Tech:" + "\n";
        switch (player.TechLevel) {
            case 1:
                currentTechText.text += "I";
                break;
            case 2:
                currentTechText.text += "II";
                break;
            case 3:
                currentTechText.text += "III";
                break;
            case 4:
                currentTechText.text += "IV";
                break;
            case 5:
                currentTechText.text += "V";
                break;
        }
    }

    public void UpdatePopCount() {
        popCountText.text = "Current Pop: " + player.CurrentPop;
    }

    public void UpdateUnitInfoPanel() {
        //Update with info of designated unit
        if (player.DesignatedUnit != null) {
            if (player.DesignatedUnit.Data.GetType() == typeof(OffensiveUnit) || player.DesignatedUnit.Data.GetType().IsSubclassOf(typeof(OffensiveUnit))) {
                OffensiveUnit unit = player.DesignatedUnit.GetComponent<OffensiveUnit>();
                if (!designatedUnitPortrait.gameObject.activeSelf) {
                    designatedUnitPortrait.gameObject.SetActive(true);
                    designatedUnitSlider.gameObject.SetActive(true);
                    designatedUnitHealthText.gameObject.SetActive(true);
                    designatedUnitName.gameObject.SetActive(true);
                    designatedUnitPopCost.gameObject.SetActive(true);
                    designatedUnitDamage.gameObject.SetActive(true);
                    designatedUnitAttackSpeed.gameObject.SetActive(true);
                    designatedUnitAttackRange.gameObject.SetActive(true);
                    designatedUnitMoveSpeed.gameObject.SetActive(true);
                    designatedUnitVisionRadius.gameObject.SetActive(true);
                    designatedUnitTier.gameObject.SetActive(true);
                }
                if (!designatedUnitDamage.gameObject.activeSelf) {
                    designatedUnitDamage.gameObject.SetActive(true);
                    designatedUnitAttackSpeed.gameObject.SetActive(true);
                    designatedUnitAttackRange.gameObject.SetActive(true);
                    designatedUnitMoveSpeed.gameObject.SetActive(true);
                }
                designatedUnitPortrait.sprite = unit.GUnit.UnitPortrait;
                designatedUnitSlider.maxValue = unit.GUnit.MaxHealth;
                designatedUnitSlider.value = unit.GUnit.Health;
                designatedUnitHealthText.text = "(" + unit.GUnit.Health + "/" + unit.GUnit.MaxHealth + ")";
                designatedUnitName.text = unit.GUnit.UnitName;
                designatedUnitPopCost.text = "Population: " + unit.GUnit.PopCost;
                designatedUnitDamage.text = "Damage: " + unit.Damage;
                designatedUnitAttackSpeed.text = "Attackspeed: " + unit.AttackSpeed;
                designatedUnitAttackRange.text = "Range: " + unit.AttackRange;
                designatedUnitMoveSpeed.text = "Movespeed: " + unit.Movespeed;
                designatedUnitVisionRadius.text = "Vision Radius: " + unit.GUnit.VisionRadius;
                designatedUnitTier.text = "Unit Tier: " + unit.GUnit.UnitTier;
            }
            else if (player.DesignatedUnit.Data.GetType() == typeof(Building) || player.DesignatedUnit.Data.GetType().IsSubclassOf(typeof(Building))) {
                Building building = player.DesignatedUnit.GetComponent<Building>();
                if (!designatedUnitPortrait.gameObject.activeSelf) {
                    designatedUnitPortrait.gameObject.SetActive(true);
                    designatedUnitSlider.gameObject.SetActive(true);
                    designatedUnitHealthText.gameObject.SetActive(true);
                    designatedUnitName.gameObject.SetActive(true);
                    designatedUnitVisionRadius.gameObject.SetActive(true);
                    designatedUnitTier.gameObject.SetActive(true);
                }
                if (designatedUnitDamage.gameObject.activeSelf) {
                    designatedUnitDamage.gameObject.SetActive(false);
                    designatedUnitAttackSpeed.gameObject.SetActive(false);
                    designatedUnitAttackRange.gameObject.SetActive(false);
                    designatedUnitMoveSpeed.gameObject.SetActive(false);
                }
                designatedUnitPortrait.sprite = building.GUnit.UnitPortrait;
                designatedUnitSlider.maxValue = building.GUnit.MaxHealth;
                designatedUnitSlider.value = building.GUnit.Health;
                designatedUnitHealthText.text = "(" + building.GUnit.Health + "/" + building.GUnit.MaxHealth + ")";
                designatedUnitName.text = building.GUnit.UnitName;
                designatedUnitVisionRadius.text = "Vision Radius: " + building.GUnit.VisionRadius;
                designatedUnitTier.text = "Unit Tier: " + building.GUnit.UnitTier;
            }
        }
        else {
            designatedUnitPortrait.gameObject.SetActive(false);
            designatedUnitSlider.gameObject.SetActive(false);
            designatedUnitName.gameObject.SetActive(false);
            designatedUnitPopCost.gameObject.SetActive(false);
            designatedUnitDamage.gameObject.SetActive(false);
            designatedUnitAttackSpeed.gameObject.SetActive(false);
            designatedUnitAttackRange.gameObject.SetActive(false);
            designatedUnitMoveSpeed.gameObject.SetActive(false);
            designatedUnitVisionRadius.gameObject.SetActive(false);
            designatedUnitTier.gameObject.SetActive(false);
        }
    }

    public void UpdateGroupInfoPanel() {
        foreach (UnitTile tile in unitTiles) {
            if (tile.Unit == null && tile.isActiveAndEnabled) {
                tile.gameObject.SetActive(false);
            }
        }
    }

    public void AddUnitToGroup(GameUnit unit) {
        foreach (UnitTile tile in unitTiles) {
            if (tile.Unit == null) {
                tile.Unit = unit;
                tile.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void RemoveUnitFromGroup(GameUnit unit) {
        foreach (UnitTile tile in unitTiles) {
            if (tile.Unit == unit) {
                tile.Unit = null;
                tile.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void UpdateCommandPanel() {
        DisableCommands();
        if (player.DesignatedUnit != null) {
            if (player.DesignatedUnit.Data.GetType() == typeof(OffensiveUnit)) {
                OffensiveUnit unit = player.DesignatedUnit.GetComponent<OffensiveUnit>();
                EnableCommand(cmdBtns[0], Resources.Load<Sprite>("CommandSprites/Attack"), string.Empty);
                cmdBtns[0].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.ATTACK); });
                EnableCommand(cmdBtns[1], Resources.Load<Sprite>("CommandSprites/Defend"), string.Empty);
                cmdBtns[1].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.DEFEND); });
                EnableCommand(cmdBtns[2], Resources.Load<Sprite>("CommandSprites/Patrol"), string.Empty);
                cmdBtns[2].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.PATROL); });
                EnableCommand(cmdBtns[3], Resources.Load<Sprite>("CommandSprites/Guard"), string.Empty);
                cmdBtns[3].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.GUARD); });
                EnableCommand(cmdBtns[4], Resources.Load<Sprite>("CommandSprites/Follow"), string.Empty);
                cmdBtns[4].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.FOLLOW); });
                EnableCommand(cmdBtns[5], Resources.Load<Sprite>("CommandSprites/DoNothing"), string.Empty);
                cmdBtns[5].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.DO_NOTHING); });
            }
            else if (player.DesignatedUnit.Data.GetType() == typeof(WorkerUnit)) {
                WorkerUnit worker = player.DesignatedUnit.GetComponent<WorkerUnit>();
                if (!worker.InBuildMenu) {
                    EnableCommand(cmdBtns[0], Resources.Load<Sprite>("CommandSprites/Attack"), string.Empty);
                    cmdBtns[0].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.ATTACK); });
                    EnableCommand(cmdBtns[1], Resources.Load<Sprite>("CommandSprites/Defend"), string.Empty);
                    cmdBtns[1].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.DEFEND); });
                    EnableCommand(cmdBtns[2], Resources.Load<Sprite>("CommandSprites/Patrol"), string.Empty);
                    cmdBtns[2].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.PATROL); });
                    EnableCommand(cmdBtns[3], Resources.Load<Sprite>("CommandSprites/Guard"), string.Empty);
                    cmdBtns[3].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.GUARD); });
                    EnableCommand(cmdBtns[4], Resources.Load<Sprite>("CommandSprites/Follow"), string.Empty);
                    cmdBtns[4].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.FOLLOW); });
                    EnableCommand(cmdBtns[5], Resources.Load<Sprite>("CommandSprites/DoNothing"), string.Empty);
                    cmdBtns[5].onClick.AddListener(delegate { player.BeginTargeting(UnitStates.DO_NOTHING); });
                    EnableCommand(cmdBtns[6], Resources.Load<Sprite>("CommandSprites/Build"), string.Empty);
                    cmdBtns[6].onClick.AddListener(delegate { worker.ToggleBuildMenu(); });
                }
                else {
                    for (int i = 0; i < worker.BuildingUnits.Count; i++) {
                        int tempInt = i;
                        EnableCommand(cmdBtns[i], worker.BuildingUnits[i].GUnit.UnitPortrait, string.Empty);
                        cmdBtns[i].onClick.AddListener(delegate { gm.CreateBuilding(worker.BuildingUnits[tempInt].name); });
                    }
                    EnableCommand(cmdBtns[worker.BuildingUnits.Count], Resources.Load<Sprite>("CommandSprites/Cancel"), string.Empty);
                    cmdBtns[worker.BuildingUnits.Count].onClick.AddListener(delegate { worker.ToggleBuildMenu(); });
                }
            }
            else if (player.DesignatedUnit.Data.GetType() == typeof(UnitBuilding)) {
                UnitBuilding building = player.DesignatedUnit.GetComponent<UnitBuilding>();
                EnableCommand(cmdBtns[0], Resources.Load<Sprite>("CommandSprites/RallyPoint"), string.Empty);
                cmdBtns[0].onClick.AddListener(delegate { print("Rally Command Issued!"); });
                EnableCommand(cmdBtns[1], Resources.Load<Sprite>("CommandSprites/Demolish"), string.Empty);
                cmdBtns[1].onClick.AddListener(delegate { print("Demolish Command Issued!"); });
                for (int i = 2; i - 2 < building.ProductionUnits.Count; i++)
                {
                    int tempInt = i;
                    EnableCommand(cmdBtns[i], building.ProductionUnits[i - 2].GUnit.UnitPortrait, string.Empty);
                    cmdBtns[i].onClick.AddListener(delegate { building.AddToBuildQueue(building.ProductionUnits[tempInt - 2]); });
                }
            }
        }
    }

    private void EnableCommand(Button btn, Sprite spr, string txt) {
        btn.interactable = true;
        btn.image.sprite = spr;
        btn.GetComponentInChildren<Text>().text = txt;
    }

    private void DisableCommand(Button btn) {
        btn.interactable = false;
        btn.image.sprite = null;
        btn.GetComponentInChildren<Text>().text = string.Empty;
        btn.onClick.RemoveAllListeners();
    }

    private void DisableCommands() {
        foreach (Button cmdBtn in cmdBtns) {
            cmdBtn.interactable = false;
            cmdBtn.image.sprite = null;
            cmdBtn.GetComponentInChildren<Text>().text = string.Empty;
            cmdBtn.onClick.RemoveAllListeners();
        }
    }
}
