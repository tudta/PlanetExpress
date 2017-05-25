using UnityEngine;
using System.Collections;

public class ResourceBuilding : Building {
    [SerializeField] private ResourceType rType = ResourceType.NONE;
    private float gatherTimer = 0.0f;
    [SerializeField] private float gatherDuration = 0.0f;
    [SerializeField] private int gatherCountMin = 0;
    [SerializeField] private int gatherCountMax = 0;
    private ResourceUnit rUnit = null;
    //Reference to resource unit

    public ResourceType RType {get{return rType;} set{rType = value;}}
    public ResourceUnit RUnit {get{return rUnit;} set{rUnit = value;}}

    // Use this for initialization
    public override void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
	
	}

    public IEnumerator GatherResources() {
        if (rUnit != null && rUnit.ResourceCount > 0) {
            yield return new WaitForSeconds(gatherDuration);
            rUnit.RemoveResources(Random.Range(gatherCountMin, gatherCountMax));
            StartCoroutine(GatherResources());
        }
    }
}
