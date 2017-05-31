using UnityEngine;
using System.Collections;

public class ResourceBuilding : Building {
    [SerializeField] private ResourceType rType = ResourceType.NONE;
    private float gatherTimer = 0.0f;
    [SerializeField] private float gatherDuration = 0.0f;
    [SerializeField] private int gatherCountMin = 0;
    [SerializeField] private int gatherCountMax = 0;
    private ResourceUnit rUnit = null;
    private Transform rTrans = null;
    //Reference to resource unit

    public ResourceType RType {get{return rType;} set{rType = value;}}
    public ResourceUnit RUnit {get{return rUnit;} set{rUnit = value;}}

    // Use this for initialization
    public override void Start () {
        if (!IsPlaced) {
            InvalidatePlacement();
        }
	}
	
	// Update is called once per frame
	public override void Update () {
        
    }

    public override void PlaceBuilding() {
        transform.position = rTrans.position;
        StartCoroutine(GatherResources());
        base.PlaceBuilding();
    }

    public override void OnTriggerEnter(Collider other) {
        if (!IsPlaced && !CanBePlaced) {
            ResourceBuilding rBuilding = (ResourceBuilding)GUnit.Data;
            rUnit = other.GetComponent<ResourceUnit>();
            if (rUnit != null && rUnit.RType == rBuilding.RType) {
                rTrans = other.transform;
                ValidatePlacement();
            }
        }
    }

    public override void OnTriggerStay(Collider other) {
        if (!IsPlaced && !CanBePlaced) {
            ResourceBuilding rBuilding = (ResourceBuilding)GUnit.Data;
            rUnit = other.GetComponent<ResourceUnit>();
            if (rUnit != null && rUnit.RType == rBuilding.RType) {
                rTrans = other.transform;
                ValidatePlacement();
            }
        }
    }

    public override void OnTriggerExit(Collider other) {
        if (!IsPlaced && CanBePlaced) {
            ResourceBuilding rBuilding = (ResourceBuilding)GUnit.Data;
            rUnit = other.GetComponent<ResourceUnit>();
            if (rUnit != null && rUnit.RType == rBuilding.RType) {
                rTrans = null;
                InvalidatePlacement();
            }
        }
    }

    public IEnumerator GatherResources() {
        if (rUnit != null && rUnit.ResourceCount > 0) {
            yield return new WaitForSeconds(gatherDuration);
            GUnit.PlayerEnt.AddResources(rType, rUnit.RemoveResources(Random.Range(gatherCountMin, gatherCountMax)));
            StartCoroutine(GatherResources());
        }
    }
}
