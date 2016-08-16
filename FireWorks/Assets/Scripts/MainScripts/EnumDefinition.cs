using UnityEngine;
using System.Collections;

public class EnumDefinition : MonoBehaviour
{
    public enum FireｗorksType
    {
        BOTAN,                 //牡丹
        DOSEI,                 //土星
        KARAI,                 //花雷
        KIKU,                  //菊
        SINIRI_KIKU,           //芯入り菊
        MANGEKYOU,             //万華鏡

        /*        
                NAIAGARA,          //ナイアガラ
                SINIRI_GINKAMURO_GIKU, //芯入り銀冠菊
                YAE_SIN_GIKU,          //八重芯菊
                NISIKI_KAMURO_GIKU,    //錦冠菊
                HACHI,                 //蜂
                YANAGI,                //柳
                SENKOU,                //閃光
                KAMURO_GIKU,           //冠菊
                NOBORI_RYU,            //昇竜
                NOBORI_BUNKA,          //昇分花
                SENRIN_GIKU,           //千輪菊
        */
        NONE_TYPE = -1
    }

    public enum ShotAngle
    {
        RIGHT = 0,  //右
        CENTER,     //真ん中
        LEFT,       //左

        NONE_ANGLE = -1
    }
}
