using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MenuButtons : MonoBehaviour {

    void Awake()
    {
        StaticInformation.downloaddonelevels = false;
    }

    void Start ()
    {
        loadSettingsFile();
        loadFilesQuestsFromNet();
    }

    /// <summary>
    /// Загрузка файла настроек
    /// </summary>
    private void loadSettingsFile()
    {
        SettingsApplication.loadSettingFile();
    }

    /// <summary>
    /// Загрузка файлов с вопросами и ответами с сервера разработчика.
    /// </summary>
    private void loadFilesQuestsFromNet()
    {
        StaticInformation.LevelXml.loadDataFromFileFromSite();
    }

    void Update ()
    {

    }

    void OnGUI ()
    {
        float buttonwidth = (Screen.width * 2 / 3);
        float buttonheight = 30 * Screen.dpi / 96;
        Debug.Log(string.Format("button height: {0}", buttonheight));
        Rect buttonRect = new Rect((Screen.width / 2 - buttonwidth / 2),
            (Screen.height - buttonheight - buttonheight / 2), buttonwidth, buttonheight);
        if (GUI.Button(buttonRect, "Выход"))
        {
            Application.Quit();
        }
        buttonRect.y -= (buttonheight + 1);
        if (GUI.Button(buttonRect, "Начать игру"))
        {
            Debug.Log("Load Game1 Scene start");
            SceneManager.LoadSceneAsync("Game1");
            Debug.Log("Load Game1 Scene end");
        }
    }
}
