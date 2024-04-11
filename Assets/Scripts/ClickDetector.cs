using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ClickDetector : MonoBehaviour
{
    [SerializeField]
    private CanvasManager canvasManager;
    [SerializeField]
    private float sphereCastRadius = 0.1f;

    private void Awake()
    {
        canvasManager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => ClickObject());
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
        //if (Physics.SphereCast(ray, _sphereCastRadius, out point) && point.transform.gameObject.layer != 6)
        //    LoadPipe(point.transform);
        RaycastHit[] hitList = Physics.SphereCastAll(ray, sphereCastRadius, 100f); // 구 캐스트가 파이프에 닿기 때문에 눈에 보이는 점 대신 애매한 곳에 point가 생성됨
                                                                                    // 애매한 점은 화면상 가까운 곳이라는 보장이 없음
        if (hitList.Length > 0)
        {
            GameObject nearestPipe = null;
            float minDistance = float.MaxValue;
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
            */
            foreach (RaycastHit obj in hitList)
            {
                // Ray rayToHit = new Ray(Camera.main.transform.position, obj.point - Camera.main.transform.position);
                Ray rayToHit = new Ray(Camera.main.transform.position, new Vector3(obj.point.x, 0f, obj.point.z) - Camera.main.transform.position);
                if (Physics.Raycast(rayToHit, out RaycastHit hit))
                {
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(hit.point);
                    float distance = Vector2.Distance(Input.mousePosition, new Vector2(screenPoint.x, screenPoint.y));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestPipe = hit.transform.gameObject;
                    }
                }
            }

            if (nearestPipe != null)
                LoadInfo(nearestPipe.transform);
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
