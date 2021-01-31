using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private int health = 3;
    public bool HitTree()
    {
        health--;
        StopAllCoroutines();
        StartCoroutine(hitTree());
        if(health == 0)
        {
            return true;
        }
        return false;
    }

    private IEnumerator hitTree()
    {
        Vector3 oldPosition = transform.position;
        if (health == 0)
        {
            for (float f = 0; f < 1; f += Time.deltaTime*1.5f)
            {
                if (f <0.75f)
                {
                    transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(90, 0,90), f * (1/0.75f));
                }
                else 
                {
                    transform.position = Vector3.Slerp(oldPosition, oldPosition+Vector3.down, (f - 0.75f) * 4);

                }              
                yield return new WaitForEndOfFrame();

            }
            
            Destroy(gameObject);
        }
        else {
            Vector2 targetRotation = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * 10;
            for (float f = 0; f < 1; f += Time.deltaTime * 1.5f)
            {
                if (f < 0.25f)
                {
                    transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(targetRotation.x, 0, targetRotation.y), f * 4);
                }
                else if (f < 0.5f)
                {
                    transform.rotation = Quaternion.Slerp(Quaternion.Euler(targetRotation.x, 0, targetRotation.y), Quaternion.Euler(-targetRotation.x / 2f, 0, -targetRotation.y / 2f), (f - 0.25f) * 4);

                }
                else if (f < 0.75f)
                {
                    transform.rotation = Quaternion.Slerp(Quaternion.Euler(-targetRotation.x / 2f, 0, -targetRotation.y / 2f), Quaternion.Euler(-targetRotation.x / 4f, 0, -targetRotation.y / 4f), (f - 0.5f) * 4);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(Quaternion.Euler(-targetRotation.x / 4f, 0, -targetRotation.y / 4f), Quaternion.identity, (f - 0.75f) * 4);
                }
                yield return new WaitForEndOfFrame();

            }
        }
    }
}
