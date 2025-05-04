using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    
    [SerializeField] private AudioClip StartScenebackgroundMusic; // Nhạc nền mặc định
    // public SceneSignal sceneSignal;
[SerializeField] float lastAttackTime = 0;

    void Start()
    {
        if (StartScenebackgroundMusic != null)
        {
            // AudioManager.Instance.PlayMusic(StartScenebackgroundMusic);
        }
    }
            
    public void OnStartButtonPressed()
    {
        
        SceneManager.LoadScene("Select Level");
        // sceneSignal.LoadScene("BattleSelectScene");
        
    }

    public void OnOptionButtonPressed()
    {
        
        SceneManager.LoadScene("Select Hero");
        // sceneSignal.LoadScene("BattleSelectScene");
        
    }

    public void OnExitButtonPressed()
    {
        // GameController.Instance.Quit();
    }
}