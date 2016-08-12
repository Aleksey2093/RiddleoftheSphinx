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
        int buttonwitdth = (int)(Screen.width * 2.0 / 3.0);
        int buttonheight = (int)(30);
        Rect buttonRect = new Rect((float)(Screen.width / 2.0 - buttonwitdth / 2.0),
            (float)(Screen.height - 2 * buttonheight - buttonheight / 2), buttonwitdth, buttonheight);
        if (GUI.Button(buttonRect, "Начать игру"))
        {
            Debug.Log("Load Game1 Scene start");
            SceneManager.LoadSceneAsync("Game1");
            Debug.Log("Load Game1 Scene end");
        }
        buttonRect = new Rect((float)(Screen.width / 2.0 - buttonwitdth / 2.0),
            (float)(Screen.height - buttonheight - buttonheight / 2), buttonwitdth, buttonheight);
        if (GUI.Button(buttonRect, "Выход"))
        {
            Application.Quit();
        }
    }
}
