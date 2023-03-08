using UnityEngine;
using UnityEngine.UI;

public class GameScores : MonoBehaviour
{
    private int _currentGameScore = 0;


    [SerializeField] private Text _scoreUI; 
    [SerializeField] private Text _endGameScoreUI; 

    public static GameScores Instance { get; private set; }


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void AddScore(int score)
    {
        _currentGameScore += score;
        _scoreUI.text = _currentGameScore.ToString();
        _endGameScoreUI.text = _currentGameScore.ToString();
    }
    
}

