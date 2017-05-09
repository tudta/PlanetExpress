using UnityEngine;
using System.Collections;

public class ZipperFormation : Formation {

    public ZipperFormation() : base(6, FormationType.ZIPPER) {

    }

    public override void AssignPositions()
    {
        Positions.Add(new Vector3(0.0f, 0.0f, 1.0f));
        Positions.Add(new Vector3(2.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-2.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(4.0f, 0.0f, 1.0f));
        Positions.Add(new Vector3(-4.0f, 0.0f, 1.0f));
        Positions.Add(new Vector3(6.0f, 0.0f, 0.0f));
    }
}
