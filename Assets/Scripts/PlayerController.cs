using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float steerSpeed = 8f;
    [SerializeField] private float steerAcceleration = 12f;
    [SerializeField] private float halfWidth = 3.5f;
    [SerializeField] private float referenceSpeed = 10f;

    [Header("Keyboard")]
    [SerializeField] private InputActionReference steerAction;

    [Header("Pointer drag (mouse and touch)")]
    [SerializeField] private InputActionReference pressAction;
    [SerializeField] private InputActionReference dragAction;
    [SerializeField] private float dragSensitivity = 1.5f;

    private float currentSteer;

    public void SetForwardSpeed(float speed) => forwardSpeed = speed;

    private void OnEnable()
    {
        SetEnabled(steerAction, true);
        SetEnabled(pressAction, true);
        SetEnabled(dragAction, true);
    }

    private void OnDisable()
    {
        SetEnabled(steerAction, false);
        SetEnabled(pressAction, false);
        SetEnabled(dragAction, false);
    }

    private static void SetEnabled(InputActionReference reference, bool enabled)
    {
        if (reference == null) return;
        if (enabled) reference.action.Enable();
        else reference.action.Disable();
    }

    private void Update()
    {
        // Keyboard: eased left/right steering.
        float targetSteer = steerAction != null ? steerAction.action.ReadValue<float>() : 0f;
        currentSteer = Mathf.MoveTowards(currentSteer, targetSteer, steerAcceleration * Time.deltaTime);
        float steerScale = Mathf.Sqrt(forwardSpeed / referenceSpeed);
        float keyboardMove = currentSteer * steerSpeed * steerScale * Time.deltaTime;

        // Pointer: finger movement maps to road width, so it feels the same on any screen.
        float dragMove = 0f;
        if (pressAction != null && dragAction != null && pressAction.action.IsPressed())
        {
            float pixels = dragAction.action.ReadValue<Vector2>().x;
            dragMove = pixels / Screen.width * (halfWidth * 2f) * dragSensitivity;
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x + keyboardMove + dragMove, -halfWidth, halfWidth);
        pos.z += forwardSpeed * Time.deltaTime;
        transform.position = pos;
    }
}