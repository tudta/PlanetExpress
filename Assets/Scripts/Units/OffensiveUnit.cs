using UnityEngine;
using Pathfinding;
using System.Collections;

public class OffensiveUnit : MonoBehaviour {
    [SerializeField] private GameUnit gUnit = null;
    [SerializeField] private UnitStates currentState = UnitStates.IDLE;
    private UnitStates lastState = UnitStates.IDLE;

    //Combat Stats
    [SerializeField] private float _movespeed = 0.0f;
    [SerializeField] private int damage = 0;
    [SerializeField] private float attackRange = 0.0f;
    [SerializeField] private float projectileSpeed = 0.0f;
    [SerializeField] private float attackSpeed = 0.0f;
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private Transform firePoint = null;
    private bool canFire = true;
    private Transform target = null;
    private Vector3 ogAttackDest = Vector3.zero;

    private Seeker _seeker = null;
    private Path _path = null;
    private int _currentWaypoint = 0;
    private float _waypointDistThreshold = 0.0f;

    #region Properties
    public GameUnit GUnit {
        get {
            return gUnit;
        }

        set {
            gUnit = value;
        }
    }

    public float _Movespeed {
        get {
            return _movespeed;
        }

        set {
            _movespeed = value;
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

    public Seeker _Seeker {
        get {
            return _seeker;
        }

        set {
            _seeker = value;
        }
    }

    public Vector3 OgAttackDest {
        get {
            return ogAttackDest;
        }

        set {
            ogAttackDest = value;
        }
    }
    #endregion

    void Awake() {

    }

    // Use this for initialization
    void Start() {
        gUnit = GetComponent<GameUnit>();
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.Instance.CurrentState == GameStates.PLAY) {
            //UnitState AI
            switch (currentState) {
                case UnitStates.IDLE:
                ScanForEnemiesInVision();
                break;
                case UnitStates.TRANSIT:

                //MOVE TOWARDS END OF PATH
                /*OLD METHOD
                if (transform.position == _seeker.destination) {
                    ChangeState(UnitStates.IDLE);
                }
                CheckArrival();*/

                //Check if a path exists and has not reached the end
                if (_path != null && _currentWaypoint < _path.vectorPath.Count) {
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
                    ChangeState(UnitStates.IDLE);
                }
                break;
                case UnitStates.ATTACK:
                if (Target == null) {
                    //Check 
                    else {
                        //SCAN FOR ENEMIES OR MOVE TO ATTACKING POSITION
                        ScanForEnemiesInVision();
                        MoveTo(OgAttackDest, UnitStates.ATTACK);
                    }
                }
                else {
                    if (InAttackRange(Target)) {
                        StartCoroutine(Attack(Target));
                    }
                    else {
                        //MOVE TO ATTACK TARGET
                        MoveTo(Target, UnitStates.ATTACK);
                    }
                }
                break;
                case UnitStates.PATROL:
                //Move from destination A to B
                //Attack units in range
                break;
                case UnitStates.DO_NOTHING:
                break;
            }
        }
    }

    public void ChangeState(UnitStates state) {
        lastState = currentState;
        if (state != UnitStates.ATTACK) {
            target = null;
        }
        else {
            if (target != null) {
                OgAttackDest = target.position;
            }
            else {
                OgAttackDest = _seeker.destination;
            }
        }
        currentState = state;
    }

    private void CheckArrival() {
        if (_seeker.enabled && Vector3.Distance(transform.position, _seeker.destination) <= distThreshold) {
            _seeker.destination = transform.position;
            StartCoroutine(ToggleAgent(UnitStates.IDLE));
        }
    }

    public void MoveTo(Vector3 pos, UnitStates state) {
        if (!_seeker.enabled) {
            StartCoroutine(ToggleAgent(pos, state));
        }
        else {
            _seeker.destination = pos;
            ogAttackDest = pos;
            ChangeState(state);
        }
    }

    public void MoveTo(Transform trans, UnitStates state) {
        if (!_seeker.enabled) {
            StartCoroutine(ToggleAgent(trans.position, state));
        }
        else {
            _seeker.destination = trans.position;
            ogAttackDest = trans.position;
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
        Collider[] cols = Physics.OverlapSphere(transform.position, attackRange);
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
        if (Physics.Raycast(firePoint.position, dir, out hit, attackRange)) {
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
        target = t;
        if (!_seeker.enabled) {
            StartCoroutine(ToggleAgent(target.position, UnitStates.ATTACK));
        }
        else {
            _seeker.destination = target.position;
            ChangeState(UnitStates.ATTACK);
        }
    }

    public IEnumerator Attack(Transform tar) {
        if (_seeker.enabled) {
            StartCoroutine(ToggleAgent(UnitStates.ATTACK));
        }
        if (canFire) {
            canFire = false;
            transform.LookAt(tar);
            GameObject go = (GameObject)Instantiate(projectile, firePoint.position, firePoint.rotation);
            go.GetComponent<Projectile>().SetProjectileValues(gUnit.Team, projectileSpeed, damage);
            Camera.main.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.shootSound);
            yield return new WaitForSeconds(attackSpeed);
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
