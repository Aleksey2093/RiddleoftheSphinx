using UnityEngine;
using System.Collections;

public class ButtonSettings : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        animator_start = GameObject.Find("ButtonStart").GetComponent<Animator>();
        animator_settings = gameObject.GetComponent<Animator>();
        animator_exit = GameObject.Find("ButtonExit").GetComponent<Animator>();
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(buttonClick);
    }

    Animator animator_start;
    Animator animator_settings;
    Animator animator_exit;

    void Update()
    {
        if (click)
        {
            float time2 = UnityEngine.Time.time;
            if (time2 - time1 > 5 && nextScene == false)
            {
                animator_settings.SetBool("isClose", false);
                StartCoroutine("setOtherButtonAnimation", false);
                click = false;
            }
            else if (time2 - time1 > 1 && nextScene == true)
            {
                Debug.Log("Попытка открыть окно настроек");
                nextScene = false; //click = false;
            }
        }
    }

    private bool click = false;
    float time1;
    private bool nextScene = false;

    private void buttonClick()
    {
        nextScene = (SettingsApplication.get_loadSetting() == true);
        animator_settings.SetBool("isClose", true);
        time1 = UnityEngine.Time.time;
        click = true;
        StartCoroutine("setOtherButtonAnimation", true);
    }

    IEnumerator setOtherButtonAnimation(bool what_state)
    {
        yield return new WaitForSeconds(0.3f);
        animator_start.SetBool("isClose", what_state);
        animator_exit.SetBool("isClose", what_state);
        yield return null;
    }
}
