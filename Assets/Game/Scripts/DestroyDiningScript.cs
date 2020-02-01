using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestroyDiningScript : MonoBehaviour, IItemDestroyAndFixScript
{
    public bool isBroken = false;
    private Animator _animator;
    public bool HitItem(bool isFix)
    {
        _animator = gameObject.GetComponent<Animator>();
        if (isFix && isBroken)
        {
            FixReceived();
            return true;
        }
        else if (!isFix && !isBroken)
        {
            HitReceived();
            return true;
        }
        return false;
    }

    private void HitReceived()
    {
        _animator.Play("Break");
        isBroken = true;
        Debug.Log("dining destroyed");
    }

    private void FixReceived()
    {
        _animator.Play("Fix");
        isBroken = false;
        Debug.Log("dining fixed");
    }
}
