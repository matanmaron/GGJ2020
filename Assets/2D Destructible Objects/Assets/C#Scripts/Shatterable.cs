using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatterable : MonoBehaviour, IItemDestroyAndFixScript, IHittable
{
    public List<Spawner> spawnPoints;

    private SpriteRenderer render;
    public bool isBroken = false;

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
        isBroken = true;
        render.enabled = false;

        foreach (Spawner spawn in spawnPoints)
        {
            spawn.Spawn();
        }

    }

    public void Fix()
    {
        isBroken = false;
        render.enabled = true;
        foreach (Spawner spawn in spawnPoints)
        {
            spawn.enabled = false;
        }
    }

    public void HitItem(bool isFix)
    {
        if (isFix && isBroken)
        {
            FixReceived();
        }
        else if (!isFix && !isBroken)
        {
            HitReceived();
        }
    }
}
