using System.Xml.Serialization;

namespace DontFreeze.MapEditor
{
    public enum TileTypes { EMPTY, RIVER_STRAIGHT, RIVER_BRIDGE, RIVER_CURVE, RIVER_SOURCE, MOUNTAIN }
    
    public class Tile
    {
        [XmlAttribute("type")] public int type = 0;
        [XmlAttribute("isMirrored")] public bool isMirrored = false;
        [XmlAttribute("rotation")] public int rotation = 0;
    }
}
