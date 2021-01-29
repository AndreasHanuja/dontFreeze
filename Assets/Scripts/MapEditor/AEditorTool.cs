using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontFreeze.MapEditor
{
    public abstract class AEditorTool : MonoBehaviour
    {
        public ToolManager toolManager;
        public abstract void OnLeftDown();
        public abstract void OnLeftUp();
        public abstract void OnRightDown();
        public abstract void OnRightUp();
        public abstract void OnSelect();
        public abstract void OnDeselect();

        protected Tile GetTile(Tuple<int,int> position)
        {
            return toolManager.mapManager.map.tiles[position.Item1 + position.Item2 * toolManager.mapManager.map.width];
        }
    }
}