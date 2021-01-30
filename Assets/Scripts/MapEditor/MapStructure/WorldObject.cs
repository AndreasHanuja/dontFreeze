using System.Xml.Serialization;
using UnityEngine;

namespace DontFreeze.MapEditor
{   
    public class WorldObject
    {
        [XmlAttribute("x")] public float x = 0;
        [XmlAttribute("y")] public float y = 0;
        [XmlAttribute("z")] public float z = 0;
    }
}
