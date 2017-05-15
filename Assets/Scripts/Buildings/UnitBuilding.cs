using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBuilding : Building {
    [SerializeField] private float currentBuildTime = 0.0f;
    [SerializeField] private float maxBuildTime = 0.0f;
    [SerializeField] private List<GameObject> productionUnits = new List<GameObject>();
    [SerializeField] private Transform unitRallyPoint = null;
    [SerializeField] private Queue<GameObject> buildQueue = new Queue<GameObject>();

    public Queue<GameObject> BuildQueue {get{return buildQueue;} set{buildQueue = value;}}

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (IsPlaced && buildQueue.Count > 0) {
            ProcessBuildQueue();
        }
        if (IsPlaced) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                buildQueue.Enqueue(productionUnits[0]);
            }
        }
    }

    void ProcessBuildQueue() {
        if (BuildQueue.Count > 0) {
            currentBuildTime += Time.deltaTime;
            if (currentBuildTime >= maxBuildTime) {
                NavMeshHit hit;
                NavMesh.SamplePosition(unitRallyPoint.position, out hit, 50.0f, NavMesh.AllAreas);
                Instantiate(BuildQueue.Dequeue(), hit.position, unitRallyPoint.rotation);
                currentBuildTime = 0.0f;
            }
        }
    }
}