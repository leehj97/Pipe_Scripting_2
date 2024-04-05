using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;

public class CameraControllerUniRx : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed = 150f;
    [SerializeField]
    private float rotationSpeed = 150f;
    [SerializeField]
    private float rotationLimit = 89f;
    [SerializeField]
    private float orbitRadius = 10f;
    [SerializeField]
    public float zoomSpeed = 3000f;
    [SerializeField]
    public float zoomPoint = 500f;
    [SerializeField]
    public float minZoom = -500f;
    [SerializeField]
    public float maxZoom = 500f;

    private float positionX;
    private float positionZ;
    private float rotationX;
    private float rotationY;
    private Vector3 orbit;

    private void Start()
    {
        SetCameraSeeGround();

        this.LateUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.Mouse0))
            .Subscribe(_ => MoveCamera());

        this.LateUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.Mouse1))
            .Subscribe(_ => RotateCamera());

        this.LateUpdateAsObservable()
            .Where(_ => Input.GetAxis("Mouse ScrollWheel") != 0)
            .Subscribe(_ => ZoomCamera());
    }

    private void MoveCamera()
    {
        positionX = Input.GetAxis("Mouse X") * Time.deltaTime;
        positionZ = Input.GetAxis("Mouse Y") * Time.deltaTime;

        Quaternion yRotation = Quaternion.Euler(0f, transform.GetChild(0).eulerAngles.y, 0f);
        transform.position += yRotation * new Vector3(positionX * -movingSpeed, 0, positionZ * -movingSpeed);
    }

    private void ZoomCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        Physics.Raycast(ray, out point);
        Vector3 scrolldirection = ray.GetPoint(zoomPoint);

        float step = zoomSpeed * Time.deltaTime;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && scrolldirection.y > minZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, scrolldirection, Input.GetAxis("Mouse ScrollWheel") * step);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && scrolldirection.y < maxZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, scrolldirection, Input.GetAxis("Mouse ScrollWheel") * step);
        }
    }

    private void RotateCamera()
    {
        orbit = Quaternion.Euler(-rotationY, rotationX, 0) * Vector3.forward * orbitRadius;
        rotationX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, -rotationLimit, rotationLimit);

        transform.GetChild(0).position = transform.position + orbit;
        transform.GetChild(0).LookAt(transform.position);
    }
    private void SetCameraSeeGround()
    {
        rotationY = rotationLimit;
        rotationX += 180;
        RotateCamera();
    }
}
