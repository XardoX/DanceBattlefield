using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
public class UIManager : MonoBehaviour
{
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
        if (Player.instance._defence == true)
        {
            currentDanceBonusText.text = "One-time protection";
        }
        if (Player.instance._attack == true)
        {
            currentDanceBonusText.text = "Slay your enemy!";
        }
        if (Player.instance._jump == true)
        {
            currentDanceBonusText.text = "Jump 2 squares";
        }
        if (Player.instance._earnScore == true)
        {
            currentDanceBonusText.text = "Extra points";
        }
    }
    public void SetDanceType(Queue<int> danceID)
    {
        SetDanceType(danceID, 0);
    }
    public void SetDanceType(Queue<int> danceID, int currentDance)
    {
        int i = 0;
        // Debug.Log("Count: " + danceID.Count);
        foreach(int dance in danceID)
        {
            _dances[i].sprite = _danceIcons[dance];
            _danceParticlesMat.mainTexture = _danceIcons[currentDance].texture;
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
