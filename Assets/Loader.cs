using UnityEngine;

public class Loader : MonoBehaviour
{

    // Load menu at start
    void Start()
    {
        SceneController.GetInstance().LoadScene(1);
    }
}