﻿using UnityEngine;
using System.Collections;

public class OffensiveUnit : MonoBehaviour
{
    [SerializeField] private GameUnit gUnit = null;
    [SerializeField]private UnitStates currentState = UnitStates.IDLE;
    private UnitStates lastState = UnitStates.IDLE;
    [SerializeField] private float movespeed = 0.0f;
    [SerializeField] private int damage = 0;
    [SerializeField] private float attackRange = 0.0f;
    [SerializeField] private float projectileSpeed = 0.0f;
    [SerializeField] private float attackSpeed = 0.0f;
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private Transform firePoint = null;
    private bool canFire = true;
    private Transform target = null;
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private NavMeshObstacle obstacle;
    private bool isChangingAgent = false;
    [SerializeField] private float distThreshold = 0.0f;

    #region Properties
    public GameUnit GUnit {
        get {
            return gUnit;
        }

        set {
            gUnit = value;
        }
    }

    public float Movespeed {
        get {
            return movespeed;
        }

        set {
            movespeed = value;
        }
    }

    public int Damage {
        get {
            return damage;
        }

        set {
            damage = value;
        }
    }

    public float AttackRange {
        get {
            return attackRange;
        }

        set {
            attackRange = value;
        }
    }

    public float ProjectileSpeed {
        get {
            return projectileSpeed;
        }

        set {
            projectileSpeed = value;
        }
    }

    public float AttackSpeed {
        get {
            return attackSpeed;
        }

        set {
            attackSpeed = value;
        }
    }

    public Transform Target {
        get {
            return target;
        }

        set {
            target = value;
        }
    }
    #endregion

    void Awake() {
        agent.stoppingDistance = 0.0f;
        agent.avoidancePriority = Random.Range(0, 101);
        //StartCoroutine(ToggleAgent(UnitStates.IDLE));
    }

	// Use this for initialization
	void Start () {
        gUnit = GetComponent<GameUnit>();
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.Instance.CurrentState == GameStates.PLAY) {
            if (!isChangingAgent) {
                //UnitState AI
                switch (currentState) {
                    case UnitStates.IDLE:
                        ScanForEnemiesInVision();
                        break;
                    case UnitStates.TRANSIT:
                        CheckArrival();
                        break;
                    case UnitStates.ATTACK:
                        if (Target == null) {
                            ChangeState(UnitStates.IDLE);
                        }
                        else {
                            //Attack if in range
                            if (InAttackRange(Target)) {
                                StartCoroutine(Attack(Target));
                            }
                            else {
                                MoveTo(Target, UnitStates.ATTACK);
                            }
                        }
                        break;
                    case UnitStates.PATROL:
                        //Move from destination A to B
                        //Attack units in range
                        break;
                }
            }
        }
    }

    public void ChangeState(UnitStates state) {
        lastState = currentState;
        currentState = state;
    }

    private void CheckArrival() {
        if (agent.enabled && Vector3.Distance(transform.position, agent.destination) <= distThreshold) {
            agent.destination = transform.position;
            StartCoroutine(ToggleAgent(UnitStates.IDLE));
        }
    }

    /*private IEnumerator ToggleAgent() {
        //print("ToggleAgent no destination: START");
        isChangingAgent = true;
        if (agent.enabled) {
            agent.enabled = false;
            yield return new WaitForSeconds(0);
            obstacle.enabled = true;
        }
        else {
            obstacle.enabled = false;
            yield return new WaitForSeconds(0);
            agent.enabled = true;
        }
        isChangingAgent = false;
        //print("ToggleAgent no destination: END");
    }*/

    private IEnumerator ToggleAgent(UnitStates state) {
        isChangingAgent = true;
        if (agent.enabled) {
            agent.enabled = false;
            yield return new WaitForSeconds(0);
            obstacle.enabled = true;
        }
        else {
            obstacle.enabled = false;
            yield return new WaitForSeconds(0);
            agent.enabled = true;
        }
        ChangeState(state);
        isChangingAgent = false;
    }

    private IEnumerator ToggleAgent(Vector3 pos) {
        isChangingAgent = true;
        if (!agent.enabled) {
            obstacle.enabled = false;
            yield return new WaitForSeconds(0);
            agent.enabled = true;
            agent.destination = pos;
        }
        isChangingAgent = false;
    }

    private IEnumerator ToggleAgent(Vector3 pos, UnitStates state) {
        isChangingAgent = true;
        if (!agent.enabled) {
            obstacle.enabled = false;
            yield return new WaitForSeconds(0);
            agent.enabled = true;
            agent.destination = pos;
        }
        ChangeState(state);
        isChangingAgent = false;
    }

    public void MoveTo(Vector3 pos, UnitStates state) {
        if (!agent.enabled)
        {
            StartCoroutine(ToggleAgent(pos, state));
        }
        else {
            agent.destination = pos;
            ChangeState(state);
        }
    }

    public void MoveTo(Transform trans, UnitStates state) {
        if (!agent.enabled)
        {
            StartCoroutine(ToggleAgent(trans.position, state));
        }
        else {
            agent.destination = trans.position;
            ChangeState(state);
        }
    }

    public void ScanForEnemiesInVision() {
        GameUnit unit;
        Collider[] cols = Physics.OverlapSphere(transform.position, gUnit.VisionRadius);
        foreach (Collider col in cols) {
            unit = col.GetComponent<GameUnit>();
            if (unit != null && unit.GUnitType != GameUnitTypes.TERRAIN && unit.Team != gUnit.Team) {
                SetTarget(unit.transform);
                break;
            }
        }
    }

    public void ScanForEnemiesInAttackRange() {
        GameUnit unit;
        Collider[] cols = Physics.OverlapSphere(transform.position, AttackRange);
        foreach (Collider col in cols) {
            unit = col.GetComponent<GameUnit>();
            if (unit != null && unit.GUnitType != GameUnitTypes.TERRAIN && unit.Team != gUnit.Team) {
                SetTarget(unit.transform);
                break;
            }
        }
    }

    public bool InAttackRange(Transform t) {
        Vector3 dir = (t.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, dir, out hit, AttackRange)) {
            if (hit.transform == t) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }

    public void SetTarget(Transform t) {
        Target = t;
        if (!agent.enabled) {
            StartCoroutine(ToggleAgent(Target.position, UnitStates.ATTACK));
        }
        else {
            agent.destination = Target.position;
            ChangeState(UnitStates.ATTACK);
        }
    }

    public IEnumerator Attack(Transform tar) {
        if (agent.enabled) {
            StartCoroutine(ToggleAgent(UnitStates.ATTACK));
        }
        if (canFire) {
            canFire = false;
            transform.LookAt(tar);
            GameObject go = (GameObject)Instantiate(projectile, firePoint.position, firePoint.rotation);
            go.GetComponent<Projectile>().SetProjectileValues(gUnit.Team, ProjectileSpeed, Damage);
            yield return new WaitForSeconds(AttackSpeed);
            canFire = true;
        }
    }

    public void ApplyDamage(int damage) {
        gUnit.ApplyDamage(damage);
    }

    void OnDrawGizmos() {
        //Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        //Gizmos.DrawSphere(pos, visionRadius);
    }
}
