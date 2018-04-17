using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : UnitState {
    private List<Vector3> _positions;
    private int waypointIndex = 0;

    public PatrolState(Vector3 currentPos, Vector3 tgtPos) {
        _positions.Add(currentPos);
        _positions.Add(tgtPos);
    }

    public PatrolState(Vector3 currentPos, List<Vector3> tgtPositions) {
        _positions.AddRange(tgtPositions);
    }
    
    public override void EnterState() {
        _Unit.OnPathEndReached += SetPatrolDestination;
        SetPatrolDestination();
        base.EnterState();
    }

    public override void UpdateState() {
        _Unit.HandlePathing();
        base.UpdateState();
    }

    public override void ExitState() {
        base.ExitState();
    }

    private void SetPatrolDestination() {
        if (waypointIndex >= _positions.Count) {
            waypointIndex = 0;
        }
        _Unit.MoveTo(_positions[waypointIndex]);
        waypointIndex++;
    }
}