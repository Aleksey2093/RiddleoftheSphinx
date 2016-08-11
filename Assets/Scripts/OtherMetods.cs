using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OtherMetods : MonoBehaviour {
    public static string getPathDir
    {
        get
        {
            string extract = System.IO.Path.GetTempPath(); //возвращает путь к временной папке
            extract = System.IO.Path.Combine(extract, "RiddleoftheSphinx"); //возвращает путь к папке, которая будет создана во временной папке
            System.IO.Directory.CreateDirectory(extract); //создаем эту директорию, чтобы потом не забыть сделать это других методах
            return extract;
        }
    }
}