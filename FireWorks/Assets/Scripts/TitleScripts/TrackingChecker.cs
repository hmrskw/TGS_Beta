using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrackingChecker : MonoBehaviour {
    [SerializeField]
    Image[] checker = new Image[2];

	void Awake () {
		Application.targetFrameRate = 30;
    }

    void Update () {
        if (ReceivedZKOO.GetHand(ReceivedZKOO.HAND.RIGHT).isTracking)
        {
            checker[0].color = Color.green;
        }
        else
        {
            checker[0].color = Color.red;
        }
        if (ReceivedZKOO.GetHand(ReceivedZKOO.HAND.LEFT).isTracking)
        {
            checker[1].color = Color.green;
        }
        else
        {
            checker[1].color = Color.red;
        }
    }
}
