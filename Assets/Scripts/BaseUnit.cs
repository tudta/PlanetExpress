using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] private float movespeed = 0.0f;
    [SerializeField] private int health = 0;
    [SerializeField] private int damage = 0;
    [SerializeField] private NavMeshAgent agent = null;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void MoveTo(Vector3 pos)
    {
        agent.destination = pos;
    }

    private void MoveTo(Transform trans)
    {
        agent.destination = trans.position;
    }
}
