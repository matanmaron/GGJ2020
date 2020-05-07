using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } //singletone
    
    [SerializeField] internal bool IsDebug = false;
    [SerializeField] internal float CatSpeed = 2;
    [SerializeField] internal float HumanSpeed = 1;
    [SerializeField] internal float CatLadderSpeed = 0.5f;
    [SerializeField] internal float HumanLadderSpeed = 1f;
    [SerializeField] internal float CatJumpSpeed = 200;
    [SerializeField] internal float HumanJumpSpeed = 200;
    [SerializeField] internal GameObject CatIcon;
    [SerializeField] internal GameObject HumanIcon;
    [SerializeField] internal AudioSource CatTalkSounds;
    [SerializeField] internal AudioSource CatWalkSounds;
    [SerializeField] internal AudioSource HumanTalkSounds;
    [SerializeField] internal AudioSource HumanWalkSounds;
    [SerializeField] internal AudioSource[] CatSFX;
    [SerializeField] internal AudioSource[] HumanSFX;
    [SerializeField] internal GameObject CubePos;
    [SerializeField] internal CageScript cage_script;
    [SerializeField] internal Text CatScoreText;
    [SerializeField] internal Text HumanScoreText;
    [SerializeField] internal Text UITimerText;
    [SerializeField] internal GameObject PausePanel;
    [SerializeField] internal GameObject UIPanel;
    [SerializeField] internal GameObject GameOverPanel;
    //win
    [SerializeField] private GameObject catholder;
    [SerializeField] private GameObject humanholder;
    [SerializeField] private Text catscoreui;
    [SerializeField] private Text humanscoreui;
    [SerializeField] private Text wintext;

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
    private float _timer;

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
        if (IsDebug) { Debug.Log("*** DEBUG IS ON ***"); }
        if (IsDebug) { Debug.Log("*** GameManager is Awake ! ***"); }
        NewGame();
    }

    private void Update()
    {
        if (!GamePaused)
        {
            _timer = _timer - Time.deltaTime;
            UITimerText.text = Mathf.RoundToInt(_timer).ToString();
            if (_timer <= 0)
            {
                //gameover
                GamePaused = true;
                GameOverPanel.SetActive(true);
                Win();
            }
        }
    }

    void LateUpdate()
    {
        if (!GamePaused && Input.GetKey(KeyCode.Escape))
        {
            GamePaused = !GamePaused;
            PausePanel.SetActive(GamePaused);
            UIPanel.SetActive(!GamePaused);
        }
    }

    private void NewGame()
    {
        HumanScoreText.text = "0";
        CatScoreText.text = "0";
        _timer = 120;
        GamePaused = false;
    }

    private void Win()
    {
        catscoreui.text = CatScore.ToString();
        humanscoreui.text = HumanScore.ToString();
        if (CatScore > HumanScore)
        {
            //cat win
            catholder.SetActive(true);
            catholder.GetComponentInChildren<Animator>().Play("Idle");
            wintext.text = "CAT !";
        }
        else if (CatScore < HumanScore)
        {
            //human wins
            humanholder.SetActive(true);
            humanholder.GetComponentInChildren<Animator>().Play("Idle");
            wintext.text = "HUMAN !";
        }
        else
        {
            //tie
            wintext.text = "TIE -_-";
        }
    }

    internal void ShowScore()
    {
        CatScoreText.text = CatScore.ToString();
        HumanScoreText.text = HumanScore.ToString();
    }

    public void OnButtonResume()
    {
        GamePaused = !GamePaused;
        PausePanel.SetActive(GamePaused);
        UIPanel.SetActive(!GamePaused);
        if (IsDebug) { Debug.Log("pause is " + GamePaused); }
    }
    public void HandleLadders(Transform me, Collider2D other, float speed, Ladder ladder)
    {
        var num = other.name[1];
        var dir = other.name[2];
        if ((ladder == Ladder.Down && dir == 'D') || (ladder == Ladder.Up && dir == 'U'))
        {
            return;
        }
        var newdir = dir == 'U' ? 'D' : 'U';
        var newlad = "L" + num + newdir;
        var newpos = GameObject.Find(newlad);
        if (GameManager.Instance.IsDebug && newpos == null) { Debug.Log("teleporting problem..."); }

        StartCoroutine(MoveToPosition(me, newpos.gameObject.transform.position, speed));
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float speed)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / speed;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
