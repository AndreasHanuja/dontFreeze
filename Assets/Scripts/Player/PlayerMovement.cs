using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float blockMovementUntil;

    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float currentMoveSpeed;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(blockMovementUntil - Time.realtimeSinceStartup > 0)
        {
            horizontal = 0;
            vertical = 0;
        }

        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;
        cameraDirection.Normalize();

        Vector3 cameraDirectionRight = Camera.main.transform.right;
        cameraDirectionRight.y = 0;
        cameraDirectionRight.Normalize();

        Vector3 direction = (cameraDirection * vertical + cameraDirectionRight * horizontal);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        currentMoveSpeed = direction.magnitude;
        // Animation transition should be inserted here
        if (direction.magnitude >= 0.1f)
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0.0f, targetangle, 0.0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out hit, 5, 1 << 8))
        {
            transform.GetChild(1).transform.localPosition = new Vector3(0, hit.point.y - 1f,0);
        }
        transform.position = new Vector3(transform.position.x,1, transform.position.z);
    }
}
