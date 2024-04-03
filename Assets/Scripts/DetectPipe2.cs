using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectPipe2 : MonoBehaviour
{
    [SerializeField]
    private CanvasManager _canvasManager;
    [SerializeField]
    private float _sphereCastRadius = 0.1f;

    private void Awake()
    {
        _canvasManager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !MouseOverUILayerObject.IsPointerOverUIObject())
            ClickPipe();
    }
    private void ClickPipe()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        if (Physics.Raycast(ray, out point) && point.transform.gameObject.layer != 6)
        {
            LoadPipe(point.transform);
            return;
        }
        //if (Physics.SphereCast(ray, _sphereCastRadius, out point) && point.transform.gameObject.layer != 6)
        //    LoadPipe(point.transform);
        RaycastHit[] hitList = Physics.SphereCastAll(ray, _sphereCastRadius, 100f); // �� ĳ��Ʈ�� �������� ��� ������ ���� ���̴� �� ��� �ָ��� ���� point�� ������
                                                                                    // �ָ��� ���� ȭ��� ����� ���̶�� ������ ����
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
                LoadPipe(nearestPipe.transform);
        }
    }
    private void LoadPipe(Transform transform)
    {
        PipeInfo pipeInfo = transform.GetComponent<PipeInfo>();
        _canvasManager.OpenInfo();
        _canvasManager.SetInfo(pipeInfo.pipeMaterial, pipeInfo.pipeYear, pipeInfo.linkId, pipeInfo.obstName);
    }
}
