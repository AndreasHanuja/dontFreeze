using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DontFreeze.MapEditor.EditorTools
{
    public class BridgeWaterTool : AEditorTool
    {
        private bool leftDown;
        private bool rightDown;

        private Vector3 lastPlacedPosition;

        public override void OnDeselect()
        {
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
                Tile t = GetTile(toolManager.lastHitPosition);
                if(t.type == 5)
                {
                    t.type = 8;
                    toolManager.mapManager.UpdateTile(t);
                }
            }
            else if (rightDown)
            {
                Tile t = GetTile(toolManager.lastHitPosition);
                if (t.type == 8)
                {
                    t.type = 5;
                    toolManager.mapManager.UpdateTile(t);
                }
            }
        }

        public override void OnSelect()
        {
        }
    }
}