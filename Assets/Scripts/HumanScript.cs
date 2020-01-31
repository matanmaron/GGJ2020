using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanScript : MonoBehaviour
{
    [SerializeField] bool IsDebug = false;
    [SerializeField] float Speed = 1;
    [SerializeField] private GameObject PausePanel;
    
    private Rigidbody2D _rigidbody2D;
    private GameManager _gameManager;
    private Animator _animator;
    private KeyCode _keyleft;
    private KeyCode _keyrigth;
    private bool stoped = true;
    private Face _face = Face.Left;
    private bool _changeFace = false;
    
    void Start()
    {
        if (IsDebug) { Debug.Log("*** HumanScript debug is on ***"); }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (IsDebug && _rigidbody2D == null) { Debug.Log("cannot find human Rigidbody2D"); }
        var gm = GameObject.Find("GameManager");
        if (IsDebug && gm == null) { Debug.Log("cannot find human GameManager"); }
        _gameManager = gm.GetComponent<GameManager>();
        if (IsDebug && gm == null) { Debug.Log("cannot find human GameManager script"); }
        _animator = transform.gameObject.GetComponentInChildren<Animator>();
        if (IsDebug && _animator == null) { Debug.Log("cannot find human Animator"); }
        _keyleft = _gameManager.HumanLeft;
        _keyrigth = _gameManager.HumanRight;
        _animator.Play("Idle");
    }
    
    void LateUpdate()
    {
        if (!_gameManager.GamePaused  && Input.GetKey(KeyCode.Escape))
        {//only in human ! no need to copy on cat!
            _gameManager.GamePaused = !_gameManager.GamePaused;
            PausePanel.SetActive(_gameManager.GamePaused);
        }
        if (!_gameManager.GamePaused)
        {
            Move();
            ChangeFace();
        }
    }

    private void ChangeFace()
    {
        if (_changeFace)
        {
            if (_face == Face.Left)
            {
                if (IsDebug) { Debug.Log("human turn left"); }

                //0 to 180
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (_face == Face.Right)
            {
                if (IsDebug) { Debug.Log("human turn right"); }
                //180 t0 0
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            _changeFace = false;
        }
    }
    
    private void Move()
    {
        var move = false;
        if (Input.GetKey(_keyleft))
        {
            if (IsDebug) { Debug.Log("human left"); }
            _rigidbody2D.velocity = new Vector2(Speed * -1, _rigidbody2D.velocity.y);
            stoped = false;
            move = true;
            _face = Face.Left;
            _changeFace = true;
            _animator.Play("Walk");
        }
        if (Input.GetKey(_keyrigth))
        {
            if (IsDebug) { Debug.Log("human right"); }
            _rigidbody2D.velocity = new Vector2(Speed, _rigidbody2D.velocity.y);
            stoped = false;
            move = true;
            _face = Face.Right;
            _changeFace = true;
            _animator.Play("Walk");
        }

        if (!move && !stoped)
        {
            if (IsDebug) { Debug.Log("human stop"); }
            _rigidbody2D.velocity = Vector2.zero;
            stoped = true;
            _animator.Play("Idle");
        }
    }
    
    public void OnButtonResume()
    {
        _gameManager.GamePaused = !_gameManager.GamePaused;
        PausePanel.SetActive(_gameManager.GamePaused);
        if (IsDebug) { Debug.Log("pause is " + _gameManager.GamePaused); }
    }
    
}
