using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera _playerCamera;
    private CharacterController _controller;
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private void Start()
    {
        _playerCamera = Camera.main;
        _controller = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        var grounded = _controller.isGrounded;
        if (grounded && _currentVelocity.y < 0)
        {
            _currentVelocity.y = 0f;
        }

        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && grounded)
        {
            _currentVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        _currentVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_currentVelocity * Time.deltaTime);
        _playerCamera.transform.position =
            new Vector3(transform.position.x, transform.position.y + 20, transform.position.z - 15);
    }
}
