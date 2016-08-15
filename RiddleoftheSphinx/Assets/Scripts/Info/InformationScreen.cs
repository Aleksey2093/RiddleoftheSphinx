using UnityEngine;
using System.Collections;

public class InformationScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    StaticInformation.LevelXml.Reslvl reslvl = StaticInformation.LevelXml.Reslvl.Ok;

    public void SetValue_ResLvl(StaticInformation.LevelXml.Reslvl value)
    {
        reslvl = value;
    }

    /// <summary>
    /// Отображает в верхней части экрана информацию о пройденных уровнях и прочие
    /// </summary>
    void showGUIInfoWinAndGameOver()
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

    /// <summary>
    /// Остановка игры потому, что уровни по какой-то причине отсутствуют.
    /// </summary>
    private void stopGameBesuseNotLevelCoutNull()
    {
        windowRect = getPositionDialogWindow();
        windowRect = GUI.ModalWindow(0, windowRect, DialogWindow, "Упс");
    }

    /// <summary>
    /// Тормозит игру потому, что уровни закончились
    /// </summary>
    private void stopGameBesauseEndLvl()
    {
        windowRect = getPositionDialogWindow();
        windowRect = GUI.ModalWindow(1, windowRect, DialogWindow, "Конец");
    }

    private Rect getPositionDialogWindow()
    {
       return new Rect((float)(Screen.width / 5.0), (float)(Screen.height / 5.0), 
           (float)(Screen.width * 3.0 / 4.0), (float)(Screen.height * 3.0 / 4.0));
    }

    Rect windowRect;
 
    private void DialogWindow(int windowID)
    {
        string label_text = (windowID == 1) ? "Уровни закончились. Ждите новые уровни." :
            "Проблема с загрузкой уровней.";
        float y = 20;
        var label_style = GUI.skin.label;
        label_style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(5, y, windowRect.width, 20), label_text, label_style);
        y += 21;
        var button_style = GUI.skin.button;
        button_style.alignment = TextAnchor.MiddleCenter;
        if (GUI.Button(new Rect(5, y, windowRect.width - 10, 20), "Главное меню".ToString(), button_style))
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
        y += 21;
        if (GUI.Button(new Rect(5, y, windowRect.width - 10, 20), "Выход".ToString(), button_style))
            Application.Quit();
    }

    void OnGUI()
    {
        if (SettingsApplication.get_loadSetting())
        {
            showGUIInfoWinAndGameOver();
        }
        switch (reslvl)
        {
            case StaticInformation.LevelXml.Reslvl.Ok:
                break;
            case StaticInformation.LevelXml.Reslvl.No_Lvl:
                stopGameBesuseNotLevelCoutNull();
                break;
            case StaticInformation.LevelXml.Reslvl.End_lvl:
                stopGameBesauseEndLvl();
                break;
            default:
                break;
        }
    }
}
