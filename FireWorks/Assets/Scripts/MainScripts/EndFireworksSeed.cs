using UnityEngine;
using System.Collections;

public class EndFireworksSeed : MonoBehaviour {
	[SerializeField]
	GameObject[] endFireworksImpacts = new GameObject[2];

    [SerializeField]
    GameObject aroundFireworksWordImpacts;

    [SerializeField]
	float speed;

	MainSceneChanger mSceneChanger;

	ParticleSystem endFireworksSeed;


	void Start () {
		mSceneChanger = new MainSceneChanger();
        endFireworksSeed = GetComponent<ParticleSystem>();
		StartCoroutine(EndMain());
	}
	
	IEnumerator EndMain()
	{
		while(transform.position.x>=0){
			transform.Translate(-speed*1.5f,speed,0);
			yield return null;
		}
        for (int i = 0; i < endFireworksImpacts.Length; i++)
        {
                Instantiate(endFireworksImpacts[i], transform.position, Quaternion.Euler(0, 180, 0));
        }
        for (int i = 1; i <= 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector2 sign = new Vector2(1 - (j * 2), 3 - (i * 2));
                Instantiate(aroundFireworksWordImpacts, transform.position - new Vector3(10 * i * sign.x, 5 * i * sign.y, 0), Quaternion.Euler(0, 180, 0));
            }
        }

        endFireworksSeed.Stop();
        yield return StartCoroutine(SceneChanger());
	}

	IEnumerator SceneChanger()
	{
		yield return new WaitForSeconds(5f);
		mSceneChanger.SceneChange("Result");
	}
}
