using UnityEngine;
using System.Collections;

public class LineFormation : Formation {

    public LineFormation() : base(4, FormationType.LINE) {

    }
    public override void AssignPositions() {
        Positions[0] = new Vector3(0.0f, 0.0f, 0.0f);
        Positions[1] = new Vector3(2.0f, 0.0f, 0.0f);
        Positions[2] = new Vector3(-2.0f, 0.0f, 0.0f);
        Positions[3] = new Vector3(4.0f, 0.0f, 0.0f);
        Positions[4] = new Vector3(-4.0f, 0.0f, 0.0f);
        Positions[5] = new Vector3(6.0f, 0.0f, 0.0f);
    }
}
