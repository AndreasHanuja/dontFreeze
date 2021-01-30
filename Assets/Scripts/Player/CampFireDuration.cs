using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFireDuration : MonoBehaviour
{
    public GameObject campFireOut;

    TemperatureTemplate temperatureTemplate;

    public float campDuration = 5.0f;

    private void Start()
    {
        temperatureTemplate = TemperatureTemplate.instance;
    }

    private void Update()
    {
        StartCoroutine(TurnOffCamp());
    }


    IEnumerator TurnOffCamp()
    {

        yield return new WaitForSeconds(campDuration);
        Instantiate(campFireOut, transform.position, transform.rotation);
        temperatureTemplate.campFires.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
