using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Formation {
    public enum FormationType {LINE, BOX, ZIPPER}
    private FormationType type;
    private int maxUnitCount;
    private List<Vector3> positions = new List<Vector3>();

    #region Properties
    public List<Vector3> Positions {
        get {
            return positions;
        }

        set {
            positions = value;
        }
    }
    #endregion

    public Formation(int maxUnits, FormationType formType) {
        maxUnitCount = maxUnits;
        type = formType;
    }

    public virtual void AssignPositions() {

    }

    public void CalculateCenter() {

    }
}
