using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } //singletone
    
    [SerializeField] bool IsDebug = false;
    internal KeyCode CatRight = KeyCode.D;
    internal KeyCode CatLeft = KeyCode.A;
    internal KeyCode CatUp = KeyCode.W;
    internal KeyCode CatDown = KeyCode.S;
    internal KeyCode HumanRight = KeyCode.RightArrow;
    internal KeyCode HumanLeft = KeyCode.LeftArrow;
    internal KeyCode HumanUp = KeyCode.UpArrow;
    internal KeyCode HumanDown = KeyCode.DownArrow;
    internal KeyCode CatJump = KeyCode.Space;
    internal KeyCode HumanJump = KeyCode.Return;
    internal KeyCode CatAction = KeyCode.E;
    internal KeyCode HumanAction = KeyCode.RightShift;
    internal bool GamePaused = false;
    internal int CatScore = 0;
    internal int HumanScore = 0;
    
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

    public void OnButtonStart()
    {
        if (IsDebug) { Debug.Log("start game. loading MainScene"); }
        SceneManager.LoadScene("MainScene");
    }

    public void OnButtonCredits()
    {
        
    }
    
    public void OnButtonExit()
    {
        if (IsDebug) { Debug.Log("exit game"); }
        Application.Quit();
    }
    
    public void OnButtonEndGame()
    {
        if (IsDebug) { Debug.Log("game ended. loading menu"); }
        SceneManager.LoadScene(0);
    }
    
}
