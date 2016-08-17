using UnityEngine;
using System.Collections;

public class ButtonExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(buttonClick);
	}

    private void buttonClick()
    {
        SettingsApplication.Save();
        Application.Quit();
    }
}
