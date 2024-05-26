using Pratique;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Pratique.Pathfinding;

public class Sector : MonoBehaviour
{
    public static int SIZE_IN_TILES = 10;

    public LevelGen levelGen;
    public Vector2 coors = Vector2.zero;

    public List<Tile> tiles = new List<Tile>();
    public List<Room> rooms = new List<Room>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generate()
    {
        StartCoroutine(this.generate_routine());
    }
    private IEnumerator generate_routine()
    {
        for (int x = 0; x < Sector.SIZE_IN_TILES; x++)
        {
            for (int y = 0; y < Sector.SIZE_IN_TILES; y++)
            {
                GameObject newTileObj = (GameObject)Instantiate(this.levelGen.level.resources[0]);
                newTileObj.transform.position = (this.coors + new Vector2(x, y)) * Tile.SIZE;
                newTileObj.transform.SetParent(this.transform);

                yield return null;

                Tile tile = newTileObj.GetComponent<Tile>();
                tile.coors = new Vector2(x, y);
                tile.initPathNode();

                this.tiles.Add(tile);
                this.levelGen.level.pathNodes.Add(tile.pathNode);
            }
        }
    }

    public void refreshPathNodes()
    {
        foreach (Tile tile in this.tiles)
        {
            Tile neighbour = this.getTileAt(new Vector2(tile.coors.x, tile.coors.y - 1));
            if (neighbour != null && neighbour.pathNode != null && Pathfinding.isNodeUseable(neighbour.pathNode))
                tile.pathNode.neighbours.Add(neighbour.pathNode);

            neighbour = this.getTileAt(new Vector2(tile.coors.x + 1, tile.coors.y - 1));
            if (neighbour != null && neighbour.pathNode != null && Pathfinding.isNodeUseable(neighbour.pathNode))
                tile.pathNode.neighbours.Add(neighbour.pathNode);

            neighbour = this.getTileAt(new Vector2(tile.coors.x + 1, tile.coors.y));
            if (neighbour != null && neighbour.pathNode != null && Pathfinding.isNodeUseable(neighbour.pathNode))
                tile.pathNode.neighbours.Add(neighbour.pathNode);

            neighbour = this.getTileAt(new Vector2(tile.coors.x + 1, tile.coors.y + 1));
            if (neighbour != null && neighbour.pathNode != null && Pathfinding.isNodeUseable(neighbour.pathNode))
                tile.pathNode.neighbours.Add(neighbour.pathNode);

            neighbour = this.getTileAt(new Vector2(tile.coors.x, tile.coors.y + 1));
            if (neighbour != null && neighbour.pathNode != null && Pathfinding.isNodeUseable(neighbour.pathNode))
                tile.pathNode.neighbours.Add(neighbour.pathNode);

            neighbour = this.getTileAt(new Vector2(tile.coors.x - 1, tile.coors.y + 1));
            if (neighbour != null && neighbour.pathNode != null && Pathfinding.isNodeUseable(neighbour.pathNode))
                tile.pathNode.neighbours.Add(neighbour.pathNode);

            neighbour = this.getTileAt(new Vector2(tile.coors.x - 1, tile.coors.y));
            if (neighbour != null && neighbour.pathNode != null && Pathfinding.isNodeUseable(neighbour.pathNode))
                tile.pathNode.neighbours.Add(neighbour.pathNode);

            neighbour = this.getTileAt(new Vector2(tile.coors.x - 1, tile.coors.y - 1));
            if (neighbour != null && neighbour.pathNode != null && Pathfinding.isNodeUseable(neighbour.pathNode))
                tile.pathNode.neighbours.Add(neighbour.pathNode);
        }
    }

    public Room generateRoom(Vector2 coors, int sizeX, int sizeY, Room.TYPE type = Room.TYPE.EMPTY)
    {
        Room room = this.gameObject.AddComponent<Room>();
        room.levelGen = this.levelGen;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector2 tileCoors = coors + new Vector2(x, y);
                Tile tile = this.getTileAt(tileCoors);

                if (tile != null)
                {
                    tile.switchMode(Tile.SHAPE_MODE.EMPTY);
                    room.tiles.Add(tile);
                }
            }
        }

        room.generate();

        this.rooms.Add(room);
        return room;
    }

    public Tile getTileAt(Vector2 coors)
    {
        foreach (Tile tile in this.tiles)
        {
            Vector3 diff = tile.coors - coors;
            if (diff.magnitude <= 0.01f)
            {
                return tile;
            }
        }
        return null;
    }
}
