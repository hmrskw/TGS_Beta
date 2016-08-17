using UnityEngine;
using System.Collections;

public class FireworksDeflection : MonoBehaviour
{
    [SerializeField, Tooltip("花火が打ちあがるときの振れ幅。")]
    private float moveSpeed;
    
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(this.transform.position.x + Mathf.Sin(Time.frameCount * moveSpeed), 
                                         this.transform.position.y, 
                                         this.transform.position.z);
    }
}
