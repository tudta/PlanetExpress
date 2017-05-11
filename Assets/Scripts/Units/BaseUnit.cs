using UnityEngine;
using System.Collections;

public class BaseUnit : GameUnit
{
    [SerializeField]private UnitStates currentState = UnitStates.IDLE;
    private UnitStates lastState = UnitStates.IDLE;
    [SerializeField] private float movespeed = 0.0f;
    [SerializeField] private float visionRadius;
    [SerializeField] private int damage = 0;
    [SerializeField] private float attackRange = 0.0f;
    [SerializeField] private float projectileSpeed = 0.0f;
    [SerializeField] private float attackSpeed = 0.0f;
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private Transform firePoint = null;
    private bool canFire = true;
    private Transform target = null;
    private bool isSelected = false;
    [SerializeField] private MeshRenderer ren;
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private NavMeshObstacle obstacle;
    private bool isChangingAgent = false;
    [SerializeField] private float distThreshold = 0.0f;

    public override void Awake() {
        agent.stoppingDistance = 0.0f;
        StartCoroutine(ToggleAgent(UnitStates.IDLE));
    }

	// Use this for initialization
	public override void Start () {
        
    }

    // Update is called once per frame
    public override void Update() {
        if (GameManager.Instance.CurrentState == GameStates.PLAY) {
            //Check if player is selecting unit
            if (ren.isVisible && Input.GetMouseButton(0)) {
                Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
                camPos.y = Player.InvertMouseY(camPos.y);
                if (Player.Selection.Contains(camPos)) {
                    SelectUnit();
                }
                else {
                    UnselectUnit();
                }
            }
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
                        if (target == null) {
                            ChangeState(UnitStates.IDLE);
                        }
                        else {
                            //Attack if in range
                            if (InAttackRange(target)) {
                                StartCoroutine(Attack(target));
                            }
                            else {
                                MoveTo(target, UnitStates.ATTACK);
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

    public void SelectUnit() {
        if (!Player.Instance.SelectedUnits.Contains(this)) {
            Player.Instance.SelectedUnits.Add(this);
        }
        isSelected = true;
        ren.material.color = Color.green;
    }

    public void UnselectUnit() {
        if (Player.Instance.SelectedUnits.Contains(this)) {
            Player.Instance.SelectedUnits.Remove(this);
        }
        isSelected = false;
        ren.material.color = Color.white;
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
        GameUnit gUnit;
        Collider[] cols = Physics.OverlapSphere(transform.position, visionRadius);
        foreach (Collider col in cols) {
            gUnit = col.GetComponent<GameUnit>();
            if (gUnit != null && gUnit.GUnitType != GameUnitTypes.TERRAIN && gUnit.Team != this.Team) {
                SetTarget(gUnit.transform);
                break;
            }
        }
    }

    public void ScanForEnemiesInAttackRange() {
        GameUnit gUnit;
        Collider[] cols = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider col in cols) {
            gUnit = GetComponent<GameUnit>();
            if (gUnit != null && gUnit.GUnitType != GameUnitTypes.TERRAIN && gUnit.Team != this.Team) {
                SetTarget(gUnit.transform);
                break;
            }
        }
    }

    public bool InAttackRange(Transform t) {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        Vector3 dir = (t.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, attackRange)) {
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
        if (!agent.enabled) {
            StartCoroutine(ToggleAgent(target.position, UnitStates.ATTACK));
        }
        else {
            agent.destination = target.position;
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
            go.GetComponent<Projectile>().SetProjectileValues(Team, projectileSpeed, damage);
            yield return new WaitForSeconds(attackSpeed);
            canFire = true;
        }
    }

    public override void ApplyDamage(int damage) {
        base.ApplyDamage(damage);
    }

    void OnDrawGizmos() {
        //Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        //Gizmos.DrawSphere(pos, visionRadius);
    }
}
