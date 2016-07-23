using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	public void Exit()
    {
        Application.Quit();
    }
    public void Retry()
    {
        SceneManager.LoadScene("Main");
    }
}
