using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Unit
{
    // Start is called before the first frame update
    public override void init()
    {
        base.init();

        this.equipment = new Minigun();
    }

    // Update is called once per frame
    protected override void update()
    {
        base.update();
    }
}
