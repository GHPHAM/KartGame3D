using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private VisualElement _pauseMenu;
    private VisualElement _pauseMenuBG;
    private bool isPaused = false;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _pauseMenu = root.Q<VisualElement>("Foreground");
        _pauseMenuBG = root.Q<VisualElement>("Background");

        root.Q<Button>("Play").clicked += () =>
        {
            if (isPaused)
                CloseMenu();
            else
                OpenMenu();
            isPaused = !isPaused;
        };
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                CloseMenu();
            else
                OpenMenu();
            isPaused = !isPaused;
        }
    }

    private void OpenMenu()
    {
        _pauseMenu.style.translate = new StyleTranslate(new Translate(new Length(0), new Length(0)));
        _pauseMenuBG.AddToClassList("pause-bg--visible");
        Time.timeScale = 0f;
    }

    private void CloseMenu()
    {
        _pauseMenu.style.translate = new StyleTranslate(new Translate(new Length(-100, LengthUnit.Percent), new Length(0)));
        _pauseMenuBG.RemoveFromClassList("pause-bg--visible");
        Time.timeScale = 1f;
    }
}