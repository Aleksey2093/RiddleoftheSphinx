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

    void Awake()
    {
        nowlvl = 0;
        answer_true = null;
    }

    // Use this for initialization
    void Start()
    {
        nextLevel();
    }

    public void provAnswerClickButton(string answer)
    {
        if (answer_true == answer)
        {
            SettingsApplication.addWin(nowlvl);
            nextLevel();
        }
        else
        {
            SettingsApplication.addGameOver();
        }
    }

    void nextLevel()
    {
        double i = 0;
        while(StaticInformation.downloaddonelevels == false)
        {
            double res;
            res = i % 100;
            if (res == 0)
            {
                if (StaticInformation.LevelXml.LoadDataFileNowFromSite_get() == false)
                {
                    if (StaticInformation.LevelXml.loadDataFromFileFromSite(null))
                    {
                        break;
                    }
                }
            }
            else if (i > 1000 && StaticInformation.LevelXml.LoadDataFileNowFromSite_get() == false)
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
            }
            i++;
        }
        var level_info = StaticInformation.LevelXml.getNextLevel(nowlvl);
        switch (level_info.get_Message())
        {
            case StaticInformation.LevelXml.Reslvl.Ok:
                {
                    setValueOnTextAndButtons(level_info.get_Info());
                    try
                    {
                        Animator animator = gameObject.GetComponent<Animator>();
                        animator.SetBool("Povorot", true);
                    }
                    catch(UnityEngine.UnityException ex)
                    {
                        Debug.Log(ex.Message);
                    }
                    catch(Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }
                break;
            case StaticInformation.LevelXml.Reslvl.No_Lvl:
                {
                    var a1 = GameObject.Find("Scripts").GetComponent<InformationScreen>();
                    if (a1 != null)
                        a1.SetValue_ResLvl(StaticInformation.LevelXml.Reslvl.No_Lvl);
                }
                break;
            case StaticInformation.LevelXml.Reslvl.End_lvl:
                {
                    var a1 = GameObject.Find("Scripts").GetComponent<InformationScreen>();
                    if (a1 != null)
                        a1.SetValue_ResLvl(StaticInformation.LevelXml.Reslvl.End_lvl);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Возвращает корректность получения объектов со сцены. Try - все получили - False нифига не получили
    /// </summary>
    /// <param name="textComponent">Текстовый объекток содержащий текст вопроса</param>
    /// <param name="buttons">Кпноки</param>
    /// <returns></returns>
    private bool getObjectScene(out Text textComponent, out Button[] buttons)
    {
        textComponent = GameObject.Find("TextQuest").GetComponent<Text>();
        buttons = FindObjectsOfType<Button>();
        if (textComponent != null)
        {
            if (buttons != null)
            {
                buttons = getSortButton(buttons);
                if (buttons.Length == 4)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Устанавливает значение объектов сцены
    /// </summary>
    /// <param name="level_info">Информация об уровне</param>
    private void setValueOnTextAndButtons(StaticInformation.LevelXml.LevelInformation level_info)
    {
        Text text_quest;
        Button[] buttons;
        if (getObjectScene(out text_quest, out buttons))
        {
            text_quest.text = level_info.QuestLevel;
            for (int i = 0; i < 4; i++)
                buttons[i].transform.FindChild("Text").GetComponent<Text>().text = level_info.AllAnswerLevel[i];
            answer_true = level_info.True_answerLevel;
            nowlvl = level_info.NumberLevel;
        }
        else
            Debug.Log("Error in setValue.... Text1Script");
    }

    /// <summary>
    /// Сортирует кнопки в массиве по цифре в конце названия кнопки
    /// </summary>
    /// <param name="buttons">Массив кнопок</param>
    /// <returns></returns>
    private Button[] getSortButton(Button[] buttons)
    {
        System.Collections.Generic.List<Button> list = new System.Collections.Generic.List<Button>(buttons);
        int len = list.Count, lenminus = len - 1;
        for (int i=0;i<lenminus;i++)
        {
            ret1:
            for (int j=i+1;j<len;j++)
            {
                ret2:
                string name1 = list[i].name[list[i].name.Length - 1].ToString(),
                    name2 = list[j].name[list[j].name.Length - 1].ToString();
                try
                {
                    int c1, c2;
                    if (int.TryParse(name1, out c1) == false)
                    {
                        list.RemoveAt(i); goto ret1;
                    }
                    else if (int.TryParse(name2, out c2) == false)
                    {
                        list.RemoveAt(j); goto ret2;
                    }
                    else if (c2 < c1)
                    {
                        var tmp = list[i]; list[i] = list[j]; list[j] = tmp;
                    }
                }
                catch {
                    Debug.Log("Error buttons answer sort");
                }
            }
        }
        return list.ToArray();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
        }
	}

    void OnApplicationQuit ()
    {
        SettingsApplication.Save();
    }
}
