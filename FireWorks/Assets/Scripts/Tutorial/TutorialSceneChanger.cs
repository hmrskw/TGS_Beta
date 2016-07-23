using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class TutorialSceneChanger : MonoBehaviour, ISceneBase 
{
    enum MouseButtonNumber
    {
        LEFT_BUTTON = 0,
        RIGHT_BUTTON
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButtonNumber.LEFT_BUTTON))
        {
            SceneChange("Main");
        }
    }

    public void SceneChange(string nextSceneName)
    {
            SceneManager.LoadScene(nextSceneName);
    }
}
