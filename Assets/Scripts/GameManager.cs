using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start() {
        if (PlayerPrefs.GetInt("playIntroAnimation", 1) == 1) {
            //play intro animation
        }
        PlayerPrefs.SetInt("playerIntroAnimation", 0);
    }

    public void RestartGame() {
        SceneManager.LoadScene(1);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
