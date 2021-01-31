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

        public static GameObject house;

        public GameObject[] tilePrefabs;
        public GameObject[] objectPrefabs;
        public int[] instantiationAmmount;
        public LayerMask groundMask;

        public Transform tileInstanceParent;
        public TreeList treeList;

        public string mapName;
        public Transform playerTransform;

        private bool isGenerated = false;

        private void Awake()
        {
            LoadMap();
        }

        private void Update()
        {
            if (isGenerated)
                return;
            isGenerated = true;

            for (int i = 0; i < map.worldObjectLists.Count; i++)
            {
                int j = 0;
                Debug.Log(i + " " + map.worldObjectLists[i].worldObjects.Count);
                while (j < instantiationAmmount[i] && map.worldObjectLists[i].worldObjects.Count > 0)
                {
                    int pos = (int)Random.Range(0, map.worldObjectLists[i].worldObjects.Count - 0.1f);
                    Vector3 estimatedPos = new Vector3(map.worldObjectLists[i].worldObjects[pos].x, map.worldObjectLists[i].worldObjects[pos].y, map.worldObjectLists[i].worldObjects[pos].z);
                    FixGroundY(ref estimatedPos);
                    switch (i)
                    {
                        case 1:
                            GameObject wolf = GameObject.Instantiate(objectPrefabs[i], tileInstanceParent);
                            wolf.transform.position = estimatedPos + Vector3.up;
                            map.worldObjectLists[i].worldObjects.RemoveAt(pos);
                            break;
                        case 3:
                            playerTransform.position = estimatedPos + Vector3.up;
                            break;
                        case 0:
                            GameObject tree = GameObject.Instantiate(objectPrefabs[i], tileInstanceParent);
                            treeList.trees.Add(tree);
                            tree.transform.position = estimatedPos;
                            treeList.treeList.Add(tree.transform.position);
                            map.worldObjectLists[i].worldObjects.RemoveAt(pos);
                            break;
                        case 4:
                            GameObject house = GameObject.Instantiate(objectPrefabs[i], tileInstanceParent);
                            house.transform.position = estimatedPos;
                            map.worldObjectLists[i].worldObjects.RemoveAt(pos);
                            MapGenerator.house = house;
                            break;
                        default:
                            GameObject g = GameObject.Instantiate(objectPrefabs[i], tileInstanceParent);
                            g.transform.position = estimatedPos;
                            map.worldObjectLists[i].worldObjects.RemoveAt(pos);
                            break;
                    }
                    j++;
                }
            }
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
        }

        private void FixGroundY(ref Vector3 estimatedPosition)
        {
            RaycastHit hit;
            if(Physics.Raycast(new Ray(estimatedPosition+Vector3.up,Vector3.down),out hit, 5, groundMask))
            {
                estimatedPosition.y = hit.point.y;
            }                        
        }
    }
}