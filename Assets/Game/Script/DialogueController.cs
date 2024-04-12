using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public GameObject[] faces;

    public string[] lines;

    public float textSpeed;

    private int index;

    void Start()
    {
        if (PlayerPrefs.GetInt("ShowDialogue") == 0)
        {
            textComponent.text = string.Empty;
            StartDialogue();

            PlayerPrefs.SetInt("ShowDialogue", 1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
        
    }

    
    void Update()
    {

    }

    void StartDialogue() 
    {

        index = 0;
        StartCoroutine(TypeLine());
     
    }
   
    IEnumerator TypeLine() 
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);


        }
       
    }

    void NextLine() 
    {

        if (index < lines.Length -1 )
        {
            faces[index].SetActive(false);

            index++;

            faces[index].SetActive(true);

            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    
    }
    
    public void NextSceneButton() 
    {

        if (textComponent.text == lines[index])
        {
            NextLine();
        }
        else 
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
            
        }
    
    }
   

}
