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

        public GameObject[] tilePrefabs;
        public Transform tileInstanceParent;

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

            InitMap();
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
                Tile t = new Tile();
                t.rotation = (int)Random.Range(0, 3.99f);
                t.mirrored = (int)Random.Range(0, 3.99f);
                map.tiles.Add(t);
            }

            toolManager.groundPlate.localPosition = new Vector3(map.width*5, 1, map.height*5);
            toolManager.groundPlate.localScale = new Vector3(map.width, 1, map.height);
            toolManager.groundPlate.GetComponent<Renderer>().material.mainTextureScale = new Vector2(map.width/2f, map.height/2f);

            WorldObjectList treeList = new WorldObjectList();
            treeList.type = 0;
            map.worldObjectLists.Add(treeList);

            WorldObjectList worlfList = new WorldObjectList();
            worlfList.type = 1;
            map.worldObjectLists.Add(worlfList);

            WorldObjectList waypointList = new WorldObjectList();
            worlfList.type = 2;
            map.worldObjectLists.Add(waypointList);

            WorldObjectList spawnPointList = new WorldObjectList();
            worlfList.type = 3;
            map.worldObjectLists.Add(spawnPointList);

            WorldObjectList housePointList = new WorldObjectList();
            worlfList.type = 4;
            map.worldObjectLists.Add(housePointList);

            InitMap();
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

            switch (t.tileType) {
                case TileTypes.EMPTY:
                    t.type = 0;
                    CreateTile(t);
                    break;
                case TileTypes.MOUNTAIN:
                    t.type = 1;
                    CreateTile(t);
                    break;
                case TileTypes.RIVER:
                    MagicRivers(t, neighbors);
                    CreateTile(t);
                    break;
            }
        }

        private void CreateTile(Tile t)
        {
            if(t.gameObject != null)
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
            for(int x = 0; x < map.width; x++)
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
        public void CopyArea(Tile pointA, Tile pointB, Tile pointC)
        {
            int minX = Mathf.Min(pointA.xPosition, pointB.xPosition);
            int maxX = Mathf.Max(pointA.xPosition, pointB.xPosition);
            int minY = Mathf.Min(pointA.yPosition, pointB.yPosition);
            int maxY = Mathf.Max(pointA.yPosition, pointB.yPosition);

            for (int x = pointC.xPosition; x <= pointC.xPosition + (maxX - minX); x++)
            {
                for (int y = pointC.yPosition; y <= pointC.yPosition + (maxY - minY); y++)
                {
                    if (x >= 0 && x < map.width && y >= 0 && y < map.height)
                    {
                        Tile oldTile = map.tiles[minX + (x - pointC.xPosition) + (minY + (y - pointC.yPosition)) * map.width];
                        Tile newTile = map.tiles[x + y * map.width];

                        newTile.tileType = oldTile.tileType;
                        UpdateTile(newTile);
                        foreach (Tile t in toolManager.mapManager.GetNeighbors(newTile))
                        {
                            if (t != null)
                            {
                                toolManager.mapManager.UpdateTile(t);
                            }
                        }
                    }
                }
            }

            //update edges
            for (int x = pointC.xPosition - 1; x <= pointC.xPosition + (maxX - minX) + 1; x++)
            {
                int y = pointC.yPosition - 1;
                if (x >= 0 && x < map.width && y >= 0 && y < map.height)
                    UpdateTile(map.tiles[x + y * map.width]);
                y = pointC.yPosition + (maxY - minY) + 1;
                if (x >= 0 && x < map.width && y >= 0 && y < map.height)
                    UpdateTile(map.tiles[x + y * map.width]);
            }
            for (int y = pointC.yPosition; y <= pointC.yPosition + (maxY - minY); y++)
            {
                int x = pointC.xPosition - 1;
                if (x >= 0 && x < map.width && y >= 0 && y < map.height)
                    UpdateTile(map.tiles[x + y * map.width]);
                x = pointC.xPosition + (maxX - minX) + 1;
                if (x >= 0 && x < map.width && y >= 0 && y < map.height)
                    UpdateTile(map.tiles[x + y * map.width]);
            }
        }

        private void MagicRivers(Tile t, Tile[] neighbors)
        {
            int magicProduct = 1;
            if (neighbors[0] != null && neighbors[0].tileType == TileTypes.RIVER)
            {
                magicProduct *= 2;
            }
            if (neighbors[1] != null && neighbors[1].tileType == TileTypes.RIVER)
            {
                magicProduct *= 3;
            }
            if (neighbors[2] != null && neighbors[2].tileType == TileTypes.RIVER)
            {
                magicProduct *= 5;
            }
            if (neighbors[3] != null && neighbors[3].tileType == TileTypes.RIVER)
            {
                magicProduct *= 7;
            }
            switch (magicProduct)
            {
                case 1:
                    t.type = 2;
                    t.rotation = (int)(Random.Range(0, 3.99f));
                    t.mirrored   = (int)(Random.Range(0, 3.99f));
                    //lake
                    break;
                case 2:
                    //single arm lake
                    t.type = 3;
                    t.rotation = 3;
                    t.mirrored = (Random.Range(0, 2)) < 1 ? 0 : 2;
                    break;
                case 3:
                    t.type = 3;
                    t.rotation = 2;
                    t.mirrored = (Random.Range(0, 2)) < 1 ? 0 : 2;
                    //single arm lake
                    break;
                case 5:
                    t.type = 3;
                    t.rotation = 1;
                    t.mirrored = (Random.Range(0, 2)) < 1 ? 0 : 2;
                    //single arm lake
                    break;
                case 7:
                    t.type = 3;
                    t.rotation = 0;
                    t.mirrored = (Random.Range(0, 2)) < 1 ? 0 : 2;
                    //single arm lake
                    break;
                case 6:
                    t.type = 4;
                    t.rotation = 1;
                    t.mirrored = 0;
                    //2 arm river curve
                    break;
                case 14:
                    t.type = 4;
                    t.rotation = 2;
                    t.mirrored = 0;
                    //2 arm river curve
                    break;
                case 15:
                    t.type = 4;
                    t.rotation = 0;
                    t.mirrored = 0;
                    //2 arm river curve
                    break;
                case 35:
                    t.type = 4;
                    t.rotation = 3;
                    t.mirrored = 0;
                    //2 arm river curve 
                    break;
                case 10:
                    if(t.type != 8)
                        t.type = 5;
                    t.rotation = Random.Range(0,1.99f)<1?1:3;
                    t.mirrored = Random.Range(0, 1.99f) < 1 ? 1 : 3;
                    //2 arm river
                    break;
                case 21:
                    if (t.type != 8)
                        t.type = 5;
                    t.rotation = Random.Range(0, 1.99f) < 1 ? 0 : 2;
                    t.mirrored = Random.Range(0, 1.99f) < 1 ? 1 : 3;
                    //2 arm river
                    break;
                case 30:
                    t.type = 6;
                    t.rotation = 1;
                    t.mirrored = 0;
                    //3 arm river
                    break;
                case 105:
                    t.type = 6;
                    t.rotation = 0;
                    t.mirrored = 0;
                    //3 arm river
                    break;
                case 70:
                    t.type = 6;
                    t.rotation = 3;
                    t.mirrored = 0;
                    //3 arm river
                    break;
                case 42:
                    t.type = 6;
                    t.rotation = 2;
                    t.mirrored = 0;
                    //3 arm river
                    break;
                case 210:
                    t.type = 7;
                    t.rotation = (int)Random.Range(0,3.99f);
                    t.mirrored = (int)Random.Range(0, 3.99f);
                    //4 arm river
                    break;
            }
            //TODO magic
        }
    }
}