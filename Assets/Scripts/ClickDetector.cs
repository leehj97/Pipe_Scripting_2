using UnityEngine;
using UniRx;
using UniRx.Triggers;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Unity.Burst.CompilerServices;
using System.Collections.Generic;

public class ClickDetector : MonoBehaviour
{
    [SerializeField]
    private CanvasManager canvasManager;
    [SerializeField]
    private float sphereCastRadius = 0.3f;

    private void Awake()
    {
        canvasManager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => ClickObjectAI());
    }

    private void ClickObjectAI()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        GameObject nearestObj = null;
        float minDistance = float.MaxValue;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hitList = Physics.SphereCastAll(ray, sphereCastRadius, 1000f);

        if (hitList.Length > 0)
        {
            foreach (RaycastHit obj in hitList)
            {
                Vector3 closestPoint = obj.transform.GetComponent<Collider>().ClosestPoint(worldMousePosition);
                float distance = Vector3.Distance(worldMousePosition, closestPoint);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObj = obj.transform.gameObject;
                }
            }

            if (nearestObj != null)
            {
                LoadInfo(nearestObj.transform);
            }
        }
    }

    private void ClickObject4()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // ray.direction = (Camera.main.transform.rotation * Vector3.forward);
        RaycastHit point;

        if (Physics.Raycast(ray, out point) && point.transform.gameObject.layer != 6)
        {
            LoadInfo(point.transform);
            return;
        }

        RaycastHit[] hitList = Physics.SphereCastAll(ray, sphereCastRadius, 1000f);

        if (hitList.Length > 0)
        {
            GameObject nearestPipe = null;
            float minDistance = float.MaxValue;

            // Debug.Log(ray.direction);
            Debug.Log(Camera.main.WorldToScreenPoint(ray.origin));


            foreach (RaycastHit obj in hitList)
            {
                Vector3 pos = obj.collider.transform.position;
                Quaternion rot = obj.collider.transform.rotation;
                Vector3 closestPointCollider = Physics.ClosestPoint(ray.origin, obj.collider, pos, rot);
                
                // Debug.Log(Camera.main.WorldToScreenPoint(closestPointCollider));
                Debug.Log(Camera.main.WorldToScreenPoint(obj.point));

                float distance = Vector2.Distance(Camera.main.WorldToScreenPoint(ray.origin),
                    (Vector2)Camera.main.WorldToScreenPoint(obj.point));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPipe = obj.transform.gameObject;
                }

                Debug.Log(distance);
            }

            if (nearestPipe != null)
                LoadInfo(nearestPipe.transform);
        }
    }

    private void ClickObject3(float sphereCastRad)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.SphereCast(ray, sphereCastRad, out RaycastHit hit))
        {
            LoadInfo(hit.collider.transform);
        }
        else
        {
            if (sphereCastRad > 0.1f)
                return;
            ClickObject3(sphereCastRad + 0.001f);
        }
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

        RaycastHit[] hitList = Physics.SphereCastAll(ray, sphereCastRadius, 1000f);

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

    private Vector2 FindNearestPointOnLine(Vector2 origin, Vector2 direction, Vector2 point)
    {
        direction.Normalize();
        Vector2 lhs = point - origin;

        float dotP = Vector2.Dot(lhs, direction);
        return origin + direction * dotP;
    }

    private Vector2 FindNearestPointOnLine2(Vector2 origin, Vector2 end, Vector2 point)
    {
        //Get heading
        Vector2 heading = (end - origin);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Do projection from the point but clamp it
        Vector2 lhs = point - origin;
        float dotP = Vector2.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
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

        RaycastHit[] hitList = Physics.SphereCastAll(ray, sphereCastRadius, 1000f); 

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
}
