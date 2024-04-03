using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PipeDetector : MonoBehaviour
{
    [SerializeField]
    private CanvasManager canvasManager;
    [SerializeField]
    private float sphereCastRadius = 0.1f;

    private void Awake()
    {
        canvasManager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
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
        if (Physics.SphereCast(ray, sphereCastRadius, out point) && point.transform.gameObject.layer != 6)
            LoadPipe(point.transform);
    }
    private void LoadPipe(Transform transform)
    {
        PipeInfo pipeInfo = transform.GetComponent<PipeInfo>();
        canvasManager.OpenInfo();
        canvasManager.SetInfo(pipeInfo.pipeMaterial, pipeInfo.pipeYear, pipeInfo.linkId, pipeInfo.obstName);
    }
}
