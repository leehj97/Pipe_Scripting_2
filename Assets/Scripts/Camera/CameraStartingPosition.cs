using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraStartingPosition : MonoBehaviour
{
    [SerializeField]
    private PipeJsonConverter pipejsonConverter;
    [SerializeField]
    private List<Vector3> vertex;

    private IPipeVector3ValueList iPipeVector3ValueList;

    private void Start()
    {
        iPipeVector3ValueList = new CornerPipesVector3ListGetter(pipejsonConverter.XList, pipejsonConverter.YList, pipejsonConverter.ZList);
        vertex = iPipeVector3ValueList.GetVector3List();

        if (pipejsonConverter.XList != null && pipejsonConverter.YList != null && pipejsonConverter.ZList != null) 
            transform.position = GetClosestCameraPosition(Camera.main, vertex); // ���� (0, -minDistance, 0) ��ġ�� focus �� ��ġ
    }

    private Vector3 GetClosestCameraPosition(Camera cam, List<Vector3> points)
    {
        float verticalFov = cam.fieldOfView * Mathf.Deg2Rad;                                                       // 60��, 1.047198 Rad
        float horizontalFov = Camera.VerticalToHorizontalFieldOfView(cam.fieldOfView, cam.aspect) * Mathf.Deg2Rad;   // 113.9132��, 1.988161 Rad

        float verticalCot = 1f / Mathf.Tan(0.5f * verticalFov);   // cot(��/6) = ��3 = 1.732051
        float horizontalCot = 1f / Mathf.Tan(0.5f * horizontalFov);   // cot(57��/180) = 0.6504847

        verticalCot *= 1.1f;

        float minDistance = points.Select(p => GetClosestCameraDistance(p, verticalCot, horizontalCot)).Min();

        return minDistance * cam.transform.forward;   // ���� �Ÿ�(����)�� ī�޶� �ٶ󺸴� ����(���ٴ�, (0, -1, 0))�� �����ָ� (0, -minDistance, 0)
    }

    private float GetClosestCameraDistance(Vector3 point, float verticalCot, float horizontalCot)
    {
        float d = Mathf.Max(Mathf.Abs(point.z) * verticalCot, Mathf.Abs(point.x) * horizontalCot);  // �����þ߰��� ����þ߰��� cot * ��ǥ�� �� �� ū ��

        return point.y - d; // ���� ���� ���� y������ ���� (�������� �ִ� �Ÿ�)
    }
}
