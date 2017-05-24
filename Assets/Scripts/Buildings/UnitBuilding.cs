using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBuilding : Building {
    [SerializeField] private float currentBuildTime = 0.0f;
    [SerializeField] private float maxBuildTime = 0.0f;
    [SerializeField] private List<OffensiveUnit> productionUnits = new List<OffensiveUnit>();
    [SerializeField] private Transform unitSpawnPoint = null;
    [SerializeField] private Vector3 unitRallyPoint = Vector3.zero;
    [SerializeField] private Queue<OffensiveUnit> buildQueue = new Queue<OffensiveUnit>();

    public List<OffensiveUnit> ProductionUnits {get{return productionUnits;} set{productionUnits = value;}}
    public Transform UnitSpawnPoint{get{return unitSpawnPoint;} set{unitSpawnPoint = value;}}
    public Vector3 UnitRallyPoint {get{return unitRallyPoint;} set{unitRallyPoint = value;}}
    public Queue<OffensiveUnit> BuildQueue {get{return buildQueue;} set{buildQueue = value;}}

    // Use this for initialization
    public override void Start () {
        unitRallyPoint = unitSpawnPoint.position;
	}
	
	// Update is called once per frame
	public override void Update () {
        if (IsPlaced && buildQueue.Count > 0) {
            ProcessBuildQueue();
        }
    }

    public void AddToBuildQueue(OffensiveUnit unit) {
        if (Player.Instance.CanAfford(unit.GUnit))
        {
            Player.Instance.PurchaseUnit(unit.GUnit);
            buildQueue.Enqueue(unit);
        }
    }

    void ProcessBuildQueue() {
        if (BuildQueue.Count > 0) {
            currentBuildTime += Time.deltaTime;
            if (currentBuildTime >= maxBuildTime) {
                NavMeshHit hit;
                NavMesh.SamplePosition(unitSpawnPoint.position, out hit, 50.0f, NavMesh.AllAreas);
                OffensiveUnit unit = buildQueue.Dequeue();
                unit = (OffensiveUnit)Instantiate(unit, hit.position, unitSpawnPoint.rotation);
                unit.MoveTo(unitRallyPoint, UnitStates.TRANSIT);
                currentBuildTime = 0.0f;
            }
        }
    }
}