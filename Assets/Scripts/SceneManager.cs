using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public void ChangeToScene(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("BuffScene");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
