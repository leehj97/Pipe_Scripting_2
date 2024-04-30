using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ClickDetectorAndroid : MonoBehaviour
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
            .Where(_ => Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && !MouseOverUILayerObject.IsPointerOverUIObject())
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

    private void DetectObject(float defaultSphereCastRadius)
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
                if (Physics.SphereCast(ray, defaultSphereCastRadius, out RaycastHit hit))
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
}
