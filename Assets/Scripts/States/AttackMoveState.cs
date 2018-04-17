using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoveState : UnitState {
    private Vector3 _tgtPos;
    private GameUnit _tmpUnit;

    public AttackMoveState(Vector3 pos) {
        _tgtPos = pos;
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        //ATTACK MOVE MODE
        if (_tgtPos == null) {
            ExitState();
        }
        else {
            _Unit.ScanAttackRange();
            _Unit.ScanVisionRange();
            //If no enemies in attack/vision range perform below
            _Unit.MoveTo(_tgtPos);
            _Unit.HandlePathing();
        }
        base.UpdateState();
    }

    public override void ExitState() {
        _Unit._StateMachine.ChangeState(new IdleState());
        base.ExitState();
    }
}
