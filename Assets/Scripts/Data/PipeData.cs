using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipeData
{
    public string pipeMaterial, pipeYear, toNodePoint, obstColor, frNodePoint, obstName;
    public int linkId;
    public float pipeDia;
}
public class PipeDataList
{
    public List<PipeData> pipeline;
}
