using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    // Attach buttons
    public Button startButton;
    public Button quitButton;

    void Start()
    {
        startButton.onClick.AddListener(StartLevel);
        quitButton.onClick.AddListener(QuitApplication);
    }

    // Loasd main scene
    void StartLevel()
    {
        SceneController.GetInstance().LoadScene(2);
    }

    // Quit
    void QuitApplication()
    {
        Application.Quit();
    }
}