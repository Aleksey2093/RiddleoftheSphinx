using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

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
            /*var line = new object[]
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
            Debug.Log("doc create loaded");*/

            System.IO.Stream stream = new System.IO.MemoryStream();

            var mass_tmp = BitConverter.GetBytes(win);
            stream.Write(mass_tmp,0,mass_tmp.Length);

            mass_tmp = BitConverter.GetBytes(game_over);
            stream.Write(mass_tmp,0,mass_tmp.Length);

            /*doc.Save(stream);
            Debug.Log("save doc create to stream");*/
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
        if (stream.Position > 0)
            stream.Position = 0;
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
        while (restart_cout < 100)
        {
            try
            {
                System.IO.Stream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.Stream stream = getStreamReverse(fs);
                streamParse(stream);
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
        loadSetting = true;
    }

    /// <summary>
    /// Парсинг загружаемого файла настроек
    /// </summary>
    /// <param name="stream"></param>
    private static void streamParse(Stream stream)
    {
        var len = stream.Length;
        if (len >= 8)
        {
            byte[] tmp_array = new byte[4]; int i = 0;
            List<int> list = new List<int>();
            while (i < len)
            {
                stream.Read(tmp_array,0,4); i += 4;
                int val = BitConverter.ToInt32(tmp_array,0);
                list.Add(val);
            }
            int win = list[0], game_over = list[1];
            list.RemoveRange(0, 2);
            SettingsApplication.win = win;
            SettingsApplication.game_over = game_over;
            if (saveQuestNumberWin == null)
                saveQuestNumberWin = new List<int>();
            int len_list = list.Count, len_q = saveQuestNumberWin.Count;
            for (i=0;i<len_list;i++)
            {
                bool prov = true;
                for (int j=0;j<len_q;j++)
                    if (list[i] == saveQuestNumberWin[i])
                    {
                        prov = false;
                        break;
                    }
                if (prov)
                    saveQuestNumberWin.Add(list[i]);
            }
        }
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
            string filepath = getFileSettingsPath();
            if (createFile(filepath, win, game_over))
            {
                int n = saveQuestNumberWin.Count;
                if (n > 0)
                {
                    try
                    {
                        int win_copy = win;
                        int game_over_copy = game_over;
                        int[] array = saveQuestNumberWin.ToArray();
                        loadSettingFile();
                        win = win_copy;
                        game_over = game_over_copy;
                        int count_len = saveQuestNumberWin.Count;
                        for (int i = 0; i < n; i++)
                        {
                            bool prov = true;
                            for (int j = 0; j < count_len; j++)
                                if (saveQuestNumberWin[j] == array[i])
                                {
                                    prov = false;
                                    break;
                                }
                            if (prov)
                            {
                                saveQuestNumberWin.Add(array[i]);
                                count_len++;
                            }
                        }
                        System.IO.Stream stream = new System.IO.MemoryStream();
                        byte[] tmp_bytes = BitConverter.GetBytes(win);
                        stream.Write(tmp_bytes, 0, 4);
                        tmp_bytes = BitConverter.GetBytes(SettingsApplication.game_over);
                        stream.Write(tmp_bytes, 0, 4);
                        for (int i=0;i<count_len;i++)
                        {
                            tmp_bytes = BitConverter.GetBytes(saveQuestNumberWin[i]);
                            stream.Write(tmp_bytes, 0, 4);
                        }
                        stream = getStreamReverse(stream);
                        var fs = new System.IO.FileStream(filepath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
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
