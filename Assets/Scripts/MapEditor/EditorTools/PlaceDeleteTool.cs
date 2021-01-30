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
            bool isUpdated = false;
            if (leftDown)
            {
                Tile currentTile = GetTile(toolManager.lastHitPosition);
                if(currentTile.tileType != (TileTypes)(placeType.value + 1))
                {
                    currentTile.tileType = (TileTypes)(placeType.value + 1);
                    isUpdated = true;
                }
            }
            else if (rightDown)
            {
                Tile currentTile = GetTile(toolManager.lastHitPosition);
                if (currentTile.tileType != TileTypes.EMPTY)
                {
                    currentTile.tileType = TileTypes.EMPTY;
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                Tile currentTile = GetTile(toolManager.lastHitPosition);
                toolManager.mapManager.UpdateTile(currentTile);
                foreach (Tile t in toolManager.mapManager.GetNeighbors(currentTile))
                {
                    if(t != null)
                    {
                        toolManager.mapManager.UpdateTile(t);
                    }
                }
            }
        }

        public override void OnSelect()
        {
            placeType.gameObject.SetActive(true);
        }
    }
}