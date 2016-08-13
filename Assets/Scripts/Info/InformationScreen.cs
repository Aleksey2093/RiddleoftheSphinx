using UnityEngine;
using System.Collections;

public class InformationScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (SettingsApplication.get_loadSetting())
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
    }
}
