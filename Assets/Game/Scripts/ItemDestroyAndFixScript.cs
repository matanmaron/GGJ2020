using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyAndFixScript : MonoBehaviour
{
    public void HitItem(bool isFix)
    {
        var script = transform.GetComponent<Shatterable>();
        if (isFix)
        {
            script.FixReceived();
        }
        else
        {
            script.HitReceived();
        }
    }
}
