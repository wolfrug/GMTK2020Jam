using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public Button startGameButton;
    public Button restartGame;
    public Button quitGame;
    public string startGameScene;
    public void StartGameButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}