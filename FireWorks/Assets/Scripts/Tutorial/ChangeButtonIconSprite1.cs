using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class ChangeButtonIconSprite1 : MonoBehaviour
{
    const int ICON_NUM = 2;

    [SerializeField]
    Texture[] buttonIcon = new Texture[ICON_NUM];

    private Renderer viewRenderer;

    void OnEnable()
    {
        viewRenderer = GetComponent<Renderer>();

        viewRenderer.material.mainTexture = buttonIcon[0];

        StartCoroutine(SpriteChangeTimer());
    }

    IEnumerator SpriteChangeTimer()
    {
        while (true)
        {
            viewRenderer.material.mainTexture = buttonIcon[1];

            yield return new WaitForSeconds(1.0f);

            viewRenderer.material.mainTexture = buttonIcon[0];

            yield return new WaitForSeconds(1.0f);
        }
    }
}
