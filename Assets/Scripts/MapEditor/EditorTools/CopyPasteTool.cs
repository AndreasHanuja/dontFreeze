using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontFreeze.MapEditor.EditorTools
{
    public class CopyPasteTool : AEditorTool
    {
        public GameObject selectionCube;
        public GameObject copyCube;

        private Vector3 startPos;
        private bool rightDown;
        private bool leftDown;

        private Tile pointA;
        private Tile pointB;

        public override void OnDeselect()
        {
            selectionCube.SetActive(false);
            copyCube.SetActive(false);
        }

        public override void OnLeftDown()
        {
            startPos = CurrentWorldPos();
            selectionCube.transform.localPosition = startPos;
            selectionCube.transform.localScale = new Vector3(10, 1, 10);
            leftDown = true;
            pointA = GetTile(toolManager.lastHitPosition);
            pointB = null;
        }

        public override void OnLeftUp()
        {
            leftDown = false;
            pointB = GetTile(toolManager.lastHitPosition);
        }

        public override void OnRightDown()
        {
            rightDown = true;
            copyCube.SetActive(true);
        }

        public override void OnRightUp()
        {
            rightDown = false;
            copyCube.SetActive(false);
            toolManager.mapManager.CopyArea(pointA, pointB, GetTile(toolManager.lastHitPosition));
        }

        private void Update()
        {
            if (leftDown)
            {
                Vector3 currentPos = CurrentWorldPos();
                selectionCube.transform.localPosition = (startPos + currentPos)/2 + new Vector3(5, 1, 5);
                selectionCube.transform.localScale = currentPos - startPos + new Vector3(10, 1, 10);
            }

            if (rightDown)
            {
                Vector3 currentPos = CurrentWorldPos();
                copyCube.transform.localScale = selectionCube.transform.localScale;
                copyCube.transform.localPosition = currentPos + new Vector3(copyCube.transform.localScale.x/2, 0, copyCube.transform.localScale.z / 2);
            }
        }

        private Vector3 CurrentWorldPos()
        {
            return new Vector3(toolManager.lastHitPosition.Item1 * 10, 0, toolManager.lastHitPosition.Item2 * 10);
        }
        public override void OnSelect()
        {
            selectionCube.SetActive(true);
            copyCube.SetActive(true);
        }
    }
}