using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeList : MonoBehaviour
{
    public List<Vector3> treeList;
    public List<GameObject> trees;
    private void Start()
    {
        GameObject[] tempTrees;   
        tempTrees = GameObject.FindGameObjectsWithTag("Tree");
              
        for(int i = 0; i < tempTrees.Length; i++)
        {            
            treeList.Add(tempTrees[i].transform.position);
            trees.Add(tempTrees[i]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
