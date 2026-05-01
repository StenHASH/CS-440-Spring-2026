using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSceneManager : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene"); 
    }

}
