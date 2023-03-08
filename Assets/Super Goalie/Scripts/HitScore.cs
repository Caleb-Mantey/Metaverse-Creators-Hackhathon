using UnityEngine;

public class HitScore : MonoBehaviour
{
    [SerializeField] private int score = 1;
    [SerializeField] private GameObject scoreCanvas;


    public int GetScorePoint()
    {
       if(scoreCanvas) Instantiate(scoreCanvas);
        return score;
    }
}

