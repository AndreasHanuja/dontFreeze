using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace DontFreeze.MapEditor
{   
    public class WorldObjectList
    {
        [XmlAttribute("type")] public int type;
        [XmlArray("WorldObjectLists")]
        [XmlArrayItem("WorldObject")]
        public List<WorldObject> worldObjects = new List<WorldObject>();
    }
}
