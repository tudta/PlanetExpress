using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitTile : MonoBehaviour {
    [SerializeField] private Image unitImage = null;
    [SerializeField] private Slider healthSlider = null;
    [SerializeField] private BaseUnit unit = null;

    #region Properties
    public BaseUnit Unit {get{return unit;} set{unit = value;}}
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
        unitImage.sprite = Unit.GUnit.UnitPortrait;
        healthSlider.maxValue = Unit.GUnit.MaxHealth;
        healthSlider.value = Unit.GUnit.Health;
    }
}
