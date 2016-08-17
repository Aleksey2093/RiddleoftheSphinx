using UnityEngine;
using System.Collections;

public class ButtonSettings : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(buttonClick);
    }

    private void buttonClick()
    {
        //Должны запустить сцену настроек которой у нас пока нет
    }
}
