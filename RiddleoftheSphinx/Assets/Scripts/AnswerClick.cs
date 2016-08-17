using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerClick : MonoBehaviour {

    void Awake()
    {
        Debug.Log(gameObject.name + " awake");
        var button = gameObject.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            button.onClick.AddListener(buttonClickAnswer);
        }
    }

    /// <summary>
    /// Событие, которое произойдет при нажатие на кнопку
    /// </summary>
    public void buttonClickAnswer()
    {
        Debug.Log(gameObject.name + " click");
        var answer = gameObject.transform.GetComponentInChildren<UnityEngine.UI.Text>();
        if (answer != null && answer.text != null && answer.text.Length > 0)
        {
            var text_qest_obj = GameObject.Find("Scripts");
            if (text_qest_obj != null)
            {
                var text = text_qest_obj.GetComponent<Text1Script>();
                if (text != null)
                {
                    text.provAnswerClickButton(answer.text);
                }
            }
        }
    }
}
