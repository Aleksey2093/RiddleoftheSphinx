using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MenuButtons : MonoBehaviour {

   
    void Start ()
    {
        loadFilesQuests();
        loadSettingsFile();
    }

    private void loadSettingsFile()
    {
        SettingsApplication.loadSettingFile();
    }

    /// <summary>
    /// Загрузка файлов с вопросами и ответами с сервера разработчика.
    /// </summary>
    private void loadFilesQuests()
    {
        try
        {
            System.Net.WebClient webClient = new System.Net.WebClient();
            string file_tmp = System.IO.Path.GetTempFileName();
            webClient.DownloadFileCompleted += (obj, args) => { if (args.Error == null)
                {
                    Debug.Log("Download Complite. Path xml: " + file_tmp);
                    StaticInformation.filepath_xml_lvl = file_tmp;
                }
            else
                {
                    Debug.Log("Download Error. Ex: " + args.Error.Message);
                    StaticInformation.filepath_xml_lvl = null;
                }
            };
            webClient.DownloadFileAsync(new Uri("http://2014.ucoz.org/file_c/game/unity/level.xml"), file_tmp);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    void Update ()
    {
        
    }

    void OnGUI ()
    {
        int buttonwitdth = (int)(Screen.width * 2.0 / 3.0);
        int buttonheaight = (int)(60);
        Rect buttonRect = new Rect((float)(Screen.width / 2.0 - buttonwitdth / 2.0),
            (float)(2.0 * Screen.height / 3.0 - buttonheaight / 2.0 - 1), buttonwitdth, buttonheaight);
        if (GUI.Button(buttonRect, "Начать игру"))
        {
            Debug.Log("Load Game1 Scene start");
            SceneManager.LoadScene("Game1");
            Debug.Log("Load Game1 Scene end");
        }
        buttonRect = new Rect((float)(Screen.width / 2.0 - buttonwitdth / 2.0),
            (float)(2.0 * Screen.height / 3.0 + buttonheaight / 2.0 + 1), buttonwitdth, buttonheaight);
        if (GUI.Button(buttonRect, "Выход"))
        {
            Application.Quit();
        }
    }
}
