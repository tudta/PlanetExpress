﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private static Player instance = null;
    [SerializeField] private GameStates currentState = GameStates.DEFAULT;
    [SerializeField] private PlayerCamera cam;
    private List<GameObject> selectedUnits = new List<GameObject>();
    private Rect selection = new Rect(0, 0, 0, 0);
    private Texture2D selectionVisual;
    private Vector3 startClick = -Vector3.one;
    private GameObject buildingObj = null;

    #region Properties
    public static Player Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public GameObject BuildingObj
    {
        get
        {
            return buildingObj;
        }

        set
        {
            buildingObj = value;
        }
    }
    #endregion

    void Awake()
    {
        Instance = this;
        selectionVisual = Resources.Load("Images/GoldHighlight") as Texture2D;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        switch (currentState)
        {
            case GameStates.PLAY:
                break;
            case GameStates.BUILD:
                if (buildingObj != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    LayerMask mask = 1 << 8;
                    if (Physics.Raycast(ray, out hit, 100.0f, mask))
                    {
                        print(hit.point);
                        Vector3 tmpV3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, hit.distance);
                        tmpV3 = Camera.main.ScreenToWorldPoint(tmpV3);
                        tmpV3 = new Vector3(tmpV3.x, hit.point.y, tmpV3.z);
                        buildingObj.transform.position = tmpV3;
                    }
                }
                break;
        }
    }

    void CheckInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            cam.ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
        }
        if (Input.GetMouseButtonDown(0))
        {
            startClick = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startClick = -Vector3.one;
        }
        if (Input.GetMouseButton(0))
        {
            selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));
            if (selection.width < 0)
            {
                selection.x += selection.width;
                selection.width = -selection.width;
            }
            if (selection.height < 0)
            {
                selection.y += selection.height;
                selection.height = -selection.height;
            }
        }
        switch (currentState)
        {
            case GameStates.DEFAULT:
                break;
            case GameStates.PLAY:
                break;
            case GameStates.BUILD:
                if (Input.GetMouseButton(0))
                {
                    //Place building
                    buildingObj = null;
                    //Switch to play mode
                    SwitchState("PLAY");
                }
                if (Input.GetMouseButton(1))
                {
                    //Destroy building
                    Destroy(buildingObj);
                    //Switch to play mode
                    SwitchState("PLAY");
                }
                break;
            case GameStates.PAUSE:
                break;
        }
    }

    public void SwitchState(string stateName)
    {
        currentState = (GameStates)System.Enum.Parse(typeof(GameStates), stateName);
    }

    public static float InvertMouseY(float y)
    {
        return Screen.height - y;
    }

    private void OnGUI()
    {
        if (startClick != -Vector3.one)
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(selection, selectionVisual);
        }
    }
}   
