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
    private UnitState currentState;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        if (isSelected) {
            ren.material.color = Color.green;
            if (Input.GetMouseButtonDown(1)) {
                //print ("Right-Clicked!");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                if (Physics.Raycast(ray, out hit, 200, mask)) {
                    agent.destination = hit.point;
                    currentState = UnitState.TRANSIT;
                }
            }
        }
        else {
            ren.material.color = Color.white;
        }
        if (ren.isVisible && Input.GetMouseButton(0)) {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = Player.InvertMouseY(camPos.y);
            isSelected = Player.Selection.Contains(camPos);
            //UI_Manager.instance.SelectUnits();
        }
    }

    private void MoveTo(Vector3 pos) {
        agent.destination = pos;
    }

    private void MoveTo(Transform trans) {
        agent.destination = trans.position;
    }
}
