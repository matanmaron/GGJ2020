using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDiningScript : MonoBehaviour, IItemDestroyAndFixScript
{
    public bool isBroken = false;

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

    private void HitReceived()
    {
        isBroken = true;
        Debug.Log("dining destroyed");
    }

    private void FixReceived()
    {
        isBroken = false;
        Debug.Log("dining fixed");
    }
}
