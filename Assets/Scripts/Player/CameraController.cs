using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    public float height = 10f;

    public float distance = 20f;

    public float angle = 45f;

    public float smoothSpeed = 0.5f;
    Vector3 refSmoothSpeed;

    private bool isIncreasing;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle += (isIncreasing ? 1 : -1) * (rotationSpeed * Time.deltaTime * TemperatureManager.coldPercentage);

        if (!target)
        {
            return;
        }

        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);

        Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;

        Vector3 flatTargetPosition = target.position;
        flatTargetPosition.y = 0f;
        Vector3 finalPosition = flatTargetPosition + rotateVector;
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, angle, 0);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refSmoothSpeed, smoothSpeed);
        //transform.LookAt(flatTargetPosition);
    }

    private void FixedUpdate()
    {
        isIncreasing = Random.Range(0, 500) < 5 ? !isIncreasing : isIncreasing;
    }
}
