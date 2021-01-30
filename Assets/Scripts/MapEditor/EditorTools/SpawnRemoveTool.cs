using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DontFreeze.MapEditor.EditorTools
{
    public class SpawnRemoveTool : AEditorTool
    {
        public LayerMask objectMask;

        public Dropdown placeType;

        private bool leftDown;
        private bool rightDown;

        public GameObject[] prefabs;

        private Vector3 lastPlacedPosition;

        public override void OnDeselect()
        {
            placeType.gameObject.SetActive(false);
        }

        public override void OnLeftDown()
        {
            leftDown = true;
        }

        public override void OnLeftUp()
        {
            leftDown = false;
        }

        public override void OnRightDown()
        {
            rightDown = true;
        }

        public override void OnRightUp()
        {
            rightDown = false;
        }

        private void Update()
        {
            if (leftDown)
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, Mathf.Infinity, toolManager.groundMask) && EventSystem.current.currentSelectedGameObject == null)
                {
                    if(Vector3.SqrMagnitude(hit.point - lastPlacedPosition) > 16)
                    {
                        lastPlacedPosition = hit.point;
                        GameObject g = GameObject.Instantiate(prefabs[placeType.value], toolManager.mapManager.tileInstanceParent);
                        g.transform.position = hit.point;
                        WorldObject worldObject = new WorldObject();
                        g.GetComponentInChildren<HackyObjectReference>().worldObject = worldObject;
                        worldObject.x = lastPlacedPosition.x;
                        worldObject.y = lastPlacedPosition.y;
                        worldObject.z = lastPlacedPosition.z;
                        toolManager.mapManager.map.worldObjectLists[placeType.value].worldObjects.Add(worldObject);
                    }
                }
            }
            else if (rightDown)
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, Mathf.Infinity, objectMask) && EventSystem.current.currentSelectedGameObject == null)
                {
                    WorldObject worldObject = hit.collider.gameObject.GetComponent<HackyObjectReference>().worldObject;
                    toolManager.mapManager.map.worldObjectLists[hit.collider.gameObject.GetComponent<HackyObjectReference>().type].worldObjects.Remove(worldObject);


                    Destroy(hit.collider.gameObject.transform.parent);
                }
            }
        }

        public override void OnSelect()
        {
            placeType.gameObject.SetActive(true);
        }
    }
}