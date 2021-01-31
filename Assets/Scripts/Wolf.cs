using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public enum WolfStates { IDLE, CHAISE, CIRCLE, ATTACK, FEARFIRE}


    public WolfStates currentMode;
    private float moveStartTime;
    private float circleStartTime;
    private bool circleClockwise;
    private Vector3 spawnPoint;

    public Transform player;

    public List<GameObject> campFires;

    [Header("configuration stuff")]
    public float playerAggroDistance;
    public float circleDistance;
    public float circleDuration;
    public float fearFireDistance;
    public float maxMoveTime;
    public float maxIdleDistance;
    public float looseAgroDistance;

    private Vector3 currentTargetPoint;

    public float walkSpeed;
    public float runSpeed;
    private float currentMoveSpeed;
    public float currentRotationSpeed;
    public Animator animator;

    private void Start() {
        spawnPoint = transform.position;
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        WolfStates oldState = currentMode;
        UpdateModes();
        if (currentMode != oldState)
        {
            GiveMoveCommand();
        }

        if(Time.realtimeSinceStartup - moveStartTime > maxMoveTime)
        {
            GiveMoveCommand();
        }

        if(Vector3.Magnitude(currentTargetPoint - transform.position) < 1)
        {
            GiveMoveCommand();
        }

        RotateTowardsPoint();
        MoveForward();
    }

    private void UpdateModes()
    {
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) > looseAgroDistance * looseAgroDistance)
        {
            currentMoveSpeed = walkSpeed;
            animator.SetBool("walk", true);
            currentMode = WolfStates.IDLE;
        }
            
        if (currentMode == WolfStates.IDLE && Vector3.SqrMagnitude(player.transform.position - transform.position) < playerAggroDistance * playerAggroDistance)
        {
            currentMoveSpeed = runSpeed;
            animator.SetBool("walk", false);
            //TODO play sound
            currentMode = WolfStates.CHAISE;
        }        

        if (currentMode == WolfStates.CHAISE && Vector3.SqrMagnitude(player.transform.position - transform.position) < circleDistance * circleDistance)
        {
            currentMoveSpeed = runSpeed;
            animator.SetBool("walk", false);

            currentMode = WolfStates.CIRCLE;
            circleStartTime = Time.timeSinceLevelLoad;
            circleClockwise = Random.Range(0, 2) < 1;
        }

        if (currentMode == WolfStates.CHAISE && Vector3.SqrMagnitude(player.transform.position - transform.position) < circleDistance * circleDistance)
        {
            currentMoveSpeed = runSpeed; animator.SetBool("walk", false);

            currentMode = WolfStates.CIRCLE;
        }

        if(currentMode == WolfStates.CIRCLE && Time.timeSinceLevelLoad - circleStartTime > circleDuration)
        {
            currentMoveSpeed = runSpeed; animator.SetBool("walk", false);

            currentMode = WolfStates.ATTACK;
        }

        float minDistance = float.MaxValue;
        foreach(GameObject g in TemperatureTemplate.instance.campFires)
        {
            minDistance = Mathf.Min(minDistance, Vector3.SqrMagnitude(transform.position - g.transform.position));
        }
        if(minDistance< fearFireDistance * fearFireDistance)
        {
            currentMoveSpeed = runSpeed;
            currentMode = WolfStates.FEARFIRE; animator.SetBool("walk", false);

        }
    }

    private void GiveMoveCommand()
    {
        moveStartTime = Time.realtimeSinceStartup;

        switch (currentMode)
        {
            case WolfStates.IDLE:
                currentTargetPoint = spawnPoint + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * Random.Range(0, maxIdleDistance);
                break;
            case WolfStates.CHAISE:
                currentTargetPoint = player.transform.position;
                break;
            case WolfStates.CIRCLE:
                //currentTargetPoint = player.transform.position;
                Vector3 difPoint = player.transform.position-transform.position;
                difPoint.y = 0;
                difPoint.Normalize();
                //transform.for
                float goalRotation = Mathf.Atan2(difPoint.z, difPoint.x) * Mathf.Rad2Deg + (circleClockwise?-10:10);
                Vector3 goalPosition = new Vector3(Mathf.Cos(goalRotation * Mathf.Deg2Rad), 0, Mathf.Sin(goalRotation * Mathf.Deg2Rad));
                currentTargetPoint = player.transform.position - goalPosition * circleDistance;
                break;
            case WolfStates.ATTACK:
                //currentTargetPoint = player.transform.position;
                currentTargetPoint = player.transform.position;
                break;
            case WolfStates.FEARFIRE:
                float minDistance = float.MaxValue;
                GameObject minCampfire = null;
                foreach (GameObject g in TemperatureTemplate.instance.campFires)
                {
                    if (Vector3.SqrMagnitude(transform.position - g.transform.position) < minDistance)
                    {
                        minDistance = Vector3.SqrMagnitude(transform.position - g.transform.position);
                        minCampfire = g;
                    }
                }
                currentTargetPoint = transform.position + (transform.position - minCampfire.transform.position);
                spawnPoint = minCampfire.transform.position;
                break;
        }
    }

    private void RotateTowardsPoint()
    {
        Vector3 difPoint = currentTargetPoint - transform.position;
        difPoint.y = 0;
        difPoint.Normalize();
        //transform.for
        float goalRotation = Mathf.Atan2(difPoint.x, difPoint.z) * Mathf.Rad2Deg + 360;
        goalRotation = (360+(goalRotation - transform.rotation.eulerAngles.y))%360;

        if(goalRotation > 180)
        {
            goalRotation = goalRotation - 360;
        }

        float targetY = Mathf.Clamp(goalRotation, -currentRotationSpeed * Time.deltaTime, currentRotationSpeed * Time.deltaTime);
        transform.Rotate(new Vector3(0,targetY,0),Space.World);

    }
    private void MoveForward()
    {
        if(!Physics.Raycast(new Ray(transform.position, transform.forward), 1.2f))
        {
            transform.position += transform.forward * currentMoveSpeed * Time.deltaTime;
        }
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) < 4)
        {
            Interaction.wolfCatchedPlayer = true;
        }
    }
}
