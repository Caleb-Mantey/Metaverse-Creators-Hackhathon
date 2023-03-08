using UnityEngine;

public class HitScore : MonoBehaviour
{
    [SerializeField] private int score = 1;

    public int GetScorePoint()
    {
        return score;
    }
}

