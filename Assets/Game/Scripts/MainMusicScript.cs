using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusicScript : MonoBehaviour
{
    [SerializeField] private AudioSource music;

    void Start()
    {
        music.Play();
    }
}
