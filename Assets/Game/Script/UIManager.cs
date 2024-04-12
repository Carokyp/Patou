using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text scoreText;

    public TMP_Text winScore;
    public TMP_Text winText;
    public GameObject winStar0, winStar1, winStar2, winStar3;
    public GameObject winDog0, winDog1, winDog2, winDog3;
    public GameObject happyDog,sadDog;


    public GameObject roundOverScreen;

    
    // Start is called before the first frame update
    void Start()
    {


        winStar1.SetActive(false);
        winStar1.SetActive(false);
        winStar2.SetActive(false);
        winStar3.SetActive(false);
        
        winDog0.SetActive(false);
        winDog1.SetActive(false);
        winDog2.SetActive(false);
        winDog3.SetActive(false);

        happyDog.SetActive(false);
        sadDog.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
