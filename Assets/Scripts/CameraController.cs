using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraControlActions cameraActions;
    private InputAction movement;
    private Transform cameraTransform;
    public GameObject center;

    [SerializeField]
    private float maxSpeed = 5f;
    private float speed;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    [SerializeField]
    private float minZoom = 20f;
    [SerializeField]
    private float maxZoom = 40f;
    [SerializeField]
    private float ultimateZoom = 60f;

    //value set in various functions 
    //used to update the position of the camera base object.
    private Vector3 targetPosition;

    private float zoomHeight;

    //used to track and maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    private void Awake()
    {
        cameraActions = new CameraControlActions();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        //zoomHeight = cameraTransform.localPosition.y;

        lastPosition = this.transform.position;

        movement = cameraActions.Camera.Movement;
        cameraActions.Camera.Enable();
    }

    private void OnDisable()
    {
        cameraActions.Camera.Disable();
    }

    private void Update()
    {
        //inputs
        GetKeyboardMovement();

        //move base and camera objects
        UpdateVelocity();
        UpdateBasePosition();

        cameraTransform.LookAt(center.transform);
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0f;
        lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
                    + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
            targetPosition += inputValue;
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            //create a ramp up or acceleration
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            //create smooth slow down
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        //reset for next frame
        targetPosition = Vector3.zero;
    }

    //gets the horizontal forward vector of the camera
    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;

        zoomHeight = Vector3.Distance(center.transform.position, transform.position);

        if (zoomHeight < minZoom)
        {
            this.transform.position -= forward.normalized / 10f;

            forward.y = 0f;
            forward.x = 0f;
            forward.z = 0f;
        }
        else if (zoomHeight > maxZoom)
        {
            this.transform.position += forward.normalized / 100f;

            if(zoomHeight > ultimateZoom)
            {
                forward.y = 0f;
                forward.x = 0f;
                forward.z = 0f;
            }
        }

        return forward;
    }

    //gets the horizontal right vector of the camera
    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }
}
