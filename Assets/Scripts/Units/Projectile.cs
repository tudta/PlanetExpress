using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    private int team = 0;
    private float speed = 0.0f;
    private int damage = 0;
    private Vector3 lastPos;
    private Rigidbody rB;

    public int Team {get{return team;} set{team = value;}}
    public float Speed {get{return speed;} set{speed = value;}}
    public int Damage {get{return damage;} set{damage = value;}}

    void Awake() {
        rB = GetComponent<Rigidbody>();
        lastPos = transform.position;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {
        CheckForHit();
    }

    private void CheckForHit() {
        Ray ray = new Ray(lastPos, transform.position - lastPos);
        float dist = Vector3.Distance(transform.position, lastPos);
        GameUnit hitUnit;
        RaycastHit[] hits = Physics.RaycastAll(ray, dist);
        foreach (RaycastHit hit in hits) {
            hitUnit = hit.transform.GetComponent<GameUnit>();
            if (hitUnit != null) {
                if (hitUnit.GUnitType == GameUnitTypes.TERRAIN) {
                    Destroy(gameObject);
                }
                else if (hitUnit.Team != Team) {
                    hitUnit.ApplyDamage(damage);
                    Destroy(gameObject);
                    break;
                }
            }
        }
        lastPos = transform.position;
    }

    public void SetProjectileValues(int teamNum, float speedVal, int damageVal) {
        Team = teamNum;
        damage = damageVal;
        rB.velocity = transform.forward * speedVal;
    }
}
