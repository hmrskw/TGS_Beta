using UnityEngine;
using System.Collections;

public class EndFireworksSeed : MonoBehaviour {
	[SerializeField]
	GameObject[] endFireworksImpacts;

	[SerializeField]
	float speed;

	MainSceneChanger mSceneChanger;

	SpriteRenderer seedSpriteRenderer;
	//Particle endFireworks;

	void Start () {
		mSceneChanger = new MainSceneChanger();
		seedSpriteRenderer = GetComponent<SpriteRenderer>();
		StartCoroutine(EndMain());
	}
	
	IEnumerator EndMain()
	{
		while(transform.position.x>=0){
			transform.Translate(-speed,speed,0);
			yield return null;
		}
		seedSpriteRenderer.enabled=false;
		for(int i = 0;i<endFireworksImpacts.Length;i++){
			Instantiate(endFireworksImpacts[i],transform.position,Quaternion.Euler(0,180,0));
		}
		yield return StartCoroutine(SceneChanger());
	}

	IEnumerator SceneChanger()
	{
		yield return new WaitForSeconds(5f);
		mSceneChanger.SceneChange("Result");
	}
}
