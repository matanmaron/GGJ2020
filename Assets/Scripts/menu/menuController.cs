using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public Button start_button;
    public Button credit_button;
    public Button exit_button;
    void Start()
    {
        start_button.onClick.AddListener(onClickStart);
        credit_button.onClick.AddListener(onClickCredit);
        exit_button.onClick.AddListener(onClickExit);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onClickStart()
    {
        SceneManager.LoadScene("mainScene");
    }

    void onClickCredit()
    {
        
    }

    void onClickExit()
    {
        Application.Quit();
    }
}
