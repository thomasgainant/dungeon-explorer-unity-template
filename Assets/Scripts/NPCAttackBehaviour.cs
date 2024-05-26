using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackBehaviour : MonoBehaviour
{
    public static float REFRESH_RATE = 3f; //In seconds

    public Unit unit;

    public float awarenessRadius = 10f;
    public Unit currentTarget;
    public bool isNotInRange = true;

    private NPCRoamingBehaviour roamingBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        this.roamingBehaviour = this.unit.GetComponent<NPCRoamingBehaviour>();
        StartCoroutine(this.update());
        StartCoroutine(this.longUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentTarget != null)
        {
            //attack!
            Vector3 diff = this.currentTarget.transform.position - this.transform.position;
            if (diff.magnitude <= this.unit.equipment.useRange)
            {
                this.isNotInRange = false;
                this.unit.equipment.trigger(this.currentTarget);
            }
            else
            {
                this.isNotInRange = true;
            }

            //TODO forget target if dead

            if(this.roamingBehaviour != null)
                this.roamingBehaviour.enabled = false;
        }
        else
        {
            if (this.roamingBehaviour != null)
                this.roamingBehaviour.enabled = true;
        }
    }

    private IEnumerator update()
    {
        yield return new WaitForSeconds(NPCAttackBehaviour.REFRESH_RATE * Random.Range(0f, 1f));

        bool continued = true;
        while (continued)
        {
            Unit enemy = null;
            float score = 0f;

            foreach (Unit other in this.unit.level.units)
            {
                if (other != this.unit && !this.isUnitFriendly(other))
                {
                    Vector3 diff = other.transform.position - this.unit.transform.position;
                    if(diff.magnitude <= this.awarenessRadius){
                        float currentScore = 1f / diff.magnitude; //TODO add better criteria than distance to determine who is the best enemy to choose
                        if (enemy == null || currentScore > score)
                        {
                            enemy = other;
                            score = currentScore;
                        }
                    }
                }
            }

            this.currentTarget = enemy;
            yield return new WaitForSeconds(NPCAttackBehaviour.REFRESH_RATE);
        }
    }

    private IEnumerator longUpdate()
    {
        bool continued = true;
        while (continued)
        {
            if (this.currentTarget != null && this.isNotInRange)
            {
                this.unit.setDestination(this.currentTarget.transform.position, true);
            }

            //TODO forget if not in range for a long time

            yield return new WaitForSeconds(NPCAttackBehaviour.REFRESH_RATE * 3f);
        }

    }

    public bool isUnitFriendly(Unit other)
    {
        if (
            (this.unit is Engineer || this.unit is Soldier) && (other is Engineer || other is Soldier)
        )
            return true;
        return false;
    }
}
