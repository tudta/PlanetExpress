using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : UnitState {
    private Vector3 _tgtPos;
    private GameUnit _tgtUnit;
    private GameUnit _tmpUnit;

    public AttackState(Vector3 pos) {
        _tgtPos = pos;
    }

    public AttackState(GameUnit unit) {
        _tgtUnit = unit;
    }

    public override void EnterState() {
        base.EnterState();
    }

	public override void UpdateState() {
        //ATTACK TARGET MODE
        if (_tgtUnit != null) {
            if (Vector3.Distance(_Unit.transform.position, _tgtUnit.transform.position) <= _Unit._AttackRange) {
                _Unit.Attack();
            }
            else {
                _Unit.MoveTo(_tgtUnit.transform.position);
                _Unit.HandlePathing();
            }
        }
        //ATTACK MOVE MODE
        else if (_tgtPos != null) {
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
