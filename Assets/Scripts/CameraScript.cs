using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] bool IsDebug = false;
    [SerializeField] float SensitivityX = 1;
    [SerializeField] float SensitivityY = 1;
    [SerializeField] float MinimumY = -20;
    [SerializeField] float MaximumY = 40;
    private PlayerControlScript _target;
    private float _rotationY;
    private float _rotationX;

    void Start()
    {
        if (IsDebug) { Debug.Log("*** CameraScript debug is on ***"); }
        var player = GameObject.Find("Player");
        if (IsDebug && player == null)
        {
            Debug.Log("cannot find player in scene !");
        }

        _target = player.GetComponent<PlayerControlScript>();

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void LateUpdate()
    {
        if (IsDebug && _target == null)
        { 
            Debug.Log("cannot find playercontrolscript in player !");
        }
        _rotationX += Input.GetAxis("Mouse X") * SensitivityX;
        _rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
        _rotationY = Mathf.Clamp(_rotationY, MinimumY, MaximumY);
        transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        transform.position = _target.transform.position;
    }
}
