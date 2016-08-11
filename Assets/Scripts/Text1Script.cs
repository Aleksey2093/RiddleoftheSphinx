using UnityEngine;
using System.Collections;
using System;

public class Text1Script : MonoBehaviour {

    // Use this for initialization
    void Start() {
        int i = 10;
        while (StaticInformation.filepath_xml_lvl == null || StaticInformation.filepath_xml_lvl == "")
        {
            i = 1;///h
        }
        Debug.Log("docLoad start");
        docLoad(StaticInformation.filepath_xml_lvl);
    }

    private void docLoad(string filepath_xml_lvl)
    {
        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        doc.Load(filepath_xml_lvl);
        var nod = doc.DocumentElement.ChildNodes[1];
        if (nod.Name == "lvl")
        {
            UnityEngine.UI.Text text = (UnityEngine.UI.Text)GameObject.Find("Text1").GetComponent(typeof(UnityEngine.UI.Text));
            if (text == null) { Debug.Log("text == null is true"); }
            text.text = nod.FirstChild.InnerText;
        }
    }

    // Update is called once per frame
    void Update () {

	}
}
