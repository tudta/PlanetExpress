using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour
{
    public enum UnitState {IDLE, TRANSIT, ATTACK, PATROL};

    [SerializeField] private float movespeed = 0.0f;
    [SerializeField] private int health = 0;
    [SerializeField] private int damage = 0;
    private bool isSelected = false;
    [SerializeField] private MeshRenderer ren;
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private NavMeshObstacle obstacle;
    [SerializeField] private float distThreshold = 0.0f;
    private UnitState currentState;

	// Use this for initialization
	void Start () {
        ToggleAgent();
    }

    // Update is called once per frame
    void Update() {
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
        CheckArrival();
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
            ToggleAgent();
        }
    }

    private void ToggleAgent() {
        if (agent.enabled) {
            agent.enabled = false;
            obstacle.enabled = true;
        }
        else {
            obstacle.enabled = false;
            agent.enabled = true;
        }
    }

    private IEnumerator ToggleAgent(Vector3 pos) {
        if (!agent.enabled) {
            obstacle.enabled = false;
            yield return new WaitForSeconds(0);
            agent.enabled = true;
            agent.destination = pos;
        }
    }

    public void MoveTo(Vector3 pos) {
        if (!agent.enabled)
        {
            StartCoroutine(ToggleAgent(pos));
        }
        else {
            agent.destination = pos;
        }
        currentState = UnitState.TRANSIT;
    }

    public void MoveTo(Transform trans) {
        if (!agent.enabled)
        {
            StartCoroutine(ToggleAgent(trans.position));
        }
        else {
            agent.destination = trans.position;
        }
        currentState = UnitState.TRANSIT;
        agent.destination = trans.position;
    }
}
