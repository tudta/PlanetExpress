using UnityEngine;
using System.Collections;

public class ResourceUnit : MonoBehaviour {
    [SerializeField] private ResourceType rType = ResourceType.NONE;
    [SerializeField] private int resourceCount = 0;

    public ResourceType RType {get{return rType;} set{rType = value;}}
    public int ResourceCount {get{return resourceCount;} set{resourceCount = value;}}

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int RemoveResources(int count) {
        int returnAmount;
        if (count > ResourceCount) {
            returnAmount = ResourceCount;
            DelayedDestroy(0);
        }
        else {
            returnAmount = count;
        }
        ResourceCount -= returnAmount;
        return returnAmount;
    }

    public IEnumerator DelayedDestroy(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
