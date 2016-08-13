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
        settingload = true;
    }

    private bool settingload = false;

    void Update ()
    {

    }

    void OnGUI ()
    {
        Debug.Log("ongui method");
        if (settingload)
        {
            float size = (Screen.height > Screen.width) ? Screen.width : Screen.height;
            size *= (float)0.30;
            GUIStyle style = GUI.skin.textArea;
            style.fontSize = 12;
            style.alignment = TextAnchor.MiddleCenter;
            Rect rect = new Rect(3, 3, size, style.lineHeight + style.fontSize);
            GUI.TextArea(rect, "WIN " + SettingsApplication.Win(), style);
            rect = new Rect(Screen.width - size - 3, 3, size, style.lineHeight + style.fontSize);
            GUI.TextArea(rect, "GAMEOVER " + SettingsApplication.Game_Over(), style);
        }
        float buttonwidth = (float)(Screen.width * 2.0 / 3.0);
        float buttonheight = 30;
        Rect buttonRect = new Rect((float)(Screen.width / 2.0 - buttonwidth / 2.0),
            (float)(Screen.height - buttonheight - buttonheight / 2), buttonwidth, buttonheight);
        if (GUI.Button(buttonRect, "Выход"))
        {
            Application.Quit();
        }
        //buttonRect = new Rect((float)(Screen.width / 2.0 - buttonwidth / 2.0),
        //    (float)(Screen.height - 1 -  2 * buttonheight - buttonheight / 2), buttonwidth, buttonheight);
        buttonRect.y -= (buttonheight + 1);
        if (GUI.Button(buttonRect, "Начать игру"))
        {
            Debug.Log("Load Game1 Scene start");
            SceneManager.LoadSceneAsync("Game1");
            Debug.Log("Load Game1 Scene end");
        }
    }
}
