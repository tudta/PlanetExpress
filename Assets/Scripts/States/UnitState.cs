using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitState {
    private NewUnit _unit;

    public NewUnit _Unit {get {return _unit;} set {_unit = value;}}

    public delegate void StateTransition();
    public StateTransition OnStateEnter;
    public StateTransition OnStateUpdate;
    public StateTransition OnStateExit;

    public virtual void EnterState() {
        Debug.Log("Entered " + GetType().ToString());
    }

    public virtual void UpdateState() {
        Debug.Log("Updated " + GetType().ToString());
    }

    public virtual void ExitState() {
        Debug.Log("Exited " + GetType().ToString());
    }
}