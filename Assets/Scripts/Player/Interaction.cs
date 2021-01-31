using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
    private float blockInteractionUntil;

    public Transform player;
    public TreeList treeList;

    public float distanceToTreeThreshold = 3.0f;

    public float distanceToCampFireThreShold = 8.0f;

    public float campFireIncreaseRate = 1.0f;

    public float campFireBonus = 5.0f;

    public GameObject campFire;

    public Text twigCounter;

    public Button chopIcon;
    public Button campFireIcon;

    bool isChopTreeMode = false;

    GameObject instanciatedCampFire;



    TwigManager twigManager;
    TemperatureTemplate temperatureTemplate;

    // Start is called before the first frame update
    void Start()
    {
        twigManager = TwigManager.instance;
        temperatureTemplate = TemperatureTemplate.instance;

        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerAnimator = transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if a tree is close to you? -> set some boolean (isChopTreeMode) & exchange the Icon of the ability to chop tree/place campfire (if place campfire and you dont have wood grey the icon out)

        //if isChopTreeMode and Input.GetKeyDown(KeyCode.E) -> chopTree
        //else if not isChopTreeMode and Input.GetKeyDown(KeyCode.E) and you have wood -> PutDownCampFire
        twigCounter.text = twigManager.twigCount.ToString();

        if(blockInteractionUntil - Time.realtimeSinceStartup >0)
        {
            return;
        }
        float minSqrtDistanceToTree = float.MaxValue;


        for (int i = 0; i < treeList.treeList.Count; i++)
        {
            Vector3 offset = treeList.treeList[i] - player.position;
            minSqrtDistanceToTree = Mathf.Min(minSqrtDistanceToTree, offset.sqrMagnitude);
        }


        // Animation and UI manipulatin will happen in here
        if (minSqrtDistanceToTree < distanceToTreeThreshold * distanceToTreeThreshold)
        {
            //tree is close -> show tree chop icon
            isChopTreeMode = true;
            campFireIcon.gameObject.SetActive(false);
            chopIcon.gameObject.SetActive(true);
        }
        else
        {
            chopIcon.gameObject.SetActive(false);
            isChopTreeMode = false;
            campFireIcon.gameObject.SetActive(true);
            campFireIcon.interactable = twigManager.twigCount > 4;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isChopTreeMode)
            {
                ChopTree();
            }
            else
            {
                PutDownCampFire();
            }
        }


        //ChopTree();

        //PutDownCampFire();

    }

    void ChopTree()
    {

        float minSqrtDistanceToTree = float.MaxValue;
        int closestTreeIndex = -1;

        for (int i = 0; i < treeList.treeList.Count; i++)
        {
            Vector3 offset = treeList.treeList[i] - player.position;
            if (offset.sqrMagnitude < minSqrtDistanceToTree)
            {
                closestTreeIndex = i;
            }
            minSqrtDistanceToTree = Mathf.Min(minSqrtDistanceToTree, offset.sqrMagnitude);
        }

        if (closestTreeIndex != -1)
        {
            Vector3 nearestTree = treeList.treeList[closestTreeIndex];
            GameObject tree = treeList.trees[closestTreeIndex];

            playerMovement.blockMovementUntil = Time.realtimeSinceStartup + 1;
            blockInteractionUntil = Time.realtimeSinceStartup + 1;
            playerAnimator.SetTrigger("Chop");

            if(tree.GetComponent<Tree>().HitTree()){
                treeList.treeList.Remove(nearestTree);
                treeList.trees.Remove(tree);
            }
            twigManager.AddTwig();
        }

    }

    void PutDownCampFire()
    {

        if (twigManager.twigCount > 4)
        {
            Vector3 playerPosition = transform.position + (transform.forward * 2.25f);
            playerPosition.y = 0.5f;

            //Update UI for twig
            blockInteractionUntil = Time.realtimeSinceStartup + 1;
            twigManager.twigCount -= 5;
            instanciatedCampFire = Instantiate(campFire, playerPosition, transform.rotation);
            temperatureTemplate.campFires.Add(instanciatedCampFire);
            StartCoroutine(AnimatedSpawnCampfire(instanciatedCampFire, playerPosition));

        }

    }

    IEnumerator AnimatedSpawnCampfire(GameObject campfire, Vector3 campfirePos)
    {
        for(float f = 0; f < 1; f += Time.deltaTime)
        {
            campfire.transform.localScale = Vector3.Slerp(Vector3.zero, Vector3.one, f);
            campfire.transform.position = Vector3.Slerp(transform.position, campfirePos, f);
            yield return new WaitForEndOfFrame();
        }
        campfire.transform.localScale = Vector3.Slerp(Vector3.zero, Vector3.one, 1);
        campfire.transform.position = Vector3.Slerp(transform.position, campfirePos, 1);
    }

}
