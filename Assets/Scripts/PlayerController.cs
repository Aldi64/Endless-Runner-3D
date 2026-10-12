using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float referenceSpeed = 10f;
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float steerSpeed = 8f;
    [SerializeField] private float steerAcceleration = 12f;
    [SerializeField] private float halfWidth = 3.5f;

    [Header("Input")]
    [SerializeField] private InputActionReference steerAction;

    private float currentSteer;

    public void SetForwardSpeed(float speed) => forwardSpeed = speed;

    private void OnEnable() => steerAction.action.Enable();
    private void OnDisable() => steerAction.action.Disable();

    private void Update()
    {
        float targetSteer = steerAction.action.ReadValue<float>();
        currentSteer = Mathf.MoveTowards(currentSteer, targetSteer, steerAcceleration * Time.deltaTime);

        Vector3 pos = transform.position;
        float steerScale = Mathf.Sqrt(forwardSpeed / referenceSpeed);
        pos.x = Mathf.Clamp(pos.x + currentSteer * steerSpeed * steerScale * Time.deltaTime, -halfWidth, halfWidth);
        pos.z += forwardSpeed * Time.deltaTime;
        transform.position = pos;
    }
}