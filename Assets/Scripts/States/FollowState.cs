using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : UnitState {
    private GameUnit _tgtUnit;

    public FollowState(GameUnit unit) {
        _tgtUnit = unit;
    }    

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        _Unit.MoveTo(_tgtUnit.transform.position);
        _Unit.HandlePathing();
        base.UpdateState();
    }

    public override void ExitState() {
        base.ExitState();
    }
}
