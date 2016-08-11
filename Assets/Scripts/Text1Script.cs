using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Text1Script : MonoBehaviour {

    private int nowlvl = 0;
    private string answer_true;
    private System.Xml.XmlDocument doc;

    // Use this for initialization
    void Start()
    {
        doc = new System.Xml.XmlDocument();
        int len = System.IO.Path.GetTempPath().Length;
        WaitWhile wait = new WaitWhile(() => { return (StaticInformation.filepath_xml_lvl == null || StaticInformation.filepath_xml_lvl.Length < len); });
        if (StaticInformation.filepath_xml_lvl == null || StaticInformation.filepath_xml_lvl.Length < len)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
            return;
        }
        doc.Load(StaticInformation.filepath_xml_lvl);
        docLoaded();
    }

    void NextLevel()
    {
        foreach(System.Xml.XmlNode nod in doc.DocumentElement)
        {
            if (nod.Name == "lvl")
            {
                int lvl = int.Parse(nod.FirstChild.InnerText);
                if (lvl > nowlvl && SettingsApplication.provObject(lvl) == false)
                {
                    var text = gameObject.GetComponent<Text>();
                    if (text == null) { Debug.Log("text == null is true"); }
                    text.text = nod.ChildNodes[1].InnerText;
                    Button[] button = (Button[])FindObjectsOfType(typeof(Button));
                    button = getSortButton(button);
                    string[] answer = nod.ChildNodes[2].InnerText.Split(',');
                    for (int i = 0; i < 4; i++)
                    {
                        button[i].transform.FindChild("Text").GetComponent<Text>().text = answer[i];
                    }
                    answer_true = nod.LastChild.InnerText;
                    break;
                }
            }
        }
    }

    private void docLoaded()
    {
        foreach(System.Xml.XmlNode nod in doc.DocumentElement)
        {
            if (nod.Name == "lvl")
            {
                if (SettingsApplication.provObject(int.Parse(nod.FirstChild.InnerText)) == false)
                {
                    var text = gameObject.GetComponent<Text>();
                    nowlvl = int.Parse(nod.FirstChild.InnerText);
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
}
