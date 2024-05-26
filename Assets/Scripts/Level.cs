using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public UnityEngine.Object[] resources;
    public bool debugMode = true;

    public Avatar avatar;
    public LevelGen levelGen;

    public List<Pratique.Pathfinding.PathNode> pathNodes = new List<Pratique.Pathfinding.PathNode>();
    public List<Unit> units = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        this.avatar = GameObject.FindObjectOfType<Avatar>();
        this.levelGen = this.gameObject.GetComponent<LevelGen>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnUnit(Type type, Vector3 position)
    {
        UnityEngine.Object prefab = null;
        if (type == typeof(Engineer))
        {
            prefab = this.resources[2];
        }
        else if (type == typeof(Soldier))
        {
            prefab = this.resources[3];
        }
        else if (type == typeof(Alien))
        {
            prefab = this.resources[4];
        }

        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab) as GameObject;
            instance.transform.position = position;

            Unit unit = instance.GetComponent<Unit>();
            unit.level = this;

            NPCRoamingBehaviour roamingBehaviour = instance.AddComponent<NPCRoamingBehaviour>();
            roamingBehaviour.unit = unit;

            NPCAttackBehaviour attackBehaviour = instance.AddComponent<NPCAttackBehaviour>();
            attackBehaviour.unit = unit;

            this.units.Add(unit);
        }
    }
}
