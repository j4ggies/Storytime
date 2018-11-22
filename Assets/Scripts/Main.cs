using UnityEngine;

public class Main : MonoBehaviour {

	// Load menu at start
	void Start () {
        SceneController.GetInstance().LoadScene(1);	
	}
}
