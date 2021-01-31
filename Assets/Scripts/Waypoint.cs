using DontFreeze.MapEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private bool isRotatedCorrectly = false;

    Vector3 offsetPoisiton; 

    private void Awake()
    {
        offsetPoisiton = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
    }

    void Update()
    {
        
        if (MapGenerator.house != null && isRotatedCorrectly)
        {
            Vector3 newHouseOffsetPosition = MapGenerator.house.transform.position + offsetPoisiton;
            Quaternion rotationAngle = Quaternion.LookRotation(newHouseOffsetPosition - transform.position);
            transform.rotation = rotationAngle;
                  
        }

        isRotatedCorrectly = true;
    }
}
