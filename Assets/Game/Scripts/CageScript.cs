using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Lock_The_Cage()
    {
        transform.GetComponent<Collider2D>().enabled=true;
        new WaitForSeconds(5);
        transform.GetComponent<Collider2D>().enabled = false;
    }
}
