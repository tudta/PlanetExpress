using UnityEngine;
using System.Collections;

public class LineFormation : Formation {

    public LineFormation() : base(6, FormationType.LINE) {

    }

    public override void AssignPositions() {
        Positions.Add(new Vector3(1.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(3.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-3.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(5.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-5.0f, 0.0f, 0.0f));
    }
}
