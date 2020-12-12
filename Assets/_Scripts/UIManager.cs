using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI currentDanceType = null;
    [SerializeField] private GameObject _dancesParent = null;
    [SerializeField] [ReadOnly] private List<TextMeshProUGUI> _dances = null;
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
            _dances.Add(child.GetComponent<TextMeshProUGUI>());
        }
    }

    private void Update()
    {
        if (Player._defence == true)
        {
            currentDanceBonusText.text = "One-time protection";
        }
        if (Player._attack == true)
        {
            currentDanceBonusText.text = "Slay your enemy!";
        }
        if (Player._jump == true)
        {
            currentDanceBonusText.text = "Jump 2 squares";
        }
        if (Player._earnScore == true)
        {
            currentDanceBonusText.text = "Extra points";
        }
    }

    public void SetDanceType(Queue<int> danceID)
    {
        int i = 0;
        // Debug.Log("Count: " + danceID.Count);
        foreach(int dance in danceID)
        {
            _dances[i].text = (dance + 1).ToString();
            i++;
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
    }

}
