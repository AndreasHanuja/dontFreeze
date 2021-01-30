using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureTemplate : MonoBehaviour
{
    #region singelton
    public static TemperatureTemplate instance = null;

    private void Awake()
    {
        if(instance!= null)
        {
            Debug.Log("More than one instance of Temperature termplate");
        }

        instance = this;
    }

    #endregion  

    public float maxTemperature = 50.0f;

    public float currentTemperature = 50.0f;

    public List<GameObject> campFires;

    public Transform player;

    public float campFireThreshold = 5.0f;

    public bool isCampFireDown()
    {
        float minDistance = float.MaxValue;

        foreach(GameObject obj in campFires)
        {
            Vector3 offset = player.position - obj.transform.position;
            minDistance = Mathf.Min(minDistance, offset.sqrMagnitude);               
        }

        return minDistance < campFireThreshold * campFireThreshold;
    }

    public void DecreaseTemperature(float penalty)
    {
        currentTemperature = Mathf.Clamp(currentTemperature - penalty, 0, maxTemperature);
    }

    public void IncreaseTemperature(float bonus)
    {
        currentTemperature = Mathf.Clamp(currentTemperature + bonus, 0, maxTemperature);
    }
}
