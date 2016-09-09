using UnityEngine;
using System.Collections;

public class MainGameManager : MonoBehaviour {

    private static MainGameManager instance;

    public static MainGameManager Instance
    {
        get { return instance; }
    }

    [SerializeField, Tooltip("時間制限")]
    int timeLimit;
    public int TimeLimit
    {
        get { return timeLimit; }
    }

    //シーン内での経過時間
    float gameTime;
    public float GameTime
    {
        get { return gameTime; }
    }

    bool isEnd;
    public bool IsEnd
    {
        get { return isEnd; }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start () {
        isEnd = false;
        gameTime = 0;
    }

    void Update () {
        //時間の更新
        gameTime += Time.deltaTime;
        //isEnd = (timeLimit >= gameTime);
    }

    public GameObject RayCast()
    {
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        //レイが何か当たっているかを調べる
        if (Physics.Raycast(ray, out hit))
        {
            return (hit.collider.gameObject);
            //当たったオブジェクトを格納
            /*string objName = hit.collider.gameObject.name;
            for (int i = 0;i < targetObjName.Length; i++) {
                if (objName == targetObjName[i])
                {
                    return true;
                    //当たった時に実行したい関数();
                }
            }*/
        }
        return null;
    }
}
