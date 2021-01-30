using System.Xml.Serialization;

namespace DontFreeze.MapEditor
{
    public enum TileTypes { EMPTY, RIVER, MOUNTAIN }
    
    public class Tile
    {
        [XmlAttribute("type")] public int type = 0;
        [XmlAttribute("isMirrored")] public bool isMirrored = false;
        [XmlAttribute("rotation")] public int rotation = 0;

        public TileTypes tileType;
        public int xPosition;
        public int yPosition;
    }
}
