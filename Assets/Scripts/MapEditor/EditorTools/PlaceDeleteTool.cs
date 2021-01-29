using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DontFreeze.MapEditor.EditorTools
{
    public class PlaceDeleteTool : AEditorTool
    {
        public Dropdown placeType;

        private bool leftDown;
        private bool rightDown;

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
                Tile currentTile = GetTile(toolManager.lastHitPosition);
                currentTile.type = placeType.value;
            }
            else if (rightDown)
            {
                Tile currentTile = GetTile(toolManager.lastHitPosition);
                currentTile.type = 0;
            }
        }

        public override void OnSelect()
        {
            placeType.gameObject.SetActive(true);
        }
    }
}