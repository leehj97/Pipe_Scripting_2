using System.Net;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private float sphereCastRadius = 0.1f;
    [SerializeField] private float maxSphereCastRadius = 0.01f;

    private void Awake()
    {
        canvasManager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => DetectObject2(0.001f));
    }
    private void LoadInfo(Transform transform)
    {
        canvasManager.OpenInfo();

        if (transform.CompareTag("Pipe"))
        {
            PipeInfo pipeInfo = transform.GetComponent<PipeInfo>();
            canvasManager.SetInfo(pipeInfo.pipeMaterial, pipeInfo.pipeYear, pipeInfo.linkId, pipeInfo.obstName);
        }
        else if (transform.CompareTag("Facility"))
        {
            FacilityInfo facilityInfo = transform.GetComponent<FacilityInfo>();
            canvasManager.SetInfo(facilityInfo.obstName, facilityInfo.pointId);
        }
    }

    private void DetectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;

        if (Physics.Raycast(ray, out point) && point.transform.gameObject.layer != 6)
        {
            LoadInfo(point.transform);
            return;
        }

        RaycastHit[] hitList = Physics.SphereCastAll(ray, sphereCastRadius, 1000f);

        if (hitList.Length > 0)
        {
            GameObject nearestObj = null;
            float minDistance = float.MaxValue;
            
            foreach (RaycastHit obj in hitList)
            {
                Transform pipeTransform = obj.transform;
                Vector3 pipePos = pipeTransform.position;
                float pipeHeight = pipeTransform.localScale.y;
                float pipeRad = pipeTransform.localScale.x;

                Vector2 hitPoint = Camera.main.WorldToScreenPoint(obj.point);
                Vector2 startPoint = Camera.main.WorldToScreenPoint(pipePos + Vector3.up * pipeHeight / 2f);
                Vector2 endPoint = Camera.main.WorldToScreenPoint(pipePos - Vector3.up * pipeHeight / 2f);
                Vector2 direction = endPoint- startPoint;
                Vector2 clickPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);

                Vector2 closestPointOnLine = FindNearestPointOnLine(startPoint, direction, clickPoint);

                // Debug.Log(hitPoint);
                // Debug.Log(direction.normalized);

                float distance = Vector2.Distance(clickPoint, closestPointOnLine);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObj = obj.transform.gameObject;
                }
            }
            /*
            foreach (RaycastHit obj in hitList)
            {
                Transform pipeTransform = obj.transform;
                Vector3 pipePos = pipeTransform.position;
                float pipeHeight = pipeTransform.localScale.y;
                float pipeRad = pipeTransform.localScale.x;

                Vector2 startPoint = Camera.main.WorldToScreenPoint(pipePos + Vector3.up * pipeHeight / 2f);
                Vector2 endPoint = Camera.main.WorldToScreenPoint(pipePos - Vector3.up * pipeHeight / 2f);
                Vector2 clickPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);

                Vector2 closestPointOnLine = FindNearestPointOnLine(startPoint, endPoint, clickPoint);

                Debug.Log(startPoint);
                Debug.Log(endPoint);
                // Debug.Log(closestPointOnLine);

                float pipeDia = pipeTransform.localScale.x;
                float distanceToCamera = Vector3.Distance(Camera.main.transform.position, pipePos);
                float pipeDiaOnScreen = pipeDia * Camera.main.fieldOfView / (2 * distanceToCamera * Mathf.Tan(Mathf.Deg2Rad * (Camera.main.fieldOfView / 2)));

                float distance = Vector2.Distance(clickPoint, closestPointOnLine) - (pipeDiaOnScreen / 2f);

                // Debug.Log(distance);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObj = obj.transform.gameObject;
                }
            }
            */
            if (nearestObj != null)
                LoadInfo(nearestObj.transform);
        }
    }
    public Vector2 FindNearestPointOnLine2(Vector2 origin, Vector2 direction, Vector2 point)
    {
        direction.Normalize();
        Vector2 lhs = point - origin;

        float dotP = Vector2.Dot(lhs, direction);
        return origin + direction * dotP;
    }
    private Vector2 FindNearestPointOnLine(Vector2 origin, Vector2 end, Vector2 point)
    {
        Vector2 heading = (end - origin);
        float magnitudeMax = heading.magnitude; // 선분의 길이 구하기
        heading.Normalize();    // 선분의 방향 구하기

        Vector2 lhs = point - origin;
        float dotP = Vector2.Dot(lhs, heading); // lhs 와 선분의 방향 사이의 내적 계산
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax); // 투영 길이를 0에서 선분의 길이 사이로 제한
        return origin + heading * dotP; // 시작점으로 부터 투영길이만큼 선분의 방향으로 이동한 새로운 점을 계산해서 반환
    }

    private void DetectObject2(float defaultSphereCastRadius)
    {
        Ray sphereRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hitList = Physics.SphereCastAll(sphereRay, sphereCastRadius, 1000f);

        if (hitList.Length > 0)
        {
            Vector3[] objOriginalPos = new Vector3[hitList.Length];
            float objMaxYPos = float.MinValue;

            for (int i = 0; i < hitList.Length; i++)
            {
                objOriginalPos[i] = hitList[i].transform.position;
            }

            foreach (RaycastHit obj in hitList)
            {
                float objYPos = obj.transform.position.y;
                if (objYPos > objMaxYPos)
                    objMaxYPos = objYPos;
            }

            foreach (RaycastHit obj in hitList)
            {
                Vector3 objPos = obj.transform.position;
                objPos.y = objMaxYPos;
                obj.transform.position = objPos;
            }

            while (true)
            {
                if (Physics.SphereCast(sphereRay, defaultSphereCastRadius, out RaycastHit hit))
                {
                    for (int i = 0; i < hitList.Length; i++)
                    {
                        hitList[i].transform.position = objOriginalPos[i];
                    }

                    LoadInfo(hit.collider.transform);
                }
                else
                {
                    if (defaultSphereCastRadius > maxSphereCastRadius) return;
                    defaultSphereCastRadius += 0.001f;
                    continue;
                }
                break;
            }
        }
    }

    private void DetectObject3(float defaultSphereCastRadius)
    {
        Ray sphereRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        while (true)
        {
            if (Physics.SphereCast(sphereRay, defaultSphereCastRadius, out RaycastHit hit))
            {
                LoadInfo(hit.collider.transform);
            }
            else
            {
                if (defaultSphereCastRadius > maxSphereCastRadius) return;
                defaultSphereCastRadius += 0.001f;
                continue;
            }
            break;
        }
    }
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    private void ClickObject2()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;

        if (Physics.Raycast(ray, out point) && point.transform.gameObject.layer != 6)
        {
            LoadInfo(point.transform);
            return;
        }

        RaycastHit[] hitList = Physics.SphereCastAll(ray, maxSphereCastRadius, 1000f);

        if (hitList.Length > 0)
        {
            GameObject nearestPipe = null;
            float minDistance = float.MaxValue;

            foreach (RaycastHit obj in hitList)
            {
                Vector3 objTopSpot = obj.transform.position + (new Vector3(0, 1, 0) * obj.transform.localScale.y);
                Vector3 objBottomSpot = obj.transform.position + (new Vector3(0, -1, 0) * obj.transform.localScale.y);
            }
            /*
            foreach (RaycastHit obj in hitList)
            {
                Vector3 screenPoint = Camera.main.WorldToScreenPoint(obj.point);
                float distance = Vector2.Distance(Input.mousePosition, new Vector2(screenPoint.x, screenPoint.y));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPipe = obj.transform.gameObject;
                }
            }
            */

            foreach (RaycastHit obj in hitList)
            {
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(obj.point);
                Vector2 screenDirection = (Vector2)Camera.main.WorldToScreenPoint(obj.point + obj.transform.forward) - screenPoint;
                Vector2 closestPointOnLine = FindNearestPointOnLine(screenPoint, screenDirection, Input.mousePosition);

                Debug.Log(Input.mousePosition);
                Debug.Log(screenPoint);
                Debug.Log(screenDirection);
                Debug.Log(closestPointOnLine);

                float distance = Vector2.Distance(Input.mousePosition, closestPointOnLine);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPipe = obj.transform.gameObject;
                }
            }

            if (nearestPipe != null)
                LoadInfo(nearestPipe.transform);
        }
        
    }


    private void ClickObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        if (Physics.Raycast(ray, out point) && point.transform.gameObject.layer != 6)
        {
            LoadInfo(point.transform);
            return;
        }

        RaycastHit[] hitList = Physics.SphereCastAll(ray, maxSphereCastRadius, 1000f); 

        if (hitList.Length > 0)
        {
            GameObject nearestObj = null;
            float minDistance = float.MaxValue;

            foreach (RaycastHit obj in hitList)
            {
                Ray rayToHit = new Ray(Camera.main.transform.position, new Vector3(obj.point.x, 0f, obj.point.z) - Camera.main.transform.position);
                if (Physics.Raycast(rayToHit, out RaycastHit hit))
                {
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(hit.point);
                    float distance = Vector2.Distance(Input.mousePosition, new Vector2(screenPoint.x, screenPoint.y));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestObj = hit.transform.gameObject;
                    }
                }
            }

            /*
            Vector3 mouseViewportPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            foreach (RaycastHit obj in hitList)
            {
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(obj.point);
                float distance = Vector2.Distance(mouseViewportPoint, new Vector2(viewportPoint.x, viewportPoint.y));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPipe = obj.transform.gameObject;
                }
                Debug.Log(viewportPoint.ToString("f4"));
                Debug.Log(distance.ToString("f4"));
            }

            foreach (Collider obj in hitList)
            {
                float tempDistance = Vector3.Distance(obj.transform.position, ray.origin);
                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    nearestPipe = obj.transform.gameObject;
                }
            }
            */

            if (nearestObj != null)
                LoadInfo(nearestObj.transform);
        }
    }
}
