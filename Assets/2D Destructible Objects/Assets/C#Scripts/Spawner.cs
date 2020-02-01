using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform whereToSpawn;

    public GameObject Spawn()
    {
        if (isActiveAndEnabled && gameObject.activeSelf)
        {
            return Instantiate(prefabToSpawn, whereToSpawn.position, Quaternion.identity);
        }

        return null;
    }
}
