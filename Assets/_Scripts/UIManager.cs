﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public GameObject defeatWindow;
    public TextMeshProUGUI endscore;
    public GameObject window;
    [SerializeField] private Material _danceParticlesMat = default;
    [SerializeField] private List<Sprite> _danceIcons = default;
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private Image currentDanceType = null;
    [SerializeField] private GameObject _dancesParent = null;
    [SerializeField][ReadOnly] private List<Image> _dances = null;
    [SerializeField] private GameObject _heartsParent = null;
    [SerializeField][ReadOnly] private List<GameObject> _Hearts = null;
    [SerializeField] public TextMeshProUGUI currentDanceBonusText = null;
    public static UIManager instance;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } else Destroy(this);

        foreach(Transform child in _heartsParent.transform)
        {
            _Hearts.Add(child.gameObject);
        }
        foreach(Transform child in _dancesParent.transform)
        {
            _dances.Add(child.GetComponent<Image>());
        }
    }

    private void Update()
    {
    }
    public void SetDanceType(Queue<int> danceID)
    {
        SetDanceType(danceID, 0);
    }
    public void SetDanceType(Queue<int> danceID, int currentDance)
    {
        _danceParticlesMat.mainTexture = _danceIcons[currentDance].texture;
        currentDanceType.sprite = _danceIcons[currentDance];
        
        int i = 0;
        // Debug.Log("Count: " + danceID.Count);
        foreach(int dance in danceID)
        {
            _dances[i].sprite = _danceIcons[dance];
            i++;
        }
        switch(currentDance)
        {
            case 0:
            currentDanceBonusText.text = "One-time protection";
                break;
            case 1:
            currentDanceBonusText.text = "Slay your enemy!";
                break;
            case 2:
            currentDanceBonusText.text = "Jump 2 squares";
                break;
            case 3:
            currentDanceBonusText.text = "Extra points";
                break;
            default:
                break;
        }
    }

    public void SetPlayerLives(int lives)
    {
        foreach(GameObject hearth in _Hearts)
        {
            if(lives > 0)
            {
                hearth.SetActive(true);
            }else hearth.SetActive(false);
            lives--;
        }
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
        endscore.text = score.ToString();
    }

    public void StartGame()
    {
        StartCoroutine(Cooldown());
        window.SetActive(false);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
    }

    public void ShowDefeatWindow()
    {
        defeatWindow.SetActive(true);
        Time.timeScale = 0f;
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
