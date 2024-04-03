using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetVertex : IGetVector3ValueList
{
    private List<float> _xList, _yList, _zList;
    public GetVertex(List<float> xList, List<float> yList, List<float> zList)
    {
        this._xList = xList;
        this._yList = yList;
        this._zList = zList;
    }
    public List<Vector3> GetVector3List()
    {
        float minX = _xList.Min();
        float maxX = _xList.Max();

        float minY = _yList.Min();

        float minZ = _zList.Min();
        float maxZ = _zList.Max();

        return new List<Vector3> { new Vector3(minX, minY, minZ), new Vector3(minX, minY, maxZ), new Vector3(maxX, minY, minZ), new Vector3(maxX, minY, maxZ) };
    }
}
