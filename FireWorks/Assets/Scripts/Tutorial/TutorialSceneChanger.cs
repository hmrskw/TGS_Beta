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
        //握ってるかどうかの判定
        //if (ReceivedZKOO.GetHand(ReceivedZKOO.HAND.RIGHT).isTouching)
        //{
            //SceneChange("Main");
        //}
    }

    public void SceneChange(string nextSceneName)
    {
            SceneManager.LoadScene(nextSceneName);
    }
}
