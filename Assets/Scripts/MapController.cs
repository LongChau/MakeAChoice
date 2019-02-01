//#define MAP_CONTROLLER_LOG

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapController : MonoBehaviour {

    public CanvasScaler scaler;
    public Transform cameraContainer;
    public Camera mapCamera;
    public RectTransform panelMap;
    public RectTransform cubeButton;

    public CubeController cubeController;

    public float anglePerScreenWidth;

    float curHorzRotation;
    float desiredHorzRotation;
    float curHVelocity;

    float lastHorzRotation;
    float deltaHorzRotation;
    float rotationRate;

    Vector2 lastDraggingPos;
    float scaleRate;

    public void Init()
    {
        rotationRate = anglePerScreenWidth / Screen.width;
        scaleRate = scaler.referenceResolution.x / Screen.width;

        // Calculate cube button position
        Vector2 centerPos = mapCamera.WorldToScreenPoint(
            cubeController.cube.transform.position);
        cubeButton.anchoredPosition = centerPos * scaleRate;
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    //// Update is called once per frame
    //void Update () {

    //}

    void LateUpdate()
    {
        curHorzRotation = Mathf.SmoothDampAngle(curHorzRotation, desiredHorzRotation,
            ref curHVelocity, 0.1f);
        cameraContainer.rotation = Quaternion.Euler(0, curHorzRotation, 0);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        panelMap.gameObject.SetActive(false);
        cameraContainer.gameObject.SetActive(false);
    }

    public void Activate()
    {
        cameraContainer.gameObject.SetActive(true);
        panelMap.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void OnButtonRubikRotateClicked()
    {
        Deactivate();
        cubeController.Activate();
    }

    public void OnDragBegin(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
#if MAP_CONTROLLER_LOG
        Debug.Log("Drag started");
#endif
        lastDraggingPos = pointerData.position;
        lastHorzRotation = desiredHorzRotation;
    }

    public void OnDragContinue(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        Vector2 dragDelta = pointerData.position - lastDraggingPos;
        deltaHorzRotation = dragDelta.x * rotationRate;
        desiredHorzRotation = lastHorzRotation + deltaHorzRotation;
#if MAP_CONTROLLER_LOG
        Debug.LogFormat("Drag: deltaX = {0}", deltaHorzRotation);
#endif
    }

    public void OnDragEnd(BaseEventData eventData)
    {
#if MAP_CONTROLLER_LOG
        Debug.Log("Drag end");
#endif
        desiredHorzRotation = lastHorzRotation + deltaHorzRotation;

        while (desiredHorzRotation >= 360f)
        {
            desiredHorzRotation -= 360f;
        }

        while (desiredHorzRotation <= -360f)
        {
            desiredHorzRotation += 360f;
        }

        deltaHorzRotation = 0;
    }
}
