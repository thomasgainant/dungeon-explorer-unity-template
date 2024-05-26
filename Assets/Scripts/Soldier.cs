using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    // Start is called before the first frame update
    public override void init()
    {
        base.init();

        this.equipment = new AssaultRifle();

        this.debug_shape.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    // Update is called once per frame
    protected override void update()
    {
        base.update();
    }
}
