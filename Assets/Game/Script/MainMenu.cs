using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string loadLevel;
    public void StarGame()
    {

        SceneManager.LoadScene(loadLevel);
      
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestData() 
    {
        Debug.Log(PlayerPrefs.GetInt("Level"));
        PlayerPrefs.DeleteAll();
        Debug.Log(PlayerPrefs.GetInt("Level"));
    
    }
}
