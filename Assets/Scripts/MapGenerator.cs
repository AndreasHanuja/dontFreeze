using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace DontFreeze.MapEditor
{
    public class MapGenerator : MonoBehaviour
    {
        private Map map;

        public GameObject[] tilePrefabs;
        public GameObject[] objectPrefabs;
        public int[] instantiationAmmount;

        public Transform tileInstanceParent;

        public string mapName;

        private void Start()
        {
            LoadMap();
        }

        public void LoadMap()
        {
            var serializer = new XmlSerializer(typeof(Map));
            var stream = new FileStream(Application.dataPath + "/Resources/Maps/" + mapName + ".xml", FileMode.Open);
            map = serializer.Deserialize(stream) as Map;
            stream.Close();

            InitMap();
        }

        private void CreateTile(Tile t)
        {
            if (t.gameObject != null)
            {
                Destroy(t.gameObject);
            }
            t.gameObject = GameObject.Instantiate(tilePrefabs[t.type], tileInstanceParent);

            if (t.type == 0)
            {
                t.tileType = TileTypes.EMPTY;
            }
            else if (t.type == 1)
            {
                t.tileType = TileTypes.MOUNTAIN;
            }
            else
            {
                t.tileType = TileTypes.RIVER;
            }

            t.gameObject.transform.localPosition = new Vector3(10 * t.xPosition + 5, 0, 10 * t.yPosition + 5);
            t.gameObject.transform.localRotation = Quaternion.Euler(-90, t.rotation * 90, 0);
            t.gameObject.transform.localScale = new Vector3(t.mirrored % 3 == 1 ? -100 : 100, t.mirrored / 2 == 1 ? -100 : 100, 100);
        }

        private void InitMap()
        {
            foreach (Transform child in tileInstanceParent)
            {
                Destroy(child);
            }
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.width; y++)
                {
                    Tile t = map.tiles[x + y * map.width];
                    t.xPosition = x;
                    t.yPosition = y;
                    CreateTile(t);
                }
            }

            for(int i=0; i< map.worldObjectLists.Count; i++)
            {
                int j = 0;
                while(j < instantiationAmmount[i] && map.worldObjectLists[i].worldObjects.Count > 0)
                {
                    j++;
                    int pos = (int)Random.Range(0, map.worldObjectLists[i].worldObjects.Count - 0.1f);
                    GameObject g = GameObject.Instantiate(objectPrefabs[i], tileInstanceParent);
                    g.transform.position = new Vector3(map.worldObjectLists[i].worldObjects[pos].x, map.worldObjectLists[i].worldObjects[pos].y, map.worldObjectLists[i].worldObjects[pos].z);
                    map.worldObjectLists[i].worldObjects.RemoveAt(pos);
                }
            }
        }
    }
}