using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureManager : MonoBehaviour
{
    TemperatureTemplate temperatureTemplate;

    public float temperaturePenalty = 2.5f;

    public float minimumTemp = 0.0f;

    public float warningTempThreshold = 20.0f;

    public float timeRate = 2.5f;

    public float bonusIncreaseRate = 5.0f;

    IEnumerator Start()
    {
        temperatureTemplate = TemperatureTemplate.instance;

        while (true)
        {
            yield return new WaitForSeconds(timeRate);
            if (!temperatureTemplate.isCampFireDown())
            {
                DecreaseTemperature();

                //Enable warning signal for UI?
                if(temperatureTemplate.currentTemperature < warningTempThreshold)
                {
                    Debug.Log("Warning");
                }

                // Gameover 
                if(temperatureTemplate.currentTemperature <= minimumTemp )
                {

                }
            }
            else
            {
                IncreaseTemperature();
            }

           
        }
    }
        

    void DecreaseTemperature()
    {
        temperatureTemplate.DecreaseTemperature(temperaturePenalty);
        Debug.Log(temperatureTemplate.currentTemperature);
    }

    void IncreaseTemperature()
    {
        temperatureTemplate.IncreaseTemperature(bonusIncreaseRate);
        Debug.Log(temperatureTemplate.currentTemperature);

    }
}
