using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class MainSceneChanger : MonoBehaviour , ISceneBase
{
    enum MouseButtonNumber
    {
        LEFT_BUTTON = 0,
        RIGHT_BUTTON
    }

    public void SceneChange(string nextSceneName)
    {
        SceneManager.LoadScene("Result");
    }
}
