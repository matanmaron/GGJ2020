using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject credits;
    public void OnButtonStart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnButtonCredits()
    {
        credits.SetActive(true);
    }

    public void OnCredisBack()
    {
        credits.SetActive(false);
    }
    
    public void OnButtonExit()
    {
        Application.Quit();
    }
}
