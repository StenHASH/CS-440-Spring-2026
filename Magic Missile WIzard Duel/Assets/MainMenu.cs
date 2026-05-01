using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Spellbook() 
    {
        
    }

    public void HostGame()
    {
        
    }

    public void JoinGame()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}