using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _movingSpeed = 150f;
    [SerializeField]
    private float _rotationSpeed = 150f;
    [SerializeField]
    private float _rotationLimit = 89f;
    [SerializeField]
    private float _orbitRadius = 10f;
    [SerializeField]
    public float _zoomSpeed = 3000f;
    [SerializeField]
    public float _zoomPoint = 500f;
    [SerializeField]
    public float _minZoom = -500f;
    [SerializeField]
    public float _maxZoom = 500f;

    private float _positionX;
    private float _positionZ;
    private float _rotationX;
    private float _rotationY;
    private Vector3 _orbit;

    private void Start()
    {
        SetCameraSeeGround();
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            MoveCamera();

        if (Input.GetKey(KeyCode.Mouse1))
            RotateCamera();

        ZoomCamera();
    }

    private void MoveCamera()
    {
        _positionX = Input.GetAxis("Mouse X") * Time.deltaTime;
        _positionZ = Input.GetAxis("Mouse Y") * Time.deltaTime;

        Quaternion yRotation = Quaternion.Euler(0f, transform.GetChild(0).eulerAngles.y, 0f);
        transform.position += yRotation * new Vector3(_positionX * -_movingSpeed, 0, _positionZ * -_movingSpeed);
    }

    private void ZoomCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        Physics.Raycast(ray, out point);
        Vector3 scrolldirection = ray.GetPoint(_zoomPoint);

        float step = _zoomSpeed * Time.deltaTime;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && scrolldirection.y > _minZoom)
        { 
            transform.position = Vector3.MoveTowards(transform.position, scrolldirection, Input.GetAxis("Mouse ScrollWheel") * step);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && scrolldirection.y < _maxZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, scrolldirection, Input.GetAxis("Mouse ScrollWheel") * step);
        }
    }

    private void RotateCamera()
    {
        _orbit = Quaternion.Euler(-_rotationY, _rotationX, 0) * Vector3.forward * _orbitRadius;
        _rotationX += Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY -= Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime, -_rotationLimit, _rotationLimit);

        transform.GetChild(0).position = transform.position + _orbit;
        transform.GetChild(0).LookAt(transform.position);
    }
    private void SetCameraSeeGround()
    {
        _rotationY = _rotationLimit;
        _rotationX += 180;
        RotateCamera();
    }
}
