using UnityEngine;
using System.Collections;

public class LineFormation : Formation {

    public LineFormation() : base(27, FormationType.LINE) {

    }

    public override void AssignPositions() {
        Positions.Add(new Vector3(0.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(2.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-2.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(4.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-4.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(6.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-6.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(8.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-8.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(10.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-10.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(12.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-12.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(14.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-14.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(16.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-16.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(18.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-18.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(20.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-20.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(22.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-22.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(24.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-24.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(26.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-26.0f, 0.0f, 0.0f));
    }
}
