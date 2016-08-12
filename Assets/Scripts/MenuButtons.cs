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
        loadFilesQuests();
        loadSettingsFile();
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
    private void loadFilesQuests()
    {
        StaticInformation.LevelXml.loadDataFromFileFromSite();
    }

    void Update ()
    {
        
    }

    void OnGUI ()
    {
        int buttonwitdth = (int)(Screen.width * 2.0 / 3.0);
        int buttonheaight = (int)(30);
        Rect buttonRect = new Rect((float)(Screen.width / 2.0 - buttonwitdth / 2.0),
            (float)(2.0 * Screen.height / 3.0 - buttonheaight / 2.0), buttonwitdth, buttonheaight);
        if (GUI.Button(buttonRect, "Начать игру"))
        {
            Debug.Log("Load Game1 Scene start");
            SceneManager.LoadScene("Game1");
            Debug.Log("Load Game1 Scene end");
        }
        buttonRect = new Rect((float)(Screen.width / 2.0 - buttonwitdth / 2.0),
            (float)(2.0 * Screen.height / 3.0 + buttonheaight / 2.0), buttonwitdth, buttonheaight);
        if (GUI.Button(buttonRect, "Выход"))
        {
            Application.Quit();
        }
    }
}
