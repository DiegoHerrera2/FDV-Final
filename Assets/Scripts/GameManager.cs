using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject gameOverScreen;


    private void Start()
    {
        player.OnDeath += OnPlayerDeath;
    }
    
    private void OnPlayerDeath()
    {
        Invoke(nameof(ShowGameOverScreen), 1.0f);
    }
    
    private void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
