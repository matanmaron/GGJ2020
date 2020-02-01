using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusicScript : MonoBehaviour
{
    [SerializeField] private AudioSource music =null;

    void Start()
    {

        music.Play();
    }
}
