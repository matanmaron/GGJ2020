using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    BoxCollider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Toggle_Cage()
    {
        Debug.Log("cage locked");
        _collider.enabled = true;
        yield return new WaitForSeconds(5);
        _collider.enabled = false;
        Debug.Log("cage unlocked");
    }

    public void Lock()
    {
        StartCoroutine(Toggle_Cage());
    }

   
}
