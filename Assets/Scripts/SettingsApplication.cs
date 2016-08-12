using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SettingsApplication : MonoBehaviour
{
    /// <summary>
    /// Победы
    /// </summary>
    private static int win = 0;
    /// <summary>
    /// Поражения
    /// </summary>
    private static int game_over = 0;
    /// <summary>
    /// Номера пройденных уровней
    /// </summary>
    private static List<int> saveQuestNumberWin;
    /// <summary>
    /// Указатель на то, что сейчас проиходит сохранение
    /// </summary>
    private static bool saveNow = false;

    private static string absoluteUrlApplicationFileSetting;

    /// <summary>
    /// Создает новый файл настроек в папке с приложением, если его нет
    /// </summary>
    /// <param name="filepath">Ссылка на файл</param>
    /// <returns></returns>
    private static bool createFile(string filepath)
    {
        return (createFile(filepath, win, game_over));
    }

    /// <summary>
    /// Создает новый файл настроек в папке с приложением, если его нет
    /// </summary>
    /// <param name="filepath">Ссылка на файл</param>
    /// <param name="win">Победы</param>
    /// <param name="game_over">Поражения</param>
    /// <returns></returns>
    private static bool createFile(string filepath, int win, int game_over)
    {
        try
        {
            System.IO.File.Delete(filepath);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        try
        {
            System.IO.File.WriteAllLines(filepath,
                new string[]
                {
                        "<saveFileApplication>",
                        string.Format("   <WIN>{0}</WIN>", win),
                        string.Format("   <GAMEOVER>{0}</GAMEOVER>", game_over),
                        "</saveFileApplication>"
                });
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return false;
        }
    }

    /// <summary>
    /// Возвращает путь к файлу настроек
    /// </summary>
    /// <returns></returns>
    private static string getFileSettingsPath()
    {
        if (absoluteUrlApplicationFileSetting == null || absoluteUrlApplicationFileSetting.Length == 0)
        {
            string filepath = Application.absoluteURL;
            int len = System.IO.Path.GetFileName(filepath).Length;
            filepath = filepath.Substring(0, filepath.Length - len);
            filepath = System.IO.Path.Combine(filepath, "xml_settings.saveSphinx");
            absoluteUrlApplicationFileSetting = filepath;
            return filepath;
        }
        else
            return absoluteUrlApplicationFileSetting;
    }

    /// <summary>
    /// Загружает файл настроек с диска в память
    /// </summary>
    public static void loadSettingFile()
    {
        string filepath = getFileSettingsPath();
        var doc = new System.Xml.XmlDocument();
        System.IO.FileStream fs;
        try
        {
            fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            doc.Load(fs);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            if (createFile(filepath))
            {
                fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                doc.Load(fs);
            }
        }
        bool win = false, game_over = false;
        saveQuestNumberWin = new List<int>();
        foreach (System.Xml.XmlNode node in doc.DocumentElement)
        {
            if (node.Name == "Quest")
            {
                int chisel = int.Parse(node.InnerText);
                saveQuestNumberWin.Add(chisel);
            }
            else if (win == false && node.Name == "WIN")
            {
                win = true;
            }
            else if (game_over == false && node.Name == "GAMEOVER")
            {
                game_over = true;
            }
        }
    }

    /// <summary>
    /// Возвращает количество побед пользователя
    /// </summary>
    public static int Win
    {
        get { return win; }
    }

    /// <summary>
    /// Возвращает количество поражений пользователя
    /// </summary>
    public static int Game_Over
    {
        get { return game_over; }
    }

    /// <summary>
    /// Проверяет наличие объект в коллекции пройденных уровней
    /// </summary>
    /// <param name="value">Значение номера уровня</param>
    /// <returns></returns>
    public static bool provObject(int value)
    {
        int len = saveQuestNumberWin.Count;
        for (int i = 0; i < len; i++)
            if (saveQuestNumberWin[i] == value)
                return true;
        return false;
    }

    /// <summary>
    /// Добавляет новую победу
    /// </summary>
    /// <param name="numberQuest">Номер пройденного уровня</param>
    public static void addWin(int numberQuest)
    {
        if (provObject(numberQuest) == false)
        {
            win++;
            saveQuestNumberWin.Add(numberQuest);
        }
    }

    /// <summary>
    /// Добавляет новое поражение
    /// </summary>
    public static void addGameOver()
    {
        game_over++;
    }

    /// <summary>
    /// Сохраняет настройки приложения
    /// </summary>
    public static void Save()
    {
/*        System.Threading.Thread thread = new System.Threading.Thread(() =>
        {*/
            Debug.Log("start Save");
            saveFunc();
            Debug.Log("end Save");
/*        });
        thread.Name = "Save settings application";
        thread.IsBackground = false;
        thread.Start(null);*/
    }

    private static void saveFunc()
    {
        Debug.Log("Save next while");
        /*while (saveNow)
        {
            Debug.Log("Save sleep");
            Debug.Log("Save up");
        }*/
        Debug.Log("Save end while " + saveNow.ToString());
        if (saveNow == false)
        {
            saveNow = true;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            string filepath = getFileSettingsPath();
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
            if (createFile(filepath, win, game_over))
            {
                int n = saveQuestNumberWin.Count;
                if (n > 0)
                {
                    try
                    {
                        doc.Load(fs);
                        for (int i = 0; i < n; i++)
                        {
                            var element = doc.CreateElement("Quest");
                            element.InnerText = saveQuestNumberWin[i].ToString();
                            doc.DocumentElement.AppendChild(element);
                        }
                        fs.Position = 0;
                        doc.Save(fs);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        saveNow = false;
                    }
                }
            }
            saveNow = false;
        }
    }
}
