using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    //literally have ti hardcode this sh!t
    public void loadMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void quitOnClick()
    {

        // if running in the Unity editor, do this
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        //if running in the build version
#else
        Application.Quit ();
#endif
    }
}
