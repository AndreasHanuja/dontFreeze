using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopSound : MonoBehaviour
{
    public static bool playWolf;
    public static bool playChop;

    public AudioSource wolfAS;
    public AudioSource chopAS;

    // Update is called once per frame
    void Update()
    {
        playChopAS();
        playWolfAS();
    }

    void playChopAS()
    {
        if(playChop)
        {
            chopAS.Play();
            playChop = false;
        }
    }

    void playWolfAS()
    {
        if(playWolf)
        {
            wolfAS.Play();
            playWolf = false;
        }
    }
}
