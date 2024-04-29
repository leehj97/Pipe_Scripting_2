using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PipesPositionGetter
{
    private List<float> xList = new List<float>();
    private List<float> yList = new List<float>();
    private List<float> zList = new List<float>();
    public List<float> XList
    { get { return xList; } }
    public List<float> YList
    { get { return yList; } }
    public List<float> ZList
    { get { return zList; } }

    public void GetPipePos(float xPos, float yPos, float zPos)
    {
        XList.Add(xPos);
        YList.Add(yPos);
        ZList.Add(zPos);
    }
}
