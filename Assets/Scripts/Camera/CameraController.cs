using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
#if !UNITY_ANDROID
    [SerializeField] private float movingSpeed = 150f;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float zoomSpeed = 1000f;
#endif
#if UNITY_ANDROID
    [SerializeField] private float movingSpeed = 50f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float zoomSpeed = 300f;
#endif

    [SerializeField] private float rotationLimit = 89f;
    [SerializeField] private float orbitRadius = 10f;
    [SerializeField] private float zoomPoint = 500f;
    [SerializeField] private float minZoom = -500f;
    [SerializeField] private float maxZoom = 500f;

    private float perspectiveZoomSpeed = 0.2f;
    private float touchSpeed = 0.1f;

    private float positionX;
    private float positionZ;
    private float rotationX;
    private float rotationY;
    private Vector3 orbit;

    private void Start()
    {
        SetCameraSeeGround();
        // mergetest
#if !UNITY_ANDROID
        this.LateUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.Mouse0) && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => MoveCamera());

        this.LateUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.Mouse1) && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => RotateCamera());

        this.LateUpdateAsObservable()
            .Where(_ => Input.GetAxis("Mouse ScrollWheel") != 0 && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => ZoomCamera());
#endif
#if UNITY_ANDROID
        /*
         * this.UpdateAsObservable()
            .Where(_ => )
            .Subscribe(_ => EnableMove());
        */ // 길게 누르는 조건 달성하는 스트림

        this.LateUpdateAsObservable()
            .Where(_ => Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => MoveCamera());  // 길게 누르는 조건 달성됐을 때 조건 추가

        this.LateUpdateAsObservable()
            .Where(_ => Input.touchCount == 3 && Input.GetTouch(0).phase == TouchPhase.Moved &&
            Input.GetTouch(1).phase == TouchPhase.Moved && Input.GetTouch(2).phase == TouchPhase.Moved && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => RotateCamera());

        this.LateUpdateAsObservable()
            .Where(_ => Input.touchCount == 2 && !MouseOverUILayerObject.IsPointerOverUIObject())
            .Subscribe(_ => ZoomCamera());
#endif
    }

    private void MoveCamera()
    {
#if !UNITY_ANDROID
        positionX = Input.GetAxis("Mouse X") * Time.deltaTime;
        positionZ = Input.GetAxis("Mouse Y") * Time.deltaTime;

        Quaternion yRotation = Quaternion.Euler(0f, transform.GetChild(0).eulerAngles.y, 0f);
        transform.position += yRotation * new Vector3(positionX * -movingSpeed, 0, positionZ * -movingSpeed);
#endif
#if UNITY_ANDROID
        positionX = Input.GetTouch(0).deltaPosition.x * touchSpeed * Time.deltaTime;
        positionZ = Input.GetTouch(0).deltaPosition.y * touchSpeed * Time.deltaTime;

        Quaternion yRotation = Quaternion.Euler(0f, transform.GetChild(0).eulerAngles.y, 0f);
        transform.position += yRotation * new Vector3(positionX * -movingSpeed, 0, positionZ * -movingSpeed);
#endif
    }

    /*
    private bool EnableMove()
    {

    }
    */

    private void ZoomCamera()
    {
#if !UNITY_ANDROID
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        Physics.Raycast(ray, out point);
        Vector3 scrolldirection = ray.GetPoint(zoomPoint);

        float step = zoomSpeed * Time.deltaTime;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && scrolldirection.y > minZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, scrolldirection,
                Input.GetAxis("Mouse ScrollWheel") * step);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && scrolldirection.y < maxZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, scrolldirection,
                Input.GetAxis("Mouse ScrollWheel") * step);
        }
#endif
#if UNITY_ANDROID
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        transform.position = Vector3.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), deltaMagnitudeDiff * perspectiveZoomSpeed);
#endif
    }

    private void RotateCamera()
    {
#if !UNITY_ANDROID
        orbit = Quaternion.Euler(-rotationY, rotationX, 0) * Vector3.forward * orbitRadius;
        rotationX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, -rotationLimit, rotationLimit);

        //transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);

        transform.GetChild(0).position = transform.position + orbit;
        transform.GetChild(0).LookAt(transform.position);
#endif
#if UNITY_ANDROID
        orbit = Quaternion.Euler(-rotationY, rotationX, 0) * Vector3.forward * orbitRadius;
        rotationX += Input.GetTouch(0).deltaPosition.x * touchSpeed * rotationSpeed * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY -= Input.GetTouch(0).deltaPosition.y * touchSpeed * rotationSpeed * Time.deltaTime, -rotationLimit, rotationLimit);

        //transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);

        transform.GetChild(0).position = transform.position + orbit;
        transform.GetChild(0).LookAt(transform.position);
#endif
    }

    private void SetCameraSeeGround()
    {
        rotationY = rotationLimit;
        rotationX += 180;
#if !UNITY_ANDROID
        RotateCamera();
#endif
    }
}
