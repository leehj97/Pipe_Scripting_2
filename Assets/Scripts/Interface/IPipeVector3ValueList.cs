using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPipeVector3ValueList
{
    public List<Vector3> GetVector3List(List<float> xList, List<float> yList, List<float> zList);
}