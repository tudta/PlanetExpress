using UnityEngine;
using System.Collections;

public abstract class RandomEvent {
    private string name;
    private string desc;
    private float eventDuration = 0.0f;
    private float eventTimer = 0.0f;

    public string Name {
        get {
            return name;
        }

        set {
            name = value;
        }
    }

    public string Desc {
        get {
            return desc;
        }

        set {
            desc = value;
        }
    }

    public float EventDuration {
        get {
            return eventDuration;
        }

        set {
            eventDuration = value;
        }
    }

    public float EventTimer {
        get {
            return eventTimer;
        }

        set {
            eventTimer = value;
        }
    }

    public virtual void EventStart() {
        Debug.LogError("Abstract RandomEvent Start!");
    }

    public virtual void EventUpdate() {
        if (GameManager.Instance.CurrentState != GameStates.PAUSE) {
            eventTimer += Time.deltaTime;
            if (eventTimer >= eventDuration) {
                EventEnd();
            }
        }
    }

    public virtual void EventEnd() {
        Debug.LogError("Abstract RandomEvent End!");
    }
}
