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
        SceneChange();
    }

    public void SceneChange()
    {
        if (Input.GetMouseButtonDown((int)MouseButtonNumber.LEFT_BUTTON))
        {
            //TIPS:三澤側でメインを実装したら非コメント化
            //SceneManager.LoadScene("Main");
            SceneManager.LoadScene("Result");
        }
    }
}
