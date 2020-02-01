using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatterable : MonoBehaviour, IItemDestroyAndFixScript, IHittable
{
    public List<Spawner> spawnPoints;

    private SpriteRenderer render;
    public bool isBroken = false;

    private List<GameObject> _shards;
    // Use this for initialization
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        _shards = new List<GameObject>();
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
            var i = spawn.Spawn();
            if (i != null)
            {
                _shards.Add(i);
            }
        }

    }

    public void Fix()
    {
        isBroken = false;
        render.enabled = true;
        foreach (GameObject s in _shards)
        {
            GameObject.Destroy(s.gameObject);
        }
        _shards = new List<GameObject>();
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
