using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField]
    private CanvasManager canvasManager;
    [SerializeField]
    private float maxSphereCastRadius = 0.1f;

    private void Awake()
    {
        canvasManager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => DetectObject(0.001f));
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

    private void DetectObject(float sphereCastRadius)
    {
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            
            if (Physics.SphereCast(ray, sphereCastRadius, out RaycastHit hit))
            {
                LoadInfo(hit.collider.transform);
            }
            else
            {
                if (sphereCastRadius > maxSphereCastRadius) return;
                sphereCastRadius += 0.0001f;
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
