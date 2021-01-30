using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform player;
    public TreeList treeList;

    public float distanceThreshold = 3.0f;

    TwigManager twigManager;
    // Start is called before the first frame update
    void Start()
    {
        twigManager = TwigManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        ChopTree();

        PutDownCampFire();
    }

    void ChopTree()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < treeList.treeList.Count; i++)
            {
                Vector3 offset = treeList.treeList[i] - player.position;

                if (offset.magnitude < distanceThreshold)
                {
                    Debug.Log("tree is close");
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
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(twigManager.twigCount == 0)
            {
                Debug.Log("You have No Wood!!");
            }

            else
            {
                twigManager.RemoveTwig();
            }
        }
    }
}
