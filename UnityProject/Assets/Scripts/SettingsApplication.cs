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
    /// <summary>
    /// Путь к файлу к настройками
    /// </summary>
    private static string absoluteUrlApplicationFileSetting;

    /// <summary>
    /// Настройки загружены
    /// </summary>
    private static bool loadSetting = false;

    /// <summary>
    /// Возвращает статус загрузки файла настроек
    /// </summary>
    /// <returns>true - настройки загружены, false - не загружеы или еще загружаются</returns>
    public static bool get_loadSetting()
    {
        return loadSetting;
    }

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
            Debug.Log(ex.Message);
        }
        try
        {
            var line = new object[]
                {
                        "<saveFileApplication>",
                        string.Format("   <WIN>{0}</WIN>", win),
                        string.Format("   <GAMEOVER>{0}</GAMEOVER>", game_over),
                        "</saveFileApplication>"
                };
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            string one_line = string.Format("{0}\n{1}\n{2}\n{3}", line);
            Debug.Log("one line create");
            doc.LoadXml(one_line);
            Debug.Log("doc create loaded");
            System.IO.Stream stream = new System.IO.MemoryStream();
            doc.Save(stream);
            Debug.Log("save doc create to stream");
            stream = getStreamReverse(stream);
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            FileStreamWriteFromOtherStreamData(fs, stream);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            stream.Close();
            fs.Close();
#endif
            stream.Dispose();
            fs.Dispose();
            Debug.Log("return true");
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Записывает поток байт из одного потока в другой
    /// </summary>
    /// <param name="result"></param>
    /// <param name="stream"></param>
    private static System.IO.FileStream FileStreamWriteFromOtherStreamData(System.IO.FileStream result, System.IO.Stream stream)
    {
        if (result.Position > 0)
            result.Position = 0;
        int byteint = stream.ReadByte();
        while (byteint != -1)
        {
            byte bit = (byte)byteint;
            result.WriteByte(bit);
            byteint = stream.ReadByte();
        }
        return result;
    }

    /// <summary>
    /// Разворачивает поток байт в потоке
    /// </summary>
    /// <param name="stream_original">поток</param>
    /// <returns></returns>
    private static System.IO.Stream getStreamReverse(System.IO.Stream stream_original)
    {
        List<byte> array_byte = new List<byte>();
        stream_original.Position = 0;
        int int_byte = stream_original.ReadByte();
        while (int_byte != -1)
        {
            byte bit = (byte)int_byte;
            array_byte.Add(bit);
            int_byte = stream_original.ReadByte();
        }
        array_byte.Reverse();
        return new System.IO.MemoryStream(array_byte.ToArray());
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
        loadSetting = false;
        string filepath = getFileSettingsPath();
        int restart_cout = 0;
        var doc = new System.Xml.XmlDocument();
        while (restart_cout < 100)
        {
            try
            {
                System.IO.Stream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.Stream stream = getStreamReverse(fs);
                doc.Load(stream);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                fs.Close();
                stream.Close();
#endif
                fs.Dispose();
                stream.Dispose();
                break;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                if (createFile(filepath) == false)
                    restart_cout++;
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
                win = true; SettingsApplication.win = int.Parse(node.InnerText);
            }
            else if (game_over == false && node.Name == "GAMEOVER")
            {
                game_over = true; SettingsApplication.game_over = int.Parse(node.InnerText);
            }
        }
        loadSetting = true;
    }

    /// <summary>
    /// Возвращает количество побед пользователя
    /// </summary>
    public static int Win()
    {
        return win;
    }

    /// <summary>
    /// Возвращает количество поражений пользователя
    /// </summary>
    public static int Game_Over()
    {
        return game_over;
    }

    /// <summary>
    /// Проверяет наличие объект в коллекции пройденных уровней
    /// </summary>
    /// <param name="value">Значение номера уровня</param>
    /// <returns></returns>
    public static bool provObject(int value)
    {
        bool restart = true;
        ret1:
        try
        {
            int len = saveQuestNumberWin.Count;
            for (int i = 0; i < len; i++)
                if (saveQuestNumberWin[i] == value)
                    return true;
        }
        catch(Exception ex)
        {
            if (saveQuestNumberWin == null || saveQuestNumberWin.Count == 0)
            {
                Debug.Log("Ошибка программы файл настроек не загружен" + ex.Message);
                loadSettingFile();
            }
            else
                Debug.Log("Не обработнная ошибка программы");
            if (restart)
            {
                restart = false;
                goto ret1;
            }
        }
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
        Debug.Log("start Save");
        saveFunc();
        Debug.Log("end Save");
    }

    private static void saveFunc()
    {
        Debug.Log("Save next while");
        while (saveNow)
        {
            Debug.Log("Save while");
        }
        Debug.Log("Save end while " + saveNow.ToString());
        if (saveNow == false)
        {
            saveNow = true;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            string filepath = getFileSettingsPath();
            if (createFile(filepath, win, game_over))
            {
                int n = saveQuestNumberWin.Count;
                if (n > 0)
                {
                    try
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                        System.IO.Stream stream = getStreamReverse(fs);
                        doc.Load(stream);
                        for (int i = 0; i < n; i++)
                        {
                            var element = doc.CreateElement("Quest");
                            element.InnerText = saveQuestNumberWin[i].ToString();
                            doc.DocumentElement.AppendChild(element);
                        }
                        stream = new System.IO.MemoryStream();
                        doc.Save(stream);
                        stream = getStreamReverse(stream);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                        fs.Close();
#endif                        
                        fs.Dispose();
                        fs = new System.IO.FileStream(filepath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                        FileStreamWriteFromOtherStreamData(fs, stream);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                        fs.Close();
                        stream.Close();
#endif
                        stream.Dispose();
                        fs.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }
            }
            saveNow = false;
        }
    }
}
