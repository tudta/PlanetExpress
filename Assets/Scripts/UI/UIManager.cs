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

    #region Properties
    public static UIManager Instance {get{return instance;} set{instance = value;}}
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

    public void UpdateUnitInfoPanel() {

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
