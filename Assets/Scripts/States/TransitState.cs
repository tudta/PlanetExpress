using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitState : UnitState {
    private Vector3 _tgtPos;

    public TransitState(Vector3 pos) {
        _tgtPos = pos;
    }

    public override void EnterState() {
        _Unit.MoveTo(_tgtPos);
        _Unit.OnPathEndReached += ExitState;
        base.EnterState();
    }

    public override void UpdateState() {
        _Unit.HandlePathing();
        base.UpdateState();
    }

    public override void ExitState() {
        _Unit._StateMachine.ChangeState(new IdleState());
        base.ExitState();
    }
}
