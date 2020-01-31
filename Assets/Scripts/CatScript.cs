using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    [SerializeField] bool IsDebug = false;
    [SerializeField] float Speed = 1;
    
    private Rigidbody2D _rigidbody2D;
    private GameManager _gameManager;
    private KeyCode _keyleft = KeyCode.A;
    private KeyCode _keyrigth = KeyCode.D;
    private bool stoped = true;
    void Start()
    {
        if (IsDebug) { Debug.Log("*** CatScript debug is on ***"); }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (IsDebug && _rigidbody2D == null) { Debug.Log("cannot find cat Rigidbody2D"); }
        var gm = GameObject.Find("GameManager");
        if (IsDebug && gm == null) { Debug.Log("cannot find cat GameManager"); }
        _gameManager = gm.GetComponent<GameManager>();
        if (IsDebug && gm == null) { Debug.Log("cannot find cat GameManager script"); }
        _keyleft = _gameManager.CatLeft;
        _keyrigth = _gameManager.CatRight;
    }
    
    void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        var move = false;
        if (Input.GetKey(_keyleft))
        {
            _rigidbody2D.velocity = new Vector2(Speed * -1, _rigidbody2D.velocity.y);
            move = true;
            stoped = false;
        }
        if (Input.GetKey(_keyrigth))
        {
            _rigidbody2D.velocity = new Vector2(Speed, _rigidbody2D.velocity.y);
            move = true;
            stoped = false;
        }

        if (!move && !stoped)
        {
            _rigidbody2D.velocity = Vector2.zero;
            stoped = true;
        }
    }
}
