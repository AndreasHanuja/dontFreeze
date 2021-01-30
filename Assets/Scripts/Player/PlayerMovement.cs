using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;
        cameraDirection.Normalize();

        Vector3 cameraDirectionRight = Camera.main.transform.right;
        cameraDirectionRight.y = 0;
        cameraDirectionRight.Normalize();

        Vector3 direction = (cameraDirection * vertical + cameraDirectionRight * horizontal).normalized;

        // Animation transition should be inserted here
        if(direction.magnitude >= 0.1f)
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0.0f, targetangle, 0.0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

    }
}
