using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Unit
{
    // Start is called before the first frame update
    public override void init()
    {
        base.init();

        this.equipment = new Knife();

        this.debug_shape.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    // Update is called once per frame
    protected override void update()
    {
        base.update();
    }
}
