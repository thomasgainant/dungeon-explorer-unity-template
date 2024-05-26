using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static float SIZE = 5f;

    public enum CONTENT
    {
        ROCK,
        UNBREAKABLE,
        URANIUM
    }
    public enum SHAPE_MODE
    {
        FULL,
        EMPTY
    }

    public Vector2 coors; //In Sector
    public Pratique.Pathfinding.PathNode pathNode;

    public Tile.CONTENT content = Tile.CONTENT.ROCK;
    public Tile.SHAPE_MODE shapeMode;

    public GameObject shape_full;
    public GameObject shape_empty;

    // Start is called before the first frame update
    void Start()
    {
        this.switchMode(SHAPE_MODE.FULL);
        StartCoroutine(this.longUpdate_routine());
    }

    public void initPathNode()
    {
        this.pathNode = new Pratique.Pathfinding.PathNode();
        this.pathNode.position = new Vector2(this.transform.position.x, this.transform.position.y - (Tile.SIZE/2f));
        this.pathNode.type = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator longUpdate_routine()
    {
        bool continued = true;
        while (continued)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void switchMode(Tile.SHAPE_MODE newMode)
    {
        switch (newMode)
        {
            case SHAPE_MODE.FULL:
                this.shape_full.SetActive(true);
                this.shape_empty.SetActive(false);

                if(this.pathNode != null)
                    this.pathNode.type = -1;
                break;
            case SHAPE_MODE.EMPTY:
                this.shape_full.SetActive(false);
                this.shape_empty.SetActive(true);

                this.pathNode.type = 0;
                break;
        }

        this.shapeMode = newMode;
    }

    public Vector3 getAimingPosition()
    {
        return this.transform.position + new Vector3(Tile.SIZE/2f, Tile.SIZE/2f, -30f);
    }
}
