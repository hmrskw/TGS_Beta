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
        SceneChange();
    }

    public void SceneChange()
    {
        if (Input.GetMouseButtonDown((int)MouseButtonNumber.LEFT_BUTTON))
        {
            SceneManager.LoadScene("Title");
        }
    }

}

