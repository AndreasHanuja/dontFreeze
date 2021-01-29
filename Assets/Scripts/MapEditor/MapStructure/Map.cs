using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace DontFreeze.MapEditor
{
    [XmlRoot("Map")]
    public class Map 
    {
        [XmlAttribute("name")] public string name;
        [XmlAttribute("width")] public int width;
        [XmlAttribute("height")] public int height;

        [XmlArray("Tiles")]
        [XmlArrayItem("Tile")]
        public List<Tile> tiles = new List<Tile>();
    }
}