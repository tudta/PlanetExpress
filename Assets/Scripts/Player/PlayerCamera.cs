using UnityEngine;
using System.Collections;
using System;

public class PlayerCamera : MonoBehaviour {
	[SerializeField] private float moveSpeed = 0.0f;
	[SerializeField] private float minZoomDistance = 0.0f;
	[SerializeField] private float maxZoomDistance = 0.0f;
    private float desiredZoomDist = 0.0f;
    private float zoomIncDist = 0.0f;
    [SerializeField] private int numOfZoomIncs = 0;
    private int currentZoomInc = 0;
    [SerializeField] private float minXRot = 0.0f;
	[SerializeField] private float maxXRot = 0.0f;
    [SerializeField] private float rotXSpeed = 0.0f;
    [SerializeField] private float rotYSpeed = 0.0f;
    private int leftScrollLimit = 0;
	private int rightScrollLimit = 0;
	private int topScrollLimit = 0;
	private int bottomScrollLimit = 0;
    [SerializeField] private Transform camTran = null;

	// Use this for initialization
	void Start () {
		SetScrollLimits();
        SetZoomIncrements();
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        UpdateHeight();
    }

    private void UpdateHeight() {
        desiredZoomDist = minZoomDistance + (zoomIncDist * currentZoomInc);
        RaycastHit hit;
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 500.0f, mask)) {
            transform.position = hit.point + new Vector3(0.0f, desiredZoomDist, 0.0f);
        }
    }

    private void CheckInput() {
        if (!Input.GetMouseButton(0)) {
            if (Input.mousePosition.x <= leftScrollLimit) {
                if (Input.GetMouseButton(2)) {
                    RotateCamera(Vector3.left);
                }
                else {
                    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                }
            }
            if (Input.mousePosition.x >= rightScrollLimit) {
                if (Input.GetMouseButton(2)) {
                    RotateCamera(Vector3.right);
                }
                else {
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
            }
            if (Input.mousePosition.y >= topScrollLimit) {
                if (Input.GetMouseButton(2)) {
                    RotateCamera(Vector3.up);
                }
                else {
                    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                }
            }
            if (Input.mousePosition.y <= bottomScrollLimit) {
                if (Input.GetMouseButton(2)) {
                    RotateCamera(Vector3.down);
                }
                else {
                    transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
                }
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
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f) {
            ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

	public void ZoomCamera(float axis) {
		if (axis > 0) {
			if (currentZoomInc > 0) {
                currentZoomInc--;
			}
		}
		else {
            if (currentZoomInc < numOfZoomIncs) {
                currentZoomInc++;
            }
		}
	}

    public void RotateCamera(Vector3 dir) {
        if (dir == Vector3.left) {
            transform.Rotate(Vector3.up, -rotYSpeed * Time.deltaTime, Space.Self);
        }
        else if (dir == Vector3.right) {
            transform.Rotate(Vector3.up, rotYSpeed * Time.deltaTime, Space.Self);
        }
        else if (dir == Vector3.up) {
            if (transform.rotation.x > minXRot) {
                camTran.Rotate(Vector3.left, rotXSpeed * Time.deltaTime, Space.Self);
            }
        }
        else {
            if (transform.rotation.x < maxXRot) {
                camTran.Rotate(Vector3.left, -rotXSpeed * Time.deltaTime, Space.Self);
            }
        }
    }

	private void SetScrollLimits() {
		leftScrollLimit = 0;
		rightScrollLimit = Screen.width - 1;
		topScrollLimit = Screen.height;
        bottomScrollLimit = 0;
	}

    private void SetZoomIncrements() {
        zoomIncDist = (maxZoomDistance - minZoomDistance) / numOfZoomIncs;
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
