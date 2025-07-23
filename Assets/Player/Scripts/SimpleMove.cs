using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] private InputActionReference moveReference;
    private InputAction moveAction;
    [SerializeField] private float speed = 1f;

    private void Start()
    {
        moveAction = moveReference.action;
        moveAction.Enable();
    }

    private void Update()
    {
        if (Application.isFocused)
        {
            var move = moveAction.ReadValue<Vector2>() * Time.deltaTime * speed;
            transform.localPosition += new Vector3(move.x, 0f, move.y);
        }
    }
}
