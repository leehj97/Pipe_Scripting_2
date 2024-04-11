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
            transform.position = GetClosestCameraPosition(Camera.main, vertex); // 구한 (0, -minDistance, 0) 위치에 focus 를 위치
    }

    private Vector3 GetClosestCameraPosition(Camera cam, List<Vector3> points)
    {
        float verticalFov = cam.fieldOfView * Mathf.Deg2Rad;                                                       // 60도, 1.047198 Rad
        float horizontalFov = Camera.VerticalToHorizontalFieldOfView(cam.fieldOfView, cam.aspect) * Mathf.Deg2Rad;   // 113.9132도, 1.988161 Rad

        float verticalCot = 1f / Mathf.Tan(0.5f * verticalFov);   // cot(π/6) = √3 = 1.732051
        float horizontalCot = 1f / Mathf.Tan(0.5f * horizontalFov);   // cot(57π/180) = 0.6504847

        verticalCot *= 1.1f;

        float minDistance = points.Select(p => GetClosestCameraDistance(p, verticalCot, horizontalCot)).Min();

        return minDistance * cam.transform.forward;   // 구한 거리(음수)를 카메라가 바라보는 방향(땅바닥, (0, -1, 0))에 곱해주면 (0, -minDistance, 0)
    }

    private float GetClosestCameraDistance(Vector3 point, float verticalCot, float horizontalCot)
    {
        float d = Mathf.Max(Mathf.Abs(point.z) * verticalCot, Mathf.Abs(point.x) * horizontalCot);  // 수직시야각과 수평시야각의 cot * 좌표값 중 더 큰 값

        return point.y - d; // 구한 값을 점의 y값에서 빼줌 (실질적인 최대 거리)
    }
}
