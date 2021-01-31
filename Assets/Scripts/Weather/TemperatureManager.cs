using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureManager : MonoBehaviour
{
    TemperatureTemplate temperatureTemplate;

    public float temperaturePenalty = 2.5f;

    public float minimumTemp = 0.0f;

    public float warningTempThreshold = 20.0f;

    public float timeRate = 2.5f;

    public float bonusIncreaseRate = 5.0f;

    public GameObject gameOverUI;

    public Image maskImage;

    public Image fillImage;

    IEnumerator Start()
    {
        temperatureTemplate = TemperatureTemplate.instance;

        while (true)
        {
            yield return new WaitForSeconds(timeRate);

            SetFillAmount();

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
                    gameOverUI.SetActive(true);
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

    void SetFillAmount()
    {
        float fillImageAmount = temperatureTemplate.currentTemperature / temperatureTemplate.maxTemperature;

        fillImage.fillAmount = fillImageAmount;
    }
}
