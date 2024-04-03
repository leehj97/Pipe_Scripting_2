using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CornerPipesVector3ListGetter : IPipeVector3ValueList
{
    private List<float> xList, yList, zList;
    public CornerPipesVector3ListGetter(List<float> xList, List<float> yList, List<float> zList)
    {
        this.xList = xList;
        this.yList = yList;
        this.zList = zList;
    }
    public List<Vector3> GetVector3List()
    {
        float minX = xList.Min();
        float maxX = xList.Max();

        float minY = yList.Min();

        float minZ = zList.Min();
        float maxZ = zList.Max();

        return new List<Vector3> { new Vector3(minX, minY, minZ), new Vector3(minX, minY, maxZ), new Vector3(maxX, minY, minZ), new Vector3(maxX, minY, maxZ) };
    }
}
