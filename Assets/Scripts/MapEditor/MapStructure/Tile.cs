using System.Xml.Serialization;
using UnityEngine;

namespace DontFreeze.MapEditor
{
    public enum TileTypes { EMPTY, RIVER, MOUNTAIN }
    
    public class Tile
    {
        [XmlAttribute("type")] public int type = 0;
        [XmlAttribute("isMirrored")] public int mirrored = 0; // 0 none, 1 x, 2 y, 3 both
        [XmlAttribute("rotation")] public int rotation = 0;

        [XmlIgnore] public TileTypes tileType;
        [XmlIgnore] public int xPosition;
        [XmlIgnore] public int yPosition;
        [XmlIgnore] public GameObject gameObject;
    }
}
