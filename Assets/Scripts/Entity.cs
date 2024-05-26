using System.Collections;
using UnityEngine;
public class Entity : MonoBehaviour
{
    public Level level;

    public float health = 100f;

    // Use this for initialization
    void Start()
    {
        this.init();
    }
    public virtual void init()
    {
        if(this.level == null)
            this.level = GameObject.FindObjectOfType<Level>();
    }

    // Update is called once per frame
    void Update()
    {
        this.update();
    }
    protected virtual void update()
    {

    }

    public virtual void takeDamage(float amount)
    {
        this.health -= amount;
        if (this.health <= 0f)
            this.die();
    }
    protected virtual void die()
    {
        Destroy(this.gameObject);
    }
}