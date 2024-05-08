using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CameraControllerAndroid : MonoBehaviour
{ 
    [SerializeField] private float movingSpeed = 50f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float zoomSpeed = 300f;

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
    }

    private void MoveCamera()
    {
        positionX = Input.GetTouch(0).deltaPosition.x * touchSpeed * Time.deltaTime;
        positionZ = Input.GetTouch(0).deltaPosition.y * touchSpeed * Time.deltaTime;

        Quaternion yRotation = Quaternion.Euler(0f, transform.GetChild(0).eulerAngles.y, 0f);
        transform.position += yRotation * new Vector3(positionX * -movingSpeed, 0, positionZ * -movingSpeed);
    }

    /*
    private bool EnableMove()
    {

    }
    */

    private void ZoomCamera()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        transform.position = Vector3.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), deltaMagnitudeDiff * perspectiveZoomSpeed);
    }

    private void RotateCamera()
    {
        orbit = Quaternion.Euler(-rotationY, rotationX, 0) * Vector3.forward * orbitRadius;
        rotationX += Input.GetTouch(0).deltaPosition.x * touchSpeed * rotationSpeed * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY -= Input.GetTouch(0).deltaPosition.y * touchSpeed * rotationSpeed * Time.deltaTime, -rotationLimit, rotationLimit);

        //transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);

        transform.GetChild(0).position = transform.position + orbit;
        transform.GetChild(0).LookAt(transform.position);
    }

    private void SetCameraSeeGround()
    {
        rotationY = rotationLimit;
        rotationX += 180;
        // RotateCamera();
    }
}
