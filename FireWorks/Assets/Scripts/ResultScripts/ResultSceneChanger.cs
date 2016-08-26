using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class ResultSceneChanger : MonoBehaviour, ISceneBase
{
    enum MouseButtonNumber
    {
        LEFT_BUTTON = 0,
        RIGHT_BUTTON
    }

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            SceneChange("Title");
        }*/
    }

    public void SceneChange(string nextSceneName)
    {
            SceneManager.LoadScene(nextSceneName);
    }

}

