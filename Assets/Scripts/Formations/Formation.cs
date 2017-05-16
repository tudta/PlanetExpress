using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Formation {
    private FormationType type;
    private int maxUnitCount;
    private List<Vector3> positions = new List<Vector3>();

    #region Properties
    public FormationType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public int MaxUnitCount
    {
        get
        {
            return maxUnitCount;
        }

        set
        {
            maxUnitCount = value;
        }
    }

    public List<Vector3> Positions
    {
        get
        {
            return positions;
        }

        set
        {
            positions = value;
        }
    }
    #endregion

    public Formation(int maxUnits, FormationType formType) {
        MaxUnitCount = maxUnits;
        Type = formType;
        AssignPositions();
    }

    public virtual void AssignPositions() {

    }

    public void CalculateCenter() {

    }
}
