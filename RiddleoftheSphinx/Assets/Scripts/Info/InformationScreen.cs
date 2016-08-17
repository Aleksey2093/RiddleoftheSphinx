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
        float width = (Screen.height > Screen.width) ? Screen.width : Screen.height,
            height = 20 * Screen.dpi / 96f;
        width *= 45f / 100f;
        GUIStyle style = GUI.skin.textArea;
        style.fontSize = (int)(14f * (Screen.dpi / 96f));
        style.alignment = TextAnchor.MiddleCenter;
        Rect rect = new Rect(3, 3, width, height);
        GUI.TextArea(rect, "WIN " + SettingsApplication.Win(), style);
        rect = new Rect(Screen.width - width - 3, 3, width, height);
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
        float width = (Screen.width / 10f), height = (Screen.height / 10f);
       return new Rect(width, height, 
           (Screen.width - width - width), (Screen.height - height - height));
    }

    Rect windowRect;
 
    private void DialogWindow(int windowID)
    {
        var obj = GameObject.FindObjectOfType<Canvas>();
        if (obj != null && obj.isActiveAndEnabled)
            Destroy(obj);
        string label_text = (windowID == 1) ? "Уровни закончились. Ждите новые уровни." :
            "Проблема с загрузкой уровней.";
        float y = 40, h_element = 40 * Screen.dpi / 96;
        var label_style = GUI.skin.label;
        label_style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, y, windowRect.width, h_element), label_text);
        y += 1 + h_element;
        var button_style = GUI.skin.button;
        button_style.alignment = TextAnchor.MiddleCenter;
        if (GUI.Button(new Rect(0, y, windowRect.width, h_element), "Главное меню".ToString()))
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
        y += 1 + h_element;
        if (GUI.Button(new Rect(0, y, windowRect.width, h_element), "Выход".ToString()))
            Application.Quit();
    }

    void OnGUI()
    {
        showGUIInfoWinAndGameOver();
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
