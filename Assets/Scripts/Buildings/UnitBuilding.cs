using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBuilding : Building {
    [SerializeField] private float currentBuildTime = 0.0f;
    [SerializeField] private float maxBuildTime = 0.0f;
    [SerializeField] private List<GameObject> productionUnits = new List<GameObject>();
    [SerializeField] private Transform unitRallyPoint = null;
    [SerializeField] private Queue<GameObject> buildQueue = new Queue<GameObject>();

    public List<GameObject> ProductionUnits {get{return productionUnits;} set{productionUnits = value;}}
    public Transform UnitRallyPoint {get{return unitRallyPoint;} set{unitRallyPoint = value;}}
    public Queue<GameObject> BuildQueue {get{return buildQueue;} set{buildQueue = value;}}

    // Use this for initialization
    public override void Start () {

	}
	
	// Update is called once per frame
	public override void Update () {
        if (IsPlaced && buildQueue.Count > 0) {
            ProcessBuildQueue();
        }
        if (IsPlaced) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                buildQueue.Enqueue(ProductionUnits[0]);
            }
        }
    }

    void ProcessBuildQueue() {
        if (BuildQueue.Count > 0) {
            currentBuildTime += Time.deltaTime;
            if (currentBuildTime >= maxBuildTime) {
                NavMeshHit hit;
                NavMesh.SamplePosition(UnitRallyPoint.position, out hit, 50.0f, NavMesh.AllAreas);
                Instantiate(BuildQueue.Dequeue(), hit.position, UnitRallyPoint.rotation);
                currentBuildTime = 0.0f;
            }
        }
    }
}