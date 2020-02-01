using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicScript : MonoBehaviour
{
    [SerializeField] AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayMusic());
    }

    public IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(10);
        music.Play();
    }
}
