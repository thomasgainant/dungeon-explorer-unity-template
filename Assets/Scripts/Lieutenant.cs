using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lieutenant : Unit
{
    // Start is called before the first frame update
    public override void init()
    {
        base.init();

        this.equipment = new Revolver();
    }

    // Update is called once per frame
    protected override void update()
    {
        base.update();
    }
}
