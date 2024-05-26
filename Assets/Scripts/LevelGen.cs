using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGen : MonoBehaviour
{
    public Level level;

    public List<Sector> sectors = new List<Sector>();

    // Start is called before the first frame update
    void Start()
    {
        this.level = this.gameObject.GetComponent<Level>();

        this.generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generate()
    {
        StartCoroutine(this.generate_routine());
    }
    private IEnumerator generate_routine()
    {
        Sector startSector = this.generateSectorAt(Vector2.zero);

        yield return new WaitForSeconds(5f);

        Room teleporterRoom = startSector.generateRoom(new Vector2(Sector.SIZE_IN_TILES / 2, Sector.SIZE_IN_TILES / 2) + new Vector2(-2, -1), 4, 2, Room.TYPE.TELEPORTER);

        yield return new WaitForSeconds(5f);

        startSector.refreshPathNodes();

        yield return null;

        this.level.avatar.aimedTile = startSector.getTileAt(new Vector2(Sector.SIZE_IN_TILES / 2, Sector.SIZE_IN_TILES / 2));

        yield return new WaitForSeconds(5f);

        Teleporter teleporter = teleporterRoom.items[0] as Teleporter;
        teleporter.teleport(typeof(Engineer));
        teleporter.teleport(typeof(Engineer));
        teleporter.teleport(typeof(Engineer));
        teleporter.teleport(typeof(Soldier));
    }

    public Sector generateSectorAt(Vector2 coors)
    {
        GameObject sectorObj = new GameObject("Sector");
        sectorObj.transform.position = new Vector2(
            coors.x * Sector.SIZE_IN_TILES * Tile.SIZE,
            coors.y * Sector.SIZE_IN_TILES * Tile.SIZE
        );

        Sector newSector = sectorObj.AddComponent<Sector>();
        newSector.levelGen = this;
        newSector.coors = coors;

        newSector.generate();

        this.sectors.Add(newSector);
        return newSector;
    }

    public Sector getSectorAt(Vector3 position)
    {
        foreach (Sector sector in this.sectors)
        {
            if (
                sector.transform.position.x <= position.x && position.x <= sector.transform.position.x + (Sector.SIZE_IN_TILES * Tile.SIZE)
                && sector.transform.position.y <= position.y && position.y <= sector.transform.position.y + (Sector.SIZE_IN_TILES * Tile.SIZE)
            )
            {
                return sector;
            }
        }
        return null;
    }

    public Tile getTileAt(Vector3 position)
    {
        foreach (Sector sector in this.sectors)
        {
            foreach (Tile tile in sector.tiles)
            {
                if (
                    tile.transform.position.x <= position.x && position.x <= tile.transform.position.x + Tile.SIZE
                    && tile.transform.position.y <= position.y && position.y <= tile.transform.position.y + Tile.SIZE
                )
                {
                    return tile;
                }
            }
        }
        return null;
    }
}
