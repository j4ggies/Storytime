using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    // Attach canvas
    public GameObject canvas;

    // Singleton
    static SceneController instance;

    void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static SceneController GetInstance () {
        return instance;
    }

    public void LoadScene(int sceneIndex) {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    // Async method
    IEnumerator LoadAsync (int sceneIndex) {
        // Set visible
        canvas.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until scene fully loaded
        while (!op.isDone) {
            yield return null;
        }
        // Set invisible
        canvas.SetActive(false);
    }
}
