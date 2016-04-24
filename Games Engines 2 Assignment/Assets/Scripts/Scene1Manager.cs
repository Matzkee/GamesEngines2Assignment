using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scene1Manager : MonoBehaviour {

    public float sceneSwitchTime = 40;
    

	// Use this for initialization
	void Start () {

        StartCoroutine("SwitchScene");
	}

    IEnumerator SwitchScene() {

        yield return new WaitForSeconds(sceneSwitchTime);

        SceneManager.LoadScene("Scene2");
    }
}
