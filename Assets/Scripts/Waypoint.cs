using DontFreeze.MapEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private bool isRotatedCorrectly = false;

    void Update()
    {
        if(MapGenerator.house != null && isRotatedCorrectly)
        {
            //transform.position -> this waypoint
            //MapGenerator.house.transform.position

            //magicaly set the rotation (maybe clamped to 45 degrees)
        }
    }
}
