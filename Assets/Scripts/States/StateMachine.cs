using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {
    private UnitState _currentState;
    private UnitState _previousState;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_currentState != null) {
            _currentState.UpdateState();
        }
	}

    public void ChangeState(UnitState state) {
        if (_currentState != null) {
            _currentState.ExitState();
            _previousState = _currentState;
        }
        _currentState = state;
        _currentState.EnterState();
    }
}
