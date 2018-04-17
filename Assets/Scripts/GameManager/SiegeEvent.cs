using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SiegeEvent : RandomEvent {
    private GameObject bossUnit = null;
    private GameObject mobUnit = null;
    private List<GameObject> spawnedUnits = new List<GameObject>();
    private int groupSize = 0;
    private Formation groupFormation = null;
    private UnityEngine.AI.NavMeshPath path = null;

    public SiegeEvent(GameObject boss, GameObject mob, int maxGroupSize, Formation form) {
        bossUnit = boss;
        mobUnit = mob;
        groupSize = Random.Range(2, maxGroupSize);
        groupFormation = form;
        path = new UnityEngine.AI.NavMeshPath();
    }

    public override void EventStart() {
        Debug.Log("Event started!");
        //Get random spawn waypoint
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject[] comCenObjs = GameObject.FindGameObjectsWithTag("Village");
        List<UnitBuilding> comCens = new List<UnitBuilding>();
        for (int i = 0; i < comCenObjs.Length; i++) {
            comCens.Add(comCenObjs[i].GetComponent<UnitBuilding>());
        }
        UnitBuilding comCen = comCens[Random.Range(0, comCens.Count)];
        UnityEngine.AI.NavMeshHit mainHit;
        UnityEngine.AI.NavMeshHit tmpHit;
        UnityEngine.AI.NavMeshHit comHit;
        UnityEngine.AI.NavMesh.SamplePosition(spawnPoint.transform.position, out mainHit, 20.0f, UnityEngine.AI.NavMesh.AllAreas);
        UnityEngine.AI.NavMesh.SamplePosition(comCen.transform.position, out comHit, 20.0f, UnityEngine.AI.NavMesh.AllAreas);
        //Handle position stuff with formation
        if (groupSize > 0) {
            UnityEngine.AI.NavMesh.SamplePosition(mainHit.position + groupFormation.Positions[0], out tmpHit, 100.0f, UnityEngine.AI.NavMesh.AllAreas);
            spawnedUnits.Add((GameObject)Object.Instantiate(bossUnit, tmpHit.position + groupFormation.Positions[0], spawnPoint.transform.rotation));
            spawnedUnits[0].GetComponent<OffensiveUnit>().GUnit.Team = 1;
            spawnedUnits[0].GetComponent<OffensiveUnit>().Agent.speed *= 6.0f;
            spawnedUnits[0].GetComponent<OffensiveUnit>().MoveTo(comHit.position, UnitStates.ATTACK);
            for (int i = 1; i < groupSize; i++) {
                UnityEngine.AI.NavMesh.SamplePosition(mainHit.position + groupFormation.Positions[i], out tmpHit, 100.0f, UnityEngine.AI.NavMesh.AllAreas);
                spawnedUnits.Add((GameObject)Object.Instantiate(mobUnit, tmpHit.position + groupFormation.Positions[i], spawnPoint.transform.rotation));
                spawnedUnits[i].GetComponent<OffensiveUnit>().GUnit.Team = 1;
                spawnedUnits[i].GetComponent<OffensiveUnit>().Agent.speed *= 6.0f;
                spawnedUnits[i].GetComponent<OffensiveUnit>().MoveTo(comHit.position, UnitStates.ATTACK);
            }
        }        
    }

    public override void EventUpdate() {
        if (spawnedUnits.Count == 0) {
            EventEnd();
        }
    }

    public override void EventEnd() {
        isOver = true;
    }
}
