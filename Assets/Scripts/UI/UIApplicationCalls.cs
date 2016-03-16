using UnityEngine;

using UnityEngine.SceneManagement;

public class UIApplicationCalls : MonoBehaviour 
{
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
