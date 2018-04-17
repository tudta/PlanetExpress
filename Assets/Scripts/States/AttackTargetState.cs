using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetState : UnitState {
    private GameUnit _tgtUnit;

    public AttackTargetState(GameUnit unit) {
        _tgtUnit = unit;
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        //ATTACK TARGET MODE
        if (_tgtUnit == null) {
            ExitState();
        }
        else {
            if (Vector3.Distance(_Unit.transform.position, _tgtUnit.transform.position) <= _Unit._AttackRange) {
                _Unit.Attack();
            }
            else {
                _Unit.MoveTo(_tgtUnit.transform.position);
                _Unit.HandlePathing();
            }
        }
        base.UpdateState();
    }

    public override void ExitState() {
        _Unit._StateMachine.ChangeState(new IdleState());
        base.ExitState();
    }
}
