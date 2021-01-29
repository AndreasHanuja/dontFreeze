using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DontFreeze.MapEditor
{
    public class ToolManager : MonoBehaviour
    {
        public Transform groundPlate;
        public List<AEditorTool> editorTools;
        public LayerMask groundMask;
        public MapManager mapManager;
        public Dropdown toolDropdown;

        public Tuple<int, int> lastHitPosition;
        private int currentTool = 0;

        private void Start()
        {
            editorTools[0].OnSelect();
        }
        public void UpdateTool()
        {
            editorTools[currentTool].OnDeselect();
            editorTools[toolDropdown.value].OnSelect();
            currentTool = toolDropdown.value;
        }

        private void Update()
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(r, out hit, Mathf.Infinity, groundMask)){
                lastHitPosition = Tuple.Create((int)hit.point.x / 10, (int)hit.point.z / 10);

                if (Input.GetMouseButtonDown(0))
                {
                    editorTools[currentTool].OnLeftDown();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    editorTools[currentTool].OnLeftUp();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    editorTools[currentTool].OnRightDown();
                }
                if (Input.GetMouseButtonUp(1))
                {
                    editorTools[currentTool].OnRightUp();
                }
            }
        }
    }
}