using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    [SerializeField] private List<Button> buildingButtons = new List<Button>();
    [SerializeField] private GameObject groupInfoPanel = null;
    private List<UnitTile> unitTiles = new List<UnitTile>();
    private Player player = null;
    [SerializeField] private Text metalCount = null;
    [SerializeField] private Text fuelCount = null;
    [SerializeField] private Text foodCount = null;
    [SerializeField] private Image designatedUnitPortrait = null;
    [SerializeField] private Slider designatedUnitSlider = null;
    [SerializeField] private Text designatedUnitName = null;
    [SerializeField] private Text designatedUnitPopCost = null;
    [SerializeField] private Text designatedUnitDamage = null;
    [SerializeField] private Text designatedUnitAttackSpeed = null;
    [SerializeField] private Text designatedUnitAttackRange = null;
    [SerializeField] private Text designatedUnitMoveSpeed = null;
    [SerializeField] private Text designatedUnitVisionRadius = null;
    [SerializeField] private Text designatedUnitTier = null;
    [SerializeField] private List<CommandButton> cmdBtns = new List<CommandButton>();

    #region Properties
    public static UIManager Instance {get{return instance;} set{instance = value;}}
    public List<CommandButton> CmdBtns {get{return cmdBtns;} set{cmdBtns = value;}}
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
        UpdateUnitInfoPanel();
        UpdateGroupInfoPanel();
	}

    private void Init() {
        player = Player.Instance;
        unitTiles.AddRange(groupInfoPanel.GetComponentsInChildren<UnitTile>());
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

    public void UpdateUnitInfoPanel() {
        //Update with info of designated unit
        if (player.DesignatedUnit != null) {
            if (!designatedUnitPortrait.gameObject.activeSelf)
            {
                designatedUnitPortrait.gameObject.SetActive(true);
                designatedUnitSlider.gameObject.SetActive(true);
                designatedUnitName.gameObject.SetActive(true);
                designatedUnitPopCost.gameObject.SetActive(true);
                designatedUnitDamage.gameObject.SetActive(true);
                designatedUnitAttackSpeed.gameObject.SetActive(true);
                designatedUnitAttackRange.gameObject.SetActive(true);
                designatedUnitMoveSpeed.gameObject.SetActive(true);
                designatedUnitVisionRadius.gameObject.SetActive(true);
                designatedUnitTier.gameObject.SetActive(true);
            }
            designatedUnitPortrait.sprite = player.DesignatedUnit.GUnit.UnitPortrait;
            designatedUnitSlider.maxValue = player.DesignatedUnit.GUnit.MaxHealth;
            designatedUnitSlider.value = player.DesignatedUnit.GUnit.Health;
            designatedUnitName.text = player.DesignatedUnit.GUnit.UnitName;
            designatedUnitPopCost.text = "Population: " + player.DesignatedUnit.GUnit.PopCost;
            designatedUnitDamage.text = "Damage: " + player.DesignatedUnit.Damage;
            designatedUnitAttackSpeed.text = "Attackspeed: " + player.DesignatedUnit.AttackSpeed;
            designatedUnitAttackRange.text = "Range: " + player.DesignatedUnit.AttackRange;
            designatedUnitMoveSpeed.text = "Movespeed: " + player.DesignatedUnit.Movespeed;
            designatedUnitVisionRadius.text = "Vision Radius: " + player.DesignatedUnit.VisionRadius;
            designatedUnitTier.text = "Unit Tier: " + player.DesignatedUnit.UnitTier;
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

    public void AddUnitToGroup(BaseUnit unit) {
        foreach (UnitTile tile in unitTiles) {
            if (tile.Unit == null) {
                tile.Unit = unit;
                tile.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void RemoveUnitFromGroup(BaseUnit unit) {
        foreach (UnitTile tile in unitTiles) {
            if (tile.Unit == unit) {
                tile.Unit = null;
                tile.gameObject.SetActive(false);
                break;
            }
        }
    }
}
