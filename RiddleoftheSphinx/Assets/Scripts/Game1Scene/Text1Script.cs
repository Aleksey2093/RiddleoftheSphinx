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

    private Text textQuest;
    private Text[] buttonsAnswers = new Text[4];
    private Animator animator_textQuest;

    // Use this for initialization
    void Start()
    {
        if (SettingsApplication.get_loadSetting() == false)
        {
            SettingsApplication.loadSettingFile();
        }
        try
        {
            textQuest = GameObject.Find("TextQuest").GetComponent<Text>();
            animator_textQuest = textQuest.GetComponent<Animator>();
            for (int i = 0; i < 4; i++)
                buttonsAnswers[i] = GameObject.Find(string.Format("Button{0}", (i + 1))).transform.GetComponentInChildren<Text>();
            nextLevel();
            Debug.Log("Start text1script end");
        }
        catch(Exception ex)
        {
            Debug.Log("Start text1script error: " + ex.Message);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
        }
    }

    public void provAnswerClickButton(string answer)
    {
        if (answer_true == answer)
                SettingsApplication.addWin(nowlvl);
            else
                SettingsApplication.addGameOver();
        nextLevel();
    }

    void nextLevel()
    {
        float time1 = UnityEngine.Time.time;
        while (StaticInformation.downloaddonelevels == false)
        {
            float time2 = UnityEngine.Time.time;
            if (StaticInformation.LevelXml.LoadDataFileNowFromSite_get() == false
                && StaticInformation.LevelXml.loadDataFromFileFromSite())
                break;
            else if (time2 > time1 && StaticInformation.LevelXml.LoadDataFileNowFromSite_get() == false)
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
        }
        var level_info = StaticInformation.LevelXml.getNextLevel(nowlvl);
        switch (level_info.get_Message())
        {
            case StaticInformation.LevelXml.Reslvl.Ok:
                {
                    try
                    {
                        animator_textQuest.SetBool("Povorot", true);
                    }
                    catch (UnityEngine.UnityException ex)
                    {
                        Debug.Log(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                    setValueOnTextAndButtons(level_info.get_Info());
                    //StartCoroutine("setValueOnTextAndButtons", level_info.get_Info());
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
    /// Устанавливает значение объектов сцены
    /// </summary>
    /// <param name="level_info">Информация об уровне</param>
    void setValueOnTextAndButtons(StaticInformation.LevelXml.LevelInformation level_info)
    {
        if (textQuest != null && Array.TrueForAll(buttonsAnswers, x=> x != null))
        {
            textQuest.text = level_info.QuestLevel;
            for (int i = 0; i < 4; i++)
                buttonsAnswers[i].text = level_info.AllAnswerLevel[i];
            answer_true = level_info.True_answerLevel;
            nowlvl = level_info.NumberLevel;
        }
        else
            Debug.Log("Error in setValue.... Text1Script");
        //yield return null;
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            SettingsApplication.Save();
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
        }
	}
}
