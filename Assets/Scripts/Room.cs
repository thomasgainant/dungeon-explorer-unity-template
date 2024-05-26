using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum TYPE
    {
        EMPTY,
        TELEPORTER
    }

    public LevelGen levelGen;

    public List<Tile> tiles = new List<Tile>();
    public List<Entity> items = new List<Entity>();

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
        StartCoroutine(this.generateTeleporterRoom());
    }
    private IEnumerator generateTeleporterRoom()
    {
        GameObject teleporterObj = Instantiate(this.levelGen.level.resources[1]) as GameObject;
        teleporterObj.transform.position = this.tiles[0].transform.position;

        Teleporter teleporter = teleporterObj.GetComponent<Teleporter>();
        this.items.Add(teleporter);

        this.levelGen.level.spawnUnit(typeof(Alien), this.tiles[this.tiles.Count - 1].transform.position);
        yield return null;
    }
}
