using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class TitleSceneChanger : MonoBehaviour, ISceneBase
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
            SceneChange("Tutorial");
        }
    }

    public void SceneChange(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
