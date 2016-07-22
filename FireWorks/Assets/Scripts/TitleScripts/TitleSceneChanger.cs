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
        SceneChange();

        Debug.Log("Title");
    }


    public void SceneChange()
    {
        if (Input.GetMouseButtonDown((int)MouseButtonNumber.LEFT_BUTTON))
        {
            SceneManager.LoadScene("Tutorial");

            Debug.Log("Title2");

        }
    }
}
