using TMPro;
using UnityEngine;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int _score;
    private void Start()
    {
        Coin.OnCoinCollected += () =>
        {
            _score++;
            scoreText.text = _score.ToString();
        };
    }
    
}
