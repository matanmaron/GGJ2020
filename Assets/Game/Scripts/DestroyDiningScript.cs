using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestroyDiningScript : MonoBehaviour, IItemDestroyAndFixScript
{
    public bool isBroken = false;
    private Animator _animator;
    public void HitItem(bool isFix)
    {
        _animator = gameObject.GetComponent<Animator>();
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
