using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public GameObject screenStart;
    public GameObject screenWin;
    public PlayerMovement player;
    public TextMeshProUGUI brickText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI outBrickText;

    public void OnClickBtnStart()
    {
        GameManager.Instance.IsPlaying = true;
        screenStart.SetActive(false);
        brickText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        player.gameObject.GetComponent<PlayerMovement>().enabled = true;
    }

    public void OnClickBtnPlayAgain()
    {
        GameManager.Instance.IsPlaying = true;
        GameManager.Instance.IsWinning = false;
        GameManager.Instance.LoadNewGame();
        screenWin.SetActive(false);
        brickText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        screenStart.SetActive(true);
        player.gameObject.GetComponent<PlayerMovement>().OnInit();
        player.gameObject.GetComponent<PlayerMovement>().enabled = false;
    }
    public void OnQuit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPlaying)
        {
            if (GameManager.Instance.IsWinning)
            {
                screenWin.SetActive(true);
                return;
            }
            if (GameManager.Instance.IsOutOfBricks)
            {
                outBrickText.gameObject.SetActive(true);
            }
            else
            {
                outBrickText.gameObject.SetActive(false);
            }

            brickText.text = "Brick: " + player.OwnBrick.ToString();
            levelText.text = "Level: " + GameManager.Instance.Level;
            levelText.text = "Level: " + GameManager.Instance.Level;
        }
    }
}
