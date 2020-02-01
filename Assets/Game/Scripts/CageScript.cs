using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    BoxCollider2D _collider;
    SpriteRenderer _sprite;
    
    void Start()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public IEnumerator Toggle_Cage()
    {
        Debug.Log("cage locked");
        _collider.enabled = true;
        _sprite.enabled = true;
        yield return new WaitForSeconds(5);
        _collider.enabled = false;
        _sprite.enabled = false;
        Debug.Log("cage unlocked");
    }

    public void Lock()
    {
        StartCoroutine(Toggle_Cage());
    }

   
}
