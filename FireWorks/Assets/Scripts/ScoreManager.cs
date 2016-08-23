using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    static int totalFireWorksNum;
    public static int TotalFireWorksNum { set {totalFireWorksNum = value;} get { return totalFireWorksNum;} }

    static int explosionNum;
    public static int ExplosionNum { set { explosionNum = value; } get { return explosionNum; } }

    static int score;
    public static int Score { private set { score = value; } get { return score; } }

	public static ScoreManager Instance
	{
		get; private set;
	}

	//シーンまたいでもオブジェクトが破棄されなくする
	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		init();
	}

    public static void init()
    {
        totalFireWorksNum = 0;
        explosionNum = 0;
        score = 0;
    }

	public static void AddScore(int score_)
    {
        if (explosionNum < totalFireWorksNum)
        {
            explosionNum++;
            score += score_;
        }
    }
}
