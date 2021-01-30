using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace DontFreeze.MapEditor
{
    public class MapManager : MonoBehaviour
    {
        public Map map;
        public EditorUIManager editorUIManager;
        public ToolManager toolManager;

        private void Start()
        {
        }
        public void SaveMap()
        {
            var serializer = new XmlSerializer(typeof(Map));
            var stream = new FileStream(Application.dataPath + "/Resources/Maps/"+ editorUIManager.nameInputField.text + ".xml", FileMode.Create);
            serializer.Serialize(stream, map);
            stream.Close();
        }

        public void LoadMap()
        {
            var serializer = new XmlSerializer(typeof(Map));
            var stream = new FileStream(Application.dataPath + "/Resources/Maps/" + editorUIManager.nameInputField.text + ".xml", FileMode.Open);
            map = serializer.Deserialize(stream) as Map;
            stream.Close();
        }

        public bool FileExist(string mapName)
        {
            return System.IO.File.Exists(Application.dataPath + "/Resources/Maps/" + mapName + ".xml");
        }
        public void CreateNewMap()
        {
            map = new Map();
            map.name = editorUIManager.newMapName.text;
            map.width = int.Parse(editorUIManager.newMapWidth.text);
            map.height = int.Parse(editorUIManager.newMapHeight.text);

            map.tiles = new List<Tile>();
            for(int i=0; i<map.width * map.height; i++)
            {
                map.tiles.Add(new Tile());
            }

            toolManager.groundPlate.localPosition = new Vector3(map.width*5, 0, map.height*5);
            toolManager.groundPlate.localScale = new Vector3(map.width, 1, map.height);
            toolManager.groundPlate.GetComponent<Renderer>().material.mainTextureScale = new Vector2(map.width/2f, map.height/2f);
        }

        public Tile[] GetNeighbors(Tile t)
        {
            Tile[] output = new Tile[4];
            if (t.yPosition < map.height - 1)
            {
                output[0] = map.tiles[t.xPosition + (t.yPosition + 1) * map.width];
            }
            else
            {
                output[0] = null;
            }

            if (t.xPosition > 0)
            {
                output[1] = map.tiles[t.xPosition - 1 + t.yPosition * map.width];
            }
            else
            {
                output[1] = null;
            }

            if (t.yPosition > 0)
            {
                output[2] = map.tiles[t.xPosition + (t.yPosition - 1) * map.width];
            }
            else
            {
                output[2] = null;
            }

            if (t.xPosition < map.width - 1)
            {
                output[3] = map.tiles[t.xPosition + 1 + t.yPosition * map.width];
            }
            else
            {
                output[3] = null;
            }
            return output;
        }
        public void UpdateTile(Tile t)
        {
            Tile[] neighbors = GetNeighbors(t);

            //magic happens here!
        }

        public void CopyArea(int xMin, int yMin, int xMax, int yMax, int xPos, int yPos)
        {

        }
    }
}