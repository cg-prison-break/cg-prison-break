using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private string startAnimationScene = "StartAnimation";
    private string mainScene = "MainScene";
    [SerializeField] private GameObject mainMenuRoot;     // assign MainMenuRoot
    [SerializeField] private GameObject optionsPanel;     // assign OptionsPanel
    [SerializeField] private GameObject optionsFirstSelectable; // e.g., ResolutionDropdown
    [SerializeField] private GameObject menuFirstSelectable;    // e.g., PlayButton
    
    [SerializeField] private GameData gameData;

    public void PlayGame()
    {
        gameData.strikes = 0;
        gameData.timer = 0.0f;
        SceneManager.LoadScene(gameData.animationPlayed ? mainScene : startAnimationScene);
    } 
        

    public void OpenOptions()
    {
        mainMenuRoot.SetActive(false);
        optionsPanel.SetActive(true);
        if (optionsFirstSelectable) EventSystem.current.SetSelectedGameObject(optionsFirstSelectable);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuRoot.SetActive(true);
        if (menuFirstSelectable) EventSystem.current.SetSelectedGameObject(menuFirstSelectable);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}