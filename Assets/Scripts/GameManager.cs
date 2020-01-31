using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } //singletone
    
    [SerializeField] bool IsDebug = false;
    
    internal KeyCode CatRight = KeyCode.D;
    internal KeyCode CatLeft = KeyCode.A;
    internal KeyCode HumanRight = KeyCode.RightArrow;
    internal KeyCode HumanLeft = KeyCode.LeftArrow;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (IsDebug) { Debug.Log("*** GameManager debug is on ***"); }
    }
}
