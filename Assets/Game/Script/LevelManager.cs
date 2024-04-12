using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int nextLevelToLoad;

    public int levelToReplay;


    void Start()
    {
        
    }

    public void LoadLevel()
    {
        
        SceneManager.LoadScene(nextLevelToLoad);

    }

    public void Replay() 
    {

        SceneManager.LoadScene(levelToReplay);
    
    }
}
