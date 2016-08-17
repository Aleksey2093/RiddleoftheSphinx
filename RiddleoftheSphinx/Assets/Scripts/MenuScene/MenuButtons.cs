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
}
