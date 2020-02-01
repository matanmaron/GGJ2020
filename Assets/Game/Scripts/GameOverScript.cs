using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private GameObject catholder;
    [SerializeField] private GameObject humanholder;
    [SerializeField] private Text catscoreui;
    [SerializeField] private Text humanscoreui;
    [SerializeField] private Text wintext;
    public void Win(GameManager gm)
    {
        catscoreui.text = gm.CatScore.ToString();
        humanscoreui.text = gm.HumanScore.ToString();
        if (gm.CatScore > gm.HumanScore)
        {
            //cat win
            catholder.SetActive(true);
            catholder.GetComponentInChildren<Animator>().Play("Idle");
            wintext.text = "CAT !";
        }
        else if (gm.CatScore < gm.HumanScore)
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
}
