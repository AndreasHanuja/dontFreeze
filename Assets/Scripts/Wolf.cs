using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    enum WolfStates { IDLE, CHAISE, CIRCLE, ATTACK, FEARFIRE}


    public Transform player;
    private WolfStates currentMode;
    private float circleStartTime;
    private bool circleClockwise;

    public List<GameObject> campFires;

    [Header("configuration stuff")]
    public float playerAggroDistance;
    public float circleDistance;
    public float circleDuration;
    public float fearFireDistance;

    private Vector3 currentTargetPoint;

    private float currentMoveSpeed;
    private float currentRotationSpeed;

    private void Update()
    {
        UpdateModes();
        //MoveForward();

        RotateTowardsPoint();
    }

    private void UpdateModes()
    {
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) > playerAggroDistance * playerAggroDistance)
        {
            currentMode = WolfStates.IDLE;
        }
        else
        {
            if(currentMode == WolfStates.IDLE)
            {
                currentMode = WolfStates.CHAISE;
                circleClockwise = Random.Range(0, 2) < 1;
            }
        }

        if (currentMode == WolfStates.CHAISE && Vector3.SqrMagnitude(player.transform.position - transform.position) < circleDistance * circleDistance)
        {
            currentMode = WolfStates.CIRCLE;
            circleStartTime = Time.timeSinceLevelLoad;
        }

        if (currentMode == WolfStates.CHAISE && Vector3.SqrMagnitude(player.transform.position - transform.position) < circleDistance * circleDistance)
        {
            currentMode = WolfStates.CIRCLE;
        }

        if(currentMode == WolfStates.CIRCLE && Time.timeSinceLevelLoad - circleStartTime > circleDuration)
        {
            currentMode = WolfStates.ATTACK;
        }

        float minDistance = float.MaxValue;
        foreach(GameObject g in campFires)
        {
            minDistance = Mathf.Min(minDistance, Vector3.SqrMagnitude(transform.position - g.transform.position));
        }
        if(minDistance< fearFireDistance * fearFireDistance)
        {
            currentMode = WolfStates.FEARFIRE;
        }
    }

    private void RotateTowardsPoint()
    {
        Vector3 difPoint = player.transform.position - transform.position;
        difPoint.y = 0;
        difPoint.Normalize();
        Debug.Log(Mathf.Asin(difPoint.x));
        Mathf.

    }
    private void MoveForward()
    {
        if(!Physics.Raycast(new Ray(transform.position, transform.forward), 2))
        {
            transform.position += transform.forward * currentMoveSpeed * Time.deltaTime;
        }
    }
}
