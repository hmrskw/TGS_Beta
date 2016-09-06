using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class TitleSceneChanger : MonoBehaviour, ISceneBase
{
    void Update()
    {
        /*//FIX:タイトルだけ手のIDが反転する（原因不明）
        if (ReceivedZKOO.OpenHand(ReceivedZKOO.HAND.RIGHT))
        {
           ReceivedZKOO.Handedness = ReceivedZKOO.HAND.LEFT;
            SceneChange("Tutorial");
        }
        else if (ReceivedZKOO.OpenHand(ReceivedZKOO.HAND.LEFT))
       {
            ReceivedZKOO.Handedness = ReceivedZKOO.HAND.RIGHT;
           SceneChange("Tutorial");
        }*/
    }

    public void SceneChange(string nextSceneName)
    {
            SceneManager.LoadScene(nextSceneName);
    }
}