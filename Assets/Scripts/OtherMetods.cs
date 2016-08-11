using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OtherMetods : MonoBehaviour {

    class OtherMethods
    {
        /// <summary>
        /// Возвращает ссылку на временную папку RiddleoftheSphinx
        /// </summary>
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

    static class SettingsApplication
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
                        string.Format("   <GAVEOVER>{0}</GAMEOVER>", game_over),
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
            string filepath = UnityEngine.Application.absoluteURL;
            int len = System.IO.Path.GetFileName(filepath).Length;
            filepath = filepath.Substring(0, filepath.Length - len);
            filepath = System.IO.Path.Combine(filepath, "xml_settings.saveSphinx");
            return filepath;
        }

        /// <summary>
        /// Загружает файл настроек с диска в память
        /// </summary>
        public static void loadSettingFile()
        {
            string filepath = getFileSettingsPath();
            var doc = new System.Xml.XmlDocument();
            try
            {
                doc.Load(filepath);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
                if (createFile(filepath))
                    doc.Load(filepath);
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
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                while (saveNow == false)
                {
                    System.Threading.Thread.Sleep(300);
                }
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
                                doc.Load(filepath);
                                for (int i = 0; i < n; i++)
                                {
                                    var element = doc.CreateElement("Quest");
                                    element.InnerText = saveQuestNumberWin[i].ToString();
                                    doc.DocumentElement.AppendChild(element);
                                }
                                doc.Save(filepath);
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
            });
            thread.Name = "Save settings application";
            thread.IsBackground = false;
            thread.Start();
        }
    }
}