using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatterable : MonoBehaviour, IHittable
{
    public List<Spawner> spawnPoints;

    private SpriteRenderer render;
    private bool isBroken = false;

    // Use this for initialization
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void HitReceived()
    {
        Die();
    }

    public void FixReceived()
    {
        Fix();
    }

    public void Die()
    {
        render.enabled = false;

        foreach (Spawner spawn in spawnPoints)
        {
            spawn.Spawn();
        }

        isBroken = true;
    }

    public void Fix()
    {
        render.enabled = true;
        foreach (Spawner spawn in spawnPoints)
        {
            spawn.enabled = false;
        }
    }
}
