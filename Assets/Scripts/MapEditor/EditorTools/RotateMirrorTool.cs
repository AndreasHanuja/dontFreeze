using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontFreeze.MapEditor.EditorTools
{
    public class RotateMirrorTool : AEditorTool
    {
        public override void OnDeselect()
        {
        }

        public override void OnLeftDown()
        {
            Tile currentTile = GetTile(toolManager.lastHitPosition);
            currentTile.rotation = (currentTile.rotation + 1) % 4;
        }

        public override void OnLeftUp()
        {
        }

        public override void OnRightDown()
        {
            Tile currentTile = GetTile(toolManager.lastHitPosition);
            currentTile.isMirrored = !currentTile.isMirrored;
        }

        public override void OnRightUp()
        {
        }

        public override void OnSelect()
        {
        }
    }
}