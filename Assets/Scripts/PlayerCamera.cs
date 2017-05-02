using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	[SerializeField] private float moveSpeed = 0.0f;
	[SerializeField] private float minZoomDistance;
	[SerializeField] private float maxZoomDistance;
	private int leftScrollLimit;
	private int rightScrollLimit;
	private int topScrollLimit;
	private int bottomScrollLimit;
    [SerializeField] private Transform camTran;

	// Use this for initialization
	void Start () {
		SetScrollLimits();
	}
	
	// Update is called once per frame
	void Update () {
        if (!Input.GetMouseButton(0)) {
            if (Input.mousePosition.x <= leftScrollLimit) {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
            if (Input.mousePosition.x >= rightScrollLimit) {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            if (Input.mousePosition.y >= topScrollLimit) {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            if (Input.mousePosition.y <= bottomScrollLimit) {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.UpArrow)) {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow)) {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
        }
    }

	public void ZoomCamera(float axis) {
		if (axis > 0) {
			if (camTran.position.y > minZoomDistance) {
				camTran.Translate(Vector3.forward);
			}
		}
		else {
            if (camTran.position.y < maxZoomDistance) {
                camTran.Translate(Vector3.back);
			}
		}
	}

	private void SetScrollLimits() {
		leftScrollLimit = 0;
		rightScrollLimit = Screen.width;
		topScrollLimit = Screen.height;
        bottomScrollLimit = 0;
	}
    
    /*
    //USE TO SHOW CAMERA MOVE BORDERS
    private void OnGUI()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(1, 1, Color.red);
        tex.wrapMode = TextureWrapMode.Repeat;
        tex.Apply();
        GUI.Label(new Rect(0, 0, leftScrollLimit, Screen.height), tex);
        GUI.skin.box.normal.background = tex;
        //Draw Left
        GUI.Box(new Rect(0, 0, leftScrollLimit, Screen.height), GUIContent.none);
        //Draw Right
        GUI.Box(new Rect(rightScrollLimit, 0, leftScrollLimit, Screen.height), GUIContent.none);
        //Draw Top
        GUI.Box(new Rect(0, topScrollLimit, Screen.width, bottomScrollLimit), GUIContent.none);
        //Draw Bot
        GUI.Box(new Rect(0, 0, Screen.width, bottomScrollLimit), GUIContent.none);
    }
    */
}
