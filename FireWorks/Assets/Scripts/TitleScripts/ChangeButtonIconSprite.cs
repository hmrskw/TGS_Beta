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

    ButtonIcon buttonIconNum = ButtonIcon.HOLD_HAND;

    [SerializeField]
    Sprite[] buttonIcon = new Sprite[ICON_NUM];

    private Image viewIcon;
    
    //カウントする変数
    private int changeCountNum = 0;

    //1秒
    private const int CHANGE_TIME = 60;

    //画像を変えるかどうか
    private bool canChange = false;


    void Start()
    {
        viewIcon = GetComponent<Image>();
        
        viewIcon.sprite = buttonIcon[0];

        StartCoroutine(SpriteChangeTimer());
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
}
