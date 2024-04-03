using System.Collections.Generic;
using UnityEngine;

public class GetPointFromString : IGetVector3Value
{
    private string _point;
    public void SetPoint(string point) { _point = point; }
    public Vector3 GetVector3()
    {
        string[] separator = _point.TrimEnd(')').Split('(')[1].Split(' ');
        return new Vector3(float.Parse(separator[0]), float.Parse(separator[2]), float.Parse(separator[1]));
    }
}
