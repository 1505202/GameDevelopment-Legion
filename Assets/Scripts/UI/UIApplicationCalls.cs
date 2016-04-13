using UnityEngine;

//using UnityEngine.SceneManagement;
#pragma warning disable 618


// Temporary Left Without SceneManagment Due To Tony's Computer Not Having 5.3
public class UIApplicationCalls : MonoBehaviour 
{
    public void LoadLevel(int index)
    {
		Application.LoadLevel (index);
        //SceneManager.LoadScene(index);
    }

    public void LoadLevel(string levelName)
    {
		Application.LoadLevel (levelName);
        //SceneManager.LoadScene(levelName);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
