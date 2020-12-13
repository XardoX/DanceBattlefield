using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Title1;
    public GameObject Title2;

    public float timer = 0.43f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && Title1.activeSelf)
        {
            Title1.SetActive(false);
            Title2.SetActive(true);
            timer = 0.43f;
        }

        if (timer <= 0 && Title2.activeSelf)
        {
            Title1.SetActive(true);
            Title2.SetActive(false);
            timer = 0.43f;
        }
    }

    public void StartGame()
    {
        Debug.Log("Play!");
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

}
