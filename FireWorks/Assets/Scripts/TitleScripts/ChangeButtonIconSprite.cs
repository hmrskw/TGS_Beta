using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class ChangeButtonIconSprite : MonoBehaviour
{
    enum ButtonIcon
    {
        HOLD_HAND = 0,
        OPEN_HAND
    }

    const int ICON_NUM = 2;

    //ButtonIcon buttonIconNum = ButtonIcon.HOLD_HAND;

    [SerializeField]
    Sprite[] buttonIcon = new Sprite[ICON_NUM];

    [SerializeField]
    TitleSceneChanger titleSceneChanger;

    //private Image viewIcon;
    private SpriteRenderer viewIcon;
    //カウントする変数
    //private int changeCountNum = 0;

    //1秒
    private const int CHANGE_TIME = 60;

    //画像を変えるかどうか
    //private bool canChange = false;

    void Start()
    {
        viewIcon = GetComponent<SpriteRenderer>();
        
        viewIcon.sprite = buttonIcon[0];

        StartCoroutine(SpriteChangeTimer());
    }

    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ReceivedZKOO.Handedness = (ReceivedZKOO.HAND)i;
                titleSceneChanger.SceneChange("Tutorial");
            }
        }
    }

    IEnumerator SpriteChangeTimer()
    {
        while (true)
        {
            viewIcon.sprite = buttonIcon[1];

            yield return new WaitForSeconds(1.0f);

            viewIcon.sprite = buttonIcon[0];

            yield return new WaitForSeconds(1.0f);
        }
    }

    //bool RayCast(ReceivedZKOO.HAND id)
    //{
    //    //カメラの場所からポインタの場所に向かってレイを飛ばす
    //    Ray ray = Camera.main.ScreenPointToRay(/*new Vector2(ReceivedZKOO.GetHand(id).position.x, ReceivedZKOO.GetHand(id).position.y + Screen.height)*/Input.mousePosition);
    //    RaycastHit hit = new RaycastHit();

    //    //レイが何か当たっているかを調べる
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        //当たったオブジェクトを格納
    //        GameObject obj = hit.collider.gameObject;
            
    //        if (obj.name == "GrappHand")
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}
}
