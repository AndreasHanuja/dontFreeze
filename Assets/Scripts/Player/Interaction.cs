using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform player;
    public TreeList treeList;

    public float distanceToTreeThreshold = 3.0f;

    public float distanceToCampFireThreShold = 8.0f;

    public float campFireIncreaseRate = 1.0f;

    public float campFireBonus = 5.0f;

    public GameObject campFire;

    GameObject instanciatedCampFire;

    TwigManager twigManager;
    TemperatureTemplate temperatureTemplate;

    // Start is called before the first frame update
    void Start()
    {
        twigManager = TwigManager.instance;
        temperatureTemplate = TemperatureTemplate.instance;

              
    }

    // Update is called once per frame
    void Update()
    {
        //check if a tree is close to you? -> set some boolean (isChopTreeMode) & exchange the Icon of the ability to chop tree/place campfire (if place campfire and you dont have wood grey the icon out)

        //if isChopTreeMode and Input.GetKeyDown(KeyCode.E) -> chopTree
        //else if not isChopTreeMode and Input.GetKeyDown(KeyCode.E) and you have wood -> PutDownCampFire


        ChopTree();

        PutDownCampFire();
              
    }

    void ChopTree()
    {
        for (int i = 0; i < treeList.treeList.Count; i++)
        {
            Vector3 offset = treeList.treeList[i] - player.position;

            // Animation and UI manipulatin will happen in here
            if (offset.sqrMagnitude < distanceToTreeThreshold * distanceToTreeThreshold)
            {

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Vector3 nearestTree = treeList.treeList[i];
                    GameObject tree = treeList.trees[i];

                    treeList.treeList.Remove(nearestTree);
                    treeList.trees.Remove(tree);

                    Destroy(tree);

                    twigManager.AddTwig();
                    Debug.Log(twigManager.twigCount);
                }
            }
        }

    }

    void PutDownCampFire()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (twigManager.twigCount == 0)
            {
                Debug.Log("You have No Wood!!");
            }
            else
            {
                Vector3 playerPosition = transform.position + (transform.forward * 2);
                playerPosition.y = 0.5f;

                //Update UI for twig
                twigManager.RemoveTwig();
                instanciatedCampFire = Instantiate(campFire, playerPosition, transform.rotation);
                temperatureTemplate.campFires.Add(instanciatedCampFire);
            }
        }
    }

 
}
