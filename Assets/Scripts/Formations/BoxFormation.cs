using UnityEngine;
using System.Collections;

public class BoxFormation : Formation {

    public BoxFormation() : base(27, FormationType.BOX) {

    }

    public override void AssignPositions()
    {
        Positions.Add(new Vector3(0.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(0.0f, 0.0f, -2.0f));
        Positions.Add(new Vector3(0.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(-2.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(2.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-2.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(2.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(-2.0f, 0.0f, -2.0f));
        Positions.Add(new Vector3(2.0f, 0.0f, -2.0f));
        Positions.Add(new Vector3(-4.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(4.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-4.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(4.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(-4.0f, 0.0f, -2.0f));
        Positions.Add(new Vector3(4.0f, 0.0f, -2.0f));
        Positions.Add(new Vector3(-6.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(6.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-6.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(6.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(-6.0f, 0.0f, -2.0f));
        Positions.Add(new Vector3(6.0f, 0.0f, -2.0f));
        Positions.Add(new Vector3(-8.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(8.0f, 0.0f, 0.0f));
        Positions.Add(new Vector3(-8.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(8.0f, 0.0f, 2.0f));
        Positions.Add(new Vector3(-8.0f, 0.0f, -2.0f));
        Positions.Add(new Vector3(8.0f, 0.0f, -2.0f));
    }
}
