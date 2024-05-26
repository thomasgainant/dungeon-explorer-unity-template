using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Unit
{
    // Start is called before the first frame update
    public override void init()
    {
        base.init();

        this.debug_shape.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    protected override void update()
    {
        base.update();
    }
}
