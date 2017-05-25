using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitTile : MonoBehaviour {
    [SerializeField] private Image unitImage = null;
    [SerializeField] private Slider healthSlider = null;
    [SerializeField] private GameUnit unit = null;

    #region Properties
    public GameUnit Unit {get{return unit;} set{unit = value;}}
    #endregion

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (unit != null) {
            UpdateUIElements();
        }
	}

    private void UpdateUIElements() {
        unitImage.sprite = unit.UnitPortrait;
        healthSlider.maxValue = unit.MaxHealth;
        healthSlider.value = unit.Health;
    }

    public void SetDesignatedUnit() {
        Player.Instance.DesignatedUnit = unit;
    }
}
