using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Text1Script : MonoBehaviour {

    /// <summary>
    /// Текущий уровень
    /// </summary>
    public int nowlvl;
    /// <summary>
    /// Правильный ответ на вопрос текущего уровня
    /// </summary>
    public string answer_true;
    /// <summary>
    /// Xml документ с вопросами
    /// </summary>
    public System.Xml.XmlDocument doc;

    void Awake()
    {
        nowlvl = 0;
        answer_true = null;
        doc = new System.Xml.XmlDocument();
    }

    // Use this for initialization
    void Start()
    {
        int len = System.IO.Path.GetTempPath().Length;
        StartCoroutine(waitDownloadLevel(len));
        if (StaticInformation.filepath_xml_lvl == null || StaticInformation.filepath_xml_lvl.Length < len)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
            return;
        }
        System.IO.FileStream fs = new System.IO.FileStream(StaticInformation.filepath_xml_lvl, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        doc.Load(fs);
        docLoaded();
    }

    IEnumerator waitDownloadLevel(int len)
    {
        new WaitForSeconds(1);
        while(StaticInformation.filepath_xml_lvl == null || StaticInformation.filepath_xml_lvl.Length < len)
        {
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    public void provAnswerClickButton(string answer)
    {
        if (answer_true == answer)
        {
            SettingsApplication.addWin(nowlvl);
            NextLevel();
        }
        else
        {
            SettingsApplication.addGameOver();
        }
    }

    void NextLevel()
    {
        foreach (System.Xml.XmlNode nod in doc.DocumentElement)
        {
            if (nod.Name == "lvl")
            {
                int lvl = int.Parse(nod.FirstChild.InnerText);
                if (lvl > nowlvl && SettingsApplication.provObject(lvl) == false)
                {
                    nowlvl = lvl;
                    var text = gameObject.GetComponent<Text>();
                    text.text = nod.ChildNodes[1].InnerText;
                    Button[] button = (Button[])FindObjectsOfType(typeof(Button));
                    button = getSortButton(button);
                    string[] answer = nod.ChildNodes[2].InnerText.Split(',');
                    for (int i = 0; i < 4; i++)
                    {
                        button[i].transform.FindChild("Text").GetComponent<Text>().text = answer[i];
                    }
                    answer_true = nod.LastChild.InnerText;
                    return;
                }
            }
        }
        //Если дошли сюда то значит уровни закончились и это плохо
        StopGameBesauseNotLvl();
    }

    /// <summary>
    /// Тормозит игру потому, что уровни закончились
    /// </summary>
    private void StopGameBesauseNotLvl()
    {

    }

    private void docLoaded()
    {
        foreach(System.Xml.XmlNode nod in doc.DocumentElement)
        {
            if (nod.Name == "lvl")
            {
                int lvl = int.Parse(nod.FirstChild.InnerText);
                if (lvl > nowlvl && SettingsApplication.provObject(int.Parse(nod.FirstChild.InnerText)) == false)
                {
                    var text = gameObject.GetComponent<Text>();
                    nowlvl = lvl;
                    if (text == null) { Debug.Log("text == null is true"); }
                    text.text = nod.ChildNodes[1].InnerText;
                    Button[] button = (Button[])FindObjectsOfType(typeof(Button));
                    button = getSortButton(button);
                    string[] answer = nod.ChildNodes[2].InnerText.Split(',');
                    for (int i=0;i<4;i++)
                    {
                        button[i].transform.FindChild("Text").GetComponent<Text>().text = answer[i];
                    }
                    answer_true = nod.LastChild.InnerText;
                    break;
                }
            }
        }
    }

    private Button[] getSortButton(Button[] button)
    {
        int len = button.Length, lenminus = len - 1;
        for (int i=0;i<lenminus;i++)
        {
            for (int j=i+1;j<len;j++)
            {
                string name1 = button[i].name[button[i].name.Length - 1].ToString();
                int c1 = int.Parse(name1);
                string name2 = button[j].name[button[j].name.Length - 1].ToString();
                int c2 = int.Parse(name2);
                if (c2 < c1)
                {
                    var tmp = button[i];
                    button[i] = button[j];
                    button[j] = tmp; 
                }
            }
        }
        return button;
    }

    // Update is called once per frame
    void Update () {

	}

    void OnApplicationQuit ()
    {
        SettingsApplication.Save();
    }
}
