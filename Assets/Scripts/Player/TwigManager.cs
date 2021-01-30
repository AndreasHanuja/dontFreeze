using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwigManager : MonoBehaviour
{
    #region Singleton
    public static TwigManager instance = null;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More that one instance of Twig manager");
        }

        instance = this;
    }

    #endregion

    public int twigCount = 0;

    public void AddTwig()
    {
        twigCount++;
    }

    public void RemoveTwig()
    {
        twigCount--;
    }
}
