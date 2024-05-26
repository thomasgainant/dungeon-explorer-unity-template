using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Entity
{
    // Start is called before the first frame update
    public override void init()
    {
        base.init();
    }

    // Update is called once per frame
    protected override void update()
    {
        base.update();
    }

    public void teleport(Type type)
    {
        this.level.spawnUnit(type, this.transform.position);
    }
}
