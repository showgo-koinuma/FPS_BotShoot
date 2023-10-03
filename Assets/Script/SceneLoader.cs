using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName == "Title")
        {
            SceneManager.MoveGameObjectToScene(DDOLController.instance.gameObject, SceneManager.GetActiveScene());
            Time.timeScale = 1;
            SceneManager.sceneLoaded -= DDOLGameManagerController.instans.SceneLoaded;
            DDOLGameManagerController.instans = null;
            Destroy(DDOLGameManagerController.instans);
            DDOLController.instance = null;
            Destroy(DDOLController.instance);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        SceneManager.LoadScene(sceneName);
    }
}
