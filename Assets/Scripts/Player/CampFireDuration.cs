using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFireDuration : MonoBehaviour
{
    TemperatureTemplate temperatureTemplate;

    public float campDuration = 5.0f;

    private void Start()
    {
        temperatureTemplate = TemperatureTemplate.instance;
        StartCoroutine(TurnOffCamp());
    }


    IEnumerator TurnOffCamp()
    {
        float range = 10;
        for(float f = 0; f<campDuration; f += 0.25f)
        {
            transform.GetChild(2).GetComponent<Light>().intensity = Random.Range(0.7f, 1.2f);
            transform.GetChild(3).GetComponent<Light>().intensity = Random.Range(4f, 6f);
            yield return new WaitForSeconds(0.25f);
        }
        for (float f = 0; f < 1; f += Time.deltaTime )
        {
            transform.GetChild(2).GetComponent<Light>().intensity = Mathf.Lerp(1,0,f);
            transform.GetChild(3).GetComponent<Light>().intensity = Mathf.Lerp(5, 0, f);
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);

        temperatureTemplate.campFires.Remove(this.gameObject);
    }
}
