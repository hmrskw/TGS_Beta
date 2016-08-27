using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ResultText : MonoBehaviour {
    [SerializeField, Tooltip("スコアのテキスト")]
    Text scoreText;

    [SerializeField, Tooltip("成功率のテキスト")]
    Text successRateText;

	[SerializeField]
	Text text;

    void Start () {
        float rate = ((float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum) * 100;
		int point = (int)(ScoreManager.Score* ((float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum));
        scoreText.text = "爆破数 : " + ScoreManager.ExplosionNum.ToString().PadLeft(2, '0') +" / " +ScoreManager.TotalFireWorksNum.ToString().PadLeft(2, '0');
        successRateText.text = "成功率 : " + Convert.ToInt16(rate) .ToString().PadLeft(3, ' ')+"%";
		text.text = "得点 : " + Convert.ToInt16(point) .ToString().PadLeft(3, ' ')+"%";
    }
}
