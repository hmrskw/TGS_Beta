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
        if (ReceivedZKOO.GetHand(ReceivedZKOO.HAND.RIGHT).isTouching)
        {
            SceneChange("Title");
        }
    }

    public void SceneChange(string nextSceneName)
    {
            SceneManager.LoadScene(nextSceneName);
    }

}

