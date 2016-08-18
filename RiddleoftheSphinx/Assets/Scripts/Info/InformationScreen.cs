using UnityEngine;
using System.Collections;

public class InformationScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        dpiScreenDev96 = Screen.dpi / 96;
        height_win_gameover_information = 20 * dpiScreenDev96;
        h_element_dialog = y_dialog_win * dpiScreenDev96;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    StaticInformation.LevelXml.Reslvl reslvl = StaticInformation.LevelXml.Reslvl.Ok;

    public void SetValue_ResLvl(StaticInformation.LevelXml.Reslvl value)
    {
        reslvl = value;
    }

    float height_win_gameover_information;

    /// <summary>
    /// Отображает в верхней части экрана информацию о пройденных уровнях и прочие
    /// </summary>
    void showGUIInfoWinAndGameOver()
    {
        float width = (Screen.height > Screen.width) ? Screen.width : Screen.height;
        width *= 0.45f;
        GUIStyle style = GUI.skin.box;
        style.fontSize = (int)(style.fontSize * dpiScreenDev96);
        style.alignment = TextAnchor.MiddleCenter;
        Rect rect = new Rect(3, 3, width, height_win_gameover_information);
        GUI.Box(rect, "WIN " + SettingsApplication.Win(), style);
        rect = new Rect(Screen.width - width - 3, 3, width, height_win_gameover_information);
        GUI.Box(rect, "GAMEOVER " + SettingsApplication.Game_Over(), style);
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
        float width = (Screen.width / 10f), height = (Screen.height / 10f);
        return new Rect(width, height,
            (Screen.width - width - width), (Screen.height - height - height));
    }

    Rect windowRect;
    float dpiScreenDev96;
    float y_dialog_win = 40, h_element_dialog;

    private void DialogWindow(int windowID)
    {
        var mass = GameObject.FindGameObjectsWithTag("ElementDestroyGameDialogWindow");
        for (int i = 0; i < mass.Length; i++)
            Destroy(mass[i]);
        string label_text = (windowID == 1) ? "Уровни закончились. Ждите новые уровни." :
            "Проблема с загрузкой уровней.";

        var label_style = GUI.skin.label;
        label_style.alignment = TextAnchor.MiddleCenter;
        int sizefont = (int)(12 * dpiScreenDev96);
        label_style.fontSize = sizefont;

        GUI.Label(new Rect(0, 10, windowRect.width, h_element_dialog), label_text);

        float y = 1 + h_element_dialog;
        var button_style = GUI.skin.button;
        button_style.alignment = TextAnchor.MiddleCenter;
        button_style.fontSize = sizefont;

        if (GUI.Button(new Rect(0, y, windowRect.width, h_element_dialog), "Главное меню".ToString()))
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");

        y += 1 + h_element_dialog;

        if (GUI.Button(new Rect(0, y, windowRect.width, h_element_dialog), "Выход".ToString()))
            Application.Quit();
    }

    void OnGUI()
    {
        showGUIInfoWinAndGameOver();
        switch (reslvl)
        {
            case StaticInformation.LevelXml.Reslvl.No_Lvl:
                stopGameBesuseNotLevelCoutNull();
                break;
            case StaticInformation.LevelXml.Reslvl.End_lvl:
                stopGameBesauseEndLvl();
                break;
        }
    }

    void OnApplicationQuit()
    {
        SettingsApplication.Save();
    }
}
