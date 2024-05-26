using Pratique;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity
{
    public float speed = 1f; //m/s
    private bool isSprinting = false;
    protected Vector3 directDestination;
    protected bool dirtyDirectDestination = false;
    protected Coroutine pathfindingProcess;
    public List<Pratique.Pathfinding.PathNode> currentPath = new List<Pathfinding.PathNode>();

    public Equipment equipment = new Claws();

    public GameObject debug_shape;

    // Start is called before the first frame update
    public override void init()
    {
        base.init();

        if(!this.level.debugMode)
            this.debug_shape.SetActive(false);
    }

    // Update is called once per frame
    protected override void update()
    {
        base.update();

        if (this.dirtyDirectDestination)
        {
            if (this.directDestination.x <= this.transform.position.x - this.speed * Time.deltaTime)
            {
                this.moveLeft();
            }
            else if (this.directDestination.x >= this.transform.position.x + this.speed * Time.deltaTime)
            {
                this.moveRight();
            }
            else
            {
                this.dirtyDirectDestination = false;

                if (this.currentPath.Count > 0)
                    this.currentPath.RemoveAt(0);

                if (this.currentPath.Count > 0)
                    this.setDirectDestination(this.currentPath[0].position);
            }
        }

        if (this.equipment != null)
        {
            this.equipment.update(Time.deltaTime);
        }
    }

    public override void takeDamage(float amount)
    {
        Animation animation = this.debug_shape.GetComponent<Animation>();
        animation.Play();

        base.takeDamage(amount);
    }
    protected override void die()
    {
        this.level.units.Remove(this);

        base.die();
    }

    protected void moveLeft()
    {
        this.transform.position = this.transform.position + Vector3.left * (this.isSprinting ? 2f : 1f) * this.speed * Time.deltaTime;
    }

    protected void moveRight()
    {
        this.transform.position = this.transform.position + Vector3.right * (this.isSprinting ? 2f : 1f) * this.speed * Time.deltaTime;
    }

    public void setDestination(Vector3 destination, bool sprint = false)
    {
        if (this.pathfindingProcess != null)
            StopCoroutine(this.pathfindingProcess);

        this.pathfindingProcess = StartCoroutine(
            Pratique.Pathfinding.FindPath(this.level.pathNodes, this.transform.position, destination, nodeList => {
                if (nodeList != null && nodeList.Count == 1)
                {
                    this.currentPath.Clear();
                    this.isSprinting = sprint;
                    this.setDirectDestination(destination);
                }
                else if (nodeList != null && nodeList.Count > 0)
                {
                    this.currentPath = nodeList;
                    this.isSprinting = sprint;
                    this.setDirectDestination(this.currentPath[0].position);
                }
                else
                {
                    //No path found
                }
            })
        );
    }

    protected void setDirectDestination(Vector3 directDestination)
    {
        this.dirtyDirectDestination = true;
        this.directDestination = directDestination;
        Debug.DrawRay(directDestination, Vector3.up, Color.yellow, 2f);
    }
}
