using UnityEngine;
using System.Collections;

public class SiegeEvent : RandomEvent {
    private GameObject bossUnit = null;
    private GameObject mobUnit = null;
    private int groupSize = 0;
    private Formation groupFormation = null;

    public override void EventStart() {
        if (groupSize > 0) {
            Object.Instantiate(bossUnit);
            for (int i = 1; i < groupSize; i++) {
                Object.Instantiate(mobUnit);
            }
        }
        base.EventStart();
    }

    public override void EventUpdate() {
        base.EventUpdate();
    }

    public override void EventEnd() {
        base.EventEnd();
    }
}
