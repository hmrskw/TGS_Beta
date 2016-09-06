using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LockonRank : MonoBehaviour {
    [SerializeField]
    float drawTime;

    [SerializeField]
    AnimationCurve alphaCurve;

    float startTime;
    public float StartTime
    {
        set { startTime = value; }
    }

    Image image;

	// アクティブになった時に初期化
	void OnEnable() {
        startTime = Time.timeSinceLevelLoad;
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
    }
	
	// フェードして画像を表示
	void Update () {
        var diff = Time.timeSinceLevelLoad - startTime;

        if (diff > drawTime)
        {
            gameObject.SetActive(false);
        }

        var rate = diff / drawTime;
        float alpha = alphaCurve.Evaluate(rate);

        image.color = new Color(1,1,1,alpha);
    }
}
