using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [SerializeField] private GameStates currentState = GameStates.PLAY;
    private string buildingPath = "Prefabs/";
    private Player player = null;
    private float eventTimer = 0.0f;
    [SerializeField] private float eventInterval = 0.0f;
    [SerializeField] private List<GameObject> bossUnits = new List<GameObject>();
    [SerializeField] private List<GameObject> mobUnits = new List<GameObject>();
    private List<Formation> groupFormations = new List<Formation>();
    private List<SiegeEvent> currentEvents = new List<SiegeEvent>();
    private int eventsFired = 0;
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip clickSound;
    public AudioClip shootSound;
    public AudioClip orderSound;

    public static GameManager Instance {get{return instance;} set{instance = value;}}
    public GameStates CurrentState {get{return currentState;} set{currentState = value;}}
    
    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        player = Player.Instance;
        Init();
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            InitiateSiegeEvent();
        }
        PlayMusic();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentState == GameStates.PLAY) {
            eventTimer += Time.deltaTime;
            if (eventTimer >= eventInterval) {
                InitiateSiegeEvent();
                eventTimer = 0.0f;
                eventsFired++;
            }
            for (int i = 0; i < currentEvents.Count; i++) {
                if (!currentEvents[i].isOver) {
                    currentEvents[i].EventUpdate();
                }
            }
        }
	}

    private void Init() {
        groupFormations.Add(new LineFormation());
        groupFormations.Add(new ZipperFormation());
        groupFormations.Add(new BoxFormation());
    }

    private void PlayMusic() {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            Camera.main.GetComponent<AudioSource>().clip = menuMusic;
            Camera.main.GetComponent<AudioSource>().volume = 0.8f;
            Camera.main.GetComponent<AudioSource>().Play();
        }
        else {
            Camera.main.GetComponent<AudioSource>().clip = gameMusic;
            Camera.main.GetComponent<AudioSource>().volume = 0.4f;
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }

    public void ChangeLevel(int sceneNum) {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneNum);
    }

    public void CreateBuilding(string name) {
        //GameObject tmpGO = null;
        GameObject tmpGO = (GameObject)Resources.Load(buildingPath + name);
        Building tmpBuilding = tmpGO.GetComponent<Building>();
        if (player.CanAfford(tmpBuilding.GUnit)) {
            player.PurchaseUnit(tmpBuilding.GUnit);
            tmpGO = Instantiate(tmpGO, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            tmpBuilding = tmpGO.GetComponent<Building>();
            player.TarBuildingObj = tmpGO;
            player.TarBuilding = tmpBuilding;
            player.SwitchState("BUILD");
        }
    }

    public void InitiateSiegeEvent() {
        int groupSize = 3 + eventsFired;
        if (groupSize > 27) {
            groupSize = 27;
        }
        currentEvents.Add(new SiegeEvent(bossUnits[Random.Range(0, bossUnits.Count)], mobUnits[Random.Range(0, mobUnits.Count)], groupSize, groupFormations[Random.Range(0, groupFormations.Count)]));
        currentEvents[currentEvents.Count - 1].EventStart();
        //SiegeAttempt
        //Choose randomly from list of siege events
    }
}
