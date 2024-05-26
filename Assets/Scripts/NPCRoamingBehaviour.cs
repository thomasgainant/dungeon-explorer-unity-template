using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRoamingBehaviour : MonoBehaviour
{
    public Unit unit;

    private float roamingTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.roamingTimer > 0f)
        {
            this.roamingTimer -= Time.deltaTime;
        }
        else
        {
            this.unit.setDestination(this.transform.position + new Vector3(
                Random.Range(-5f, 5f),
                0f,
                0f
            ));
            this.resetRoamingTimer();
        }
    }

    private void resetRoamingTimer()
    {
        this.roamingTimer = Random.Range(3f, 10f);
    }
}
