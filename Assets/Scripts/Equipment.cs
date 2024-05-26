public class Equipment
{
    public float damage = 1f;
    public float useRange = 1f;
    public float cooldown = 1f;
    protected float currentCooldown = 0f;

    public void update(float deltaTime)
    {
        if (this.currentCooldown > 0f)
        {
            this.currentCooldown -= deltaTime;
        }
    }

    public void trigger(Entity target)
    {
        if(this.currentCooldown <= 0f)
        {
            this.currentCooldown = this.cooldown;
            this.use(target);
        }
    }

    protected void use(Entity target)
    {
        target.takeDamage(this.damage);
    }
}

public class ProjectileEquipment : Equipment
{
    //TODO
}

public class Claws : Equipment
{
    public Claws() {
        this.damage = 75f;
        this.useRange = 0.5f;
    }
}

public class Knife : Equipment
{
    public Knife()
    {
        this.useRange = 0.5f;
    }
}

public class AssaultRifle : Equipment
{
    public AssaultRifle()
    {
        this.useRange = 20f;
    }
}

public class Shotgun : Equipment
{
    public Shotgun()
    {
        this.useRange = 10f;
    }
}

public class Minigun : Equipment
{
    public Minigun()
    {
        this.useRange = 15f;
    }
}

public class Revolver : Equipment
{
    public Revolver()
    {
        this.useRange = 15f;
    }
}