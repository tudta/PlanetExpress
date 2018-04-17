using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NewUnit : MonoBehaviour {
    private Seeker _seeker = null;
    private Path _path = null;
    private int _currentWaypoint = 0;
    private float _waypointDistThreshold = 0.0f;
    [SerializeField] private float _movespeed = 0.0f;

    private StateMachine _stateMachine;
    private UnitStates _currentState;

    public delegate void PathingTransition();
    public event PathingTransition OnPathEndReached;

    public StateMachine _StateMachine {get {return _stateMachine;} set {_stateMachine = value;}}

    // Use this for initializationq
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*
        switch (_currentState) {
            case UnitStates.IDLE:
            break;
            case UnitStates.TRANSIT:
            HandlePathing();
            break;
            case UnitStates.ATTACK:
                 //ATTACK TARGET MODE
                 if (_attackTarget != null) {
                     if (Vector3.Distance(transform.position, _attackTarget.position) <= _attackRange) {
                        Attack();
                     }
                     else {
                        //StartPath(transform.position, _attackTarget.position);
                        HandlePathing();
                     }
                 }
                 //ATTACK MOVE MODE
                 else if (_attackPos != null) {
                    ScanAttackRange();
                    ScanVisionRange();
                    //StartPath(transform.position, _attackPos);
                    HandlePathing();
                 }
            break;
        }
        */
	}

    void OnPathComplete(Path p) {

    }

    public void HandlePathing() {
        //Check for path
        if (_path != null) {
            //Check if at end of path
            if (_currentWaypoint < _path.vectorPath.Count) {
                //Check if close enough to waypoint
                if (Vector3.Distance(transform.position, _path.vectorPath[_currentWaypoint]) <= _waypointDistThreshold) {
                    _currentWaypoint++;
                }
                //Move towards waypoint
                else {
                    Vector3 dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
                    transform.position += dir * _movespeed;
                }
            }
            else {
                if (OnPathEndReached != null) {
                    OnPathEndReached();
                }
                //ChangeState(UnitState.IDLE);
            }
        }
    }

    public void MoveTo(Vector3 pos) {
        _seeker.StartPath(transform.position, pos, OnPathComplete);
    }

    public void MoveTo(Transform tgtTrans) {
        _seeker.StartPath(transform.position, tgtTrans.position, OnPathComplete);
    }

    public void MoveTo(GameObject tgtObj) {
        _seeker.StartPath(transform.position, tgtObj.transform.position, OnPathComplete);
    }
}
