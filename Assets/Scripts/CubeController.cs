//#define CUBE_CONTROLLER_LOG
#define CUBE_ROTATE_VERTICAL
//#define CUBE_ROTATE_COLUMN

using UnityEngine;
using UnityEngine.EventSystems;

enum DragMode
{
    NO_DRAG = -1,
    DRAG_CUBE_HORIZONTAL = 0,
    DRAG_CUBE_VERTICAL = 1,
    DRAG_COLUMN = 2,
}

public class CubeController : MonoBehaviour {

    public Camera cubeCamera;
    public Transform cubeContainer;
    public Cube cube;
    public RectTransform panelCube;
    public MapController mapController;

    public float anglePerScreenWidth;

    Quaternion desiredRotation;
    float curVelocity;
    DragMode dragMode;

    public float anglePerScreenHeight;

#if CUBE_ROTATE_VERTICAL
    float deltaVRotation;
    float rotationVRate;
#endif // CUBE_ROTATE_VERTICAL

    float deltaHRotation;
    float rotationHRate;

    Vector2 lastDraggingPos;

    public void Init()
    {
#if CUBE_ROTATE_VERTICAL
        rotationVRate = anglePerScreenHeight / Screen.height;
#endif // CUBE_ROTATE_VERTICAL
        rotationHRate = anglePerScreenWidth / Screen.width;

        // Set up rubik local rotation
        //cube.SetParent(null, true);
        //cubeContainer.rotation = cubeCamera.transform.rotation;
        //cube.SetParent(cubeContainer, true);
        //desiredRotation = cube.localRotation;
        desiredRotation = Quaternion.identity;
        dragMode = DragMode.NO_DRAG;
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
        Quaternion adjustedDesiredRotation = desiredRotation;
#if CUBE_ROTATE_VERTICAL
        if (deltaHRotation != 0 || deltaVRotation != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(
                deltaVRotation, deltaHRotation, 0);
#else
        if (deltaHRotation != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(0, deltaHRotation, 0);
#endif // CUBE_ROTATE_VERTICAL
            adjustedDesiredRotation = deltaRotation * adjustedDesiredRotation;
        }

        if (cube.transform.localRotation != adjustedDesiredRotation)
        {
            float angle = Quaternion.Angle(
                cube.transform.localRotation, adjustedDesiredRotation);
            float step = Mathf.SmoothDampAngle(0, angle, ref curVelocity, 0.1f);
            cube.transform.localRotation = Quaternion.RotateTowards(
                cube.transform.localRotation, adjustedDesiredRotation, step);
        }

//#if CUBE_CONTROLLER_LOG
//        Debug.LogFormat("Cube delta H rotation = {0}", deltaHRotation);
//        Debug.LogFormat("Cube adjusted rotation = {0}", adjustedDesiredRotation.eulerAngles);
//#endif // CUBE_CONTROLLER_LOG
    }

    public void Activate()
    {
        cubeCamera.gameObject.SetActive(true);
        panelCube.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        cubeCamera.gameObject.SetActive(false);
        panelCube.gameObject.SetActive(false);
    }

    public void OnButtonBackToMapClicked()
    {
        Deactivate();
        mapController.Activate();
    }

    public void OnDragBegin(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
#if CUBE_CONTROLLER_LOG
        Debug.LogFormat("Drag started, delta = {0}", pointerData.delta);
#endif
        lastDraggingPos = pointerData.position + pointerData.delta;

#if CUBE_ROTATE_COLUMN
        // Check if drag cube or drag column
        Ray ray = rubikCamera.ScreenPointToRay(pointerData.position);
        if (Physics.Raycast(ray, 100f, Global.MASK_CUBE))
        {
            dragMode = DragMode.DRAG_COLUMN;
        }
        else
#endif // CUBE_ROTATE_COLUMN
        {
            if (Mathf.Abs(pointerData.delta.x) >= Mathf.Abs(pointerData.delta.y))
            {
                dragMode = DragMode.DRAG_CUBE_HORIZONTAL;
            }
            else
            {
                dragMode = DragMode.DRAG_CUBE_VERTICAL;
            }

            deltaHRotation = 0f;

#if CUBE_ROTATE_VERTICAL
            deltaVRotation = 0f;
#endif // CUBE_ROTATE_VERTICAL
        }
    }

    public void OnDragContinue(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        Vector2 dragDelta = pointerData.position - lastDraggingPos;

#if CUBE_ROTATE_COLUMN
        if (dragMode == DragMode.DRAG_CUBE)
#endif // CUBE_ROTATE_COLUMN
        {
            if (dragMode == DragMode.DRAG_CUBE_HORIZONTAL)
            {
                deltaHRotation = -dragDelta.x * rotationHRate;
            }
            else
            {
#if CUBE_ROTATE_VERTICAL
                deltaVRotation = dragDelta.y * rotationVRate;
#endif // CUBE_ROTATE_VERTICAL
            }
        }

#if CUBE_CONTROLLER_LOG
        Debug.LogFormat("Drag: deltaX = {0}", deltaHRotation);
#endif // CUBE_CONTROLLER_LOG
    }

    public void OnDragEnd(BaseEventData eventData)
    {
#if CUBE_CONTROLLER_LOG
        Debug.Log("Drag end");
#endif // CUBE_CONTROLLER_LOG

        RotateDirection direction;
        float newAngle;

        switch (dragMode)
        {
            case DragMode.DRAG_CUBE_HORIZONTAL:
            {
                newAngle = NearestAngle(deltaHRotation, true, out direction);
                Quaternion deltaRotation = 
                        Quaternion.Euler(0f, newAngle, 0f);
                desiredRotation = deltaRotation * desiredRotation;
                cube.RotateMatrix(direction);
                cube.UpdateRelativeLocations();

                // TODO: Adjust rotation again to ensure no accumulated epsilon

                deltaHRotation = 0f;
                break;
            }
#if CUBE_ROTATE_VERTICAL
            case DragMode.DRAG_CUBE_VERTICAL:
            {
                newAngle = NearestAngle(deltaVRotation, false, out direction);
                Quaternion deltaRotation = 
                    Quaternion.Euler(newAngle, 0f, 0f);
                desiredRotation = deltaRotation * desiredRotation;
                cube.RotateMatrix(direction);
                cube.UpdateRelativeLocations();
                
                // TODO: Adjust rotation again to ensure no accumulated epsilon  
                
                deltaVRotation = 0f;
                break;
            }
#endif // CUBE_ROTATE_VERTICAL

#if CUBE_ROTATE_COLUMN
            case DragMode.DRAG_CUBE:
            {
                break;
            }
#endif // CUBE_ROTATE_COLUMN
        }

        dragMode = DragMode.NO_DRAG;
    }

    float NearestAngle(float curAngle, bool isHorizontal, out RotateDirection direction)
    {
        float result = 0f;
        direction = RotateDirection.NO_ROTATION;

        if (curAngle < 0f)
        {
            while (curAngle < result)
            {
                if (Mathf.Abs(result - curAngle) < 30f)
                {
                    break;
                }
                result -= 90f;
                if (isHorizontal)
                {
                    direction = RotateDirection.ROTATE_RIGHT;
                }
                else
                {
                    direction = RotateDirection.ROTATE_DOWN;
                }
            }
        }
        else if (curAngle > 0f)
        {
            while (result < curAngle)
            {
                if (Mathf.Abs(result - curAngle) < 30f)
                {
                    break;
                }
                result += 90;

                if (isHorizontal)
                {
                    direction = RotateDirection.ROTATE_LEFT;
                }
                else
                {
                    direction = RotateDirection.ROTATE_UP;
                }
            }
        }
        return result;
    }
}
