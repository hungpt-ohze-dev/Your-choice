using UnityEngine;

public class InputRaycast : MonoBehaviour
{
    Camera baseCamera;

    void Awake()
    {
        baseCamera = Camera.main; // Camera Base
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = baseCamera.ScreenPointToRay(Input.mousePosition);
            CheckRaycast(ray);
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = baseCamera.ScreenPointToRay(touch.position);
                CheckRaycast(ray);
            }
        }
    }

    void CheckRaycast(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Debug.Log("Hit: " + hit.collider.name);

            // Gọi hàm trên object
            hit.collider.SendMessage("OnInputDown", SendMessageOptions.DontRequireReceiver);
        }
    }
}
