using UnityEngine;
using System.Collections;

public class ButtonStart : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(buttonClick);
    }

    private void buttonClick()
    {
        if (StaticInformation.downloaddonelevels == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game1");
        }
    }
}
