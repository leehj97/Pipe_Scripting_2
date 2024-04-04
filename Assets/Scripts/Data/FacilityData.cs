using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FacilityData
{
    public float xpos, ypos, zpos, xxpos, yypos, zzpos, xscale, yscale, zscale;
    public int pointId;
    public string obstName, modelFname, obstColor, modelName;
}
public class FacilityDataList
{
    public List<FacilityData> facility;
}
