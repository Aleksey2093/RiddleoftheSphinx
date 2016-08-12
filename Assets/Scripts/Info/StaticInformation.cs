using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class StaticInformation : MonoBehaviour {

    /// <summary>
    /// Обозначает завершение загрузки файла с уровнями для игры
    /// </summary>
    public static bool downloaddonelevels { get; set; }

    /// <summary>
    /// Класс с уровнями игры
    /// </summary>
    public static class LevelXml
    {
        /// <summary>
        /// Номера уровней
        /// </summary>
        private static List<int> numbers = new List<int>();
        /// <summary>
        /// Вопросы
        /// </summary>
        private static List<string> questings = new List<string>();
        /// <summary>
        /// Ответы на вопросы
        /// </summary>
        private static List<string[]> answers = new List<string[]>();
        /// <summary>
        /// Правильный ответ на вопрос
        /// </summary>
        private static List<byte> true_answers = new List<byte>();

        /// <summary>
        /// Структа содержащая информацию о уровне
        /// </summary>
        public class LevelInformation
        {
            private int number;
            private string quest;
            private string[] answer;
            private string true_answer;

            /// <summary>
            /// Создает новый экземпляр сткрутры возвращаемой информации об уровне
            /// </summary>
            /// <param name="number">Номер уровня</param>
            /// <param name="quest">Вопрос</param>
            /// <param name="answer">Ответы</param>
            /// <param name="true_answer">Правильный ответ</param>
            public LevelInformation(int number, string quest, string[] answer, byte true_answer)
            {
                this.number = number;
                this.quest = quest;
                this.answer = answer;
                this.true_answer = answer[true_answer];
            }

            /// <summary>
            /// Возвращае номер уровня
            /// </summary>
            public int NumberLevel
            {
                get { return number; }
            }
            /// <summary>
            /// Возвращает вопрос уровня
            /// </summary>
            public string QuestLevel
            {
                get { return quest; }
            }
            /// <summary>
            /// Возвращает все ответы
            /// </summary>
            public string[] AllAnswerLevel
            {
                get { return answer; }
            }
            /// <summary>
            /// Возвращает правильный ответ
            /// </summary>
            public string True_answerLevel
            {
                get { return true_answer; }
            }
        }

        /// <summary>
        /// Возвращает следующий уровень для игры
        /// </summary>
        /// <param name="nowlvl">Номер текущего уровня</param>
        /// <returns>LevelInformation с информацией о новом уровне</returns>
        public static LevelInformation getNextLevel(int nowlvl)
        {
            Func<int, int, LevelInformation> getLevel = (start, end) =>
            {
                for (int i = start; i < end; i++)
                    if (SettingsApplication.provObject(numbers[i]) == false)
                    {
                        return new LevelInformation(numbers[i], questings[i], answers[i], true_answers[i]);
                    }
                return null;
            };
            bool restart_metod = true;
            ret1:
            int index = numbers.FindIndex(x => x == nowlvl), count = numbers.Count;
            LevelInformation level = getLevel(index + 1, count);
            if (level != null) return level;
            //если дошли сюда значит в следующих уровней нет и нужно вернуть один из уровней до текущего
            level = getLevel(0, index);
            //Хотя можем сделать ход конем и загрузить еще раз файл с сайта и так грузим
            if (restart_metod)
            {
                loadDataFromFileFromSite();
                restart_metod = false;
                goto ret1;
            }
            //Если дошли сюда значит уровней вообще не осталось и мы должны сообщить об этом пользователю возвращаем null, остальное делает вызывающий метод (он один на текущий момент)
            return null;
        }

        /// <summary>
        /// Указывает на то что файл в данный момент загружается
        /// </summary>
        private static bool loadDataFileNowFromSite_bool = false;

        /// <summary>
        /// Возвращает значение параметра указывающего на то выполняется ли в данный момент загрузка
        /// </summary>
        public static bool LoadDataFileNowFromSite_get
        {
            get { return loadDataFileNowFromSite_bool; }
        }

        /// <summary>
        /// Загружает данные из xml файла с сайта
        /// </summary>
        /// <returns></returns>
        public static bool loadDataFromFileFromSite()
        {
            while(loadDataFileNowFromSite_bool)
            {
                Debug.Log("waitdownload");
            }
            loadDataFileNowFromSite_bool = true;
            int restart = 0;
            ret1:
            try
            {
                string url = "http://2014.ucoz.org/file_c/game/unity/level.xml";
                WWW w = new WWW(url);
                while (w.isDone == false) { };
                MemoryStream ms = new MemoryStream(w.bytes);
                downloaddonelevels = false;
                xmlParse(ms);
                downloaddonelevels = true; loadDataFileNowFromSite_bool = false;
                return true;
            }
            catch (UnityException ex)
            {
                Debug.Log(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            if (restart < 5)
            {
                restart++;
                goto ret1;
            }
            downloaddonelevels = false; loadDataFileNowFromSite_bool = false;
            return false;
        }

        /// <summary>
        /// Метод парсит xml файл получаемый из памяти и заполняет коллекции класса информацией от туда
        /// </summary>
        /// <param name="ms"></param>
        private static void xmlParse(MemoryStream ms)
        {
            var doc = new System.Xml.XmlDocument();
            doc.Load(ms);
            int count = doc.DocumentElement.ChildNodes.Count;
            for (int i = 0; i < count; i++)
                try
                {
                    int number = int.Parse(doc.DocumentElement.ChildNodes[i].ChildNodes[0].InnerText);
                    /*Если уровень этот еще не был пройден мы его тоже загружаем в другой ситуации он нам нафиг не нужен*/
                    if (SettingsApplication.provObject(number) == false)
                    {
                        string quest = doc.DocumentElement.ChildNodes[i].ChildNodes[1].InnerText;
                        string[] answer = doc.DocumentElement.ChildNodes[i].ChildNodes[2].InnerText.Split(',');
                        string true_answer = doc.DocumentElement.ChildNodes[i].ChildNodes[3].InnerText.ToLower();
                        for (byte j = 0; j < 4; j++)
                            if (answer[j].ToLower() == true_answer)
                            {
                                addNewElement(number, quest, answer, j);
                                break;
                            }
                    }
                }
                catch
                {
                    Debug.Log(string.Format("Ошибка при считывании {0}", i));
                }
        }

        /// <summary>
        /// Добавляет новый элемент в коллекцию уровней
        /// </summary>
        /// <param name="number">Номер уровня</param>
        /// <param name="quest">Задание</param>
        /// <param name="answer">Ответы</param>
        /// <param name="j">управильный ответ</param>
        private static void addNewElement(int number, string quest, string[] answer, byte j)
        {
            bool restart = true;
            restart_label:
            int[] len = new int[] { numbers.Count, questings.Count, answers.Count, true_answers.Count };
            try
            {
                numbers.Add(number);
                questings.Add(quest);
                answers.Add(answer);
                true_answers.Add(j);
            }
            catch /*В случае возникновения ошибки при добавлении удаляем информацию которая была добавлена из коллекции
            до того момента пока длинна всех уровней не станет одинаковой*/
            {
                int min = len[0];
                for (int i = 1; i < 4; i++)
                    if (min > len[i])
                        min = len[i];
                while (numbers.Count > min)
                {
                    numbers.RemoveAt(numbers.Count - 1);
                }
                while (questings.Count > min)
                {
                    questings.RemoveAt(questings.Count - 1);
                }
                while (answers.Count > min)
                {
                    answers.RemoveAt(answers.Count - 1);
                }
                while (true_answers.Count > min)
                {
                    true_answers.RemoveAt(true_answers.Count - 1);
                }
                if (restart)
                {
                    restart = false;
                    goto restart_label;
                }
            }
        }
    }
}
