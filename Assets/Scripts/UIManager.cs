using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject comingSoon_Start_Screen , comingSoon_End_Screen;


    public static UIManager instance;
    void Awake()
    {
        instance = this; 
    }

    // Use this for initialization
    void Start()
    {
        comingSoon_Start_Screen.SetActive(false);
        comingSoon_End_Screen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void On_Start_Coming_Soon(string b_Name)
    {
        if(b_Name == "Bot")
        {
            comingSoon_Start_Screen.GetComponent<RectTransform>().transform.localPosition = new Vector3(1.5f, -240.0f, 0);
        }
        else if (b_Name == "Online")
        {
            comingSoon_Start_Screen.GetComponent<RectTransform>().transform.localPosition = new Vector3(1.5f, -370.0f, 0);
        }

        comingSoon_Start_Screen.SetActive(true);
        StartCoroutine(wait_Start_Coming_Soon());
    }

    IEnumerator wait_Start_Coming_Soon()
    {
        yield return new WaitForSeconds(1.0f);
        comingSoon_Start_Screen.SetActive(false);
    }

    public void On_End_Coming_Soon()
    {
        comingSoon_End_Screen.SetActive(true);
        StartCoroutine(wait_End_Coming_Soon());
    }

    IEnumerator wait_End_Coming_Soon()
    {
        yield return new WaitForSeconds(1.0f);
        comingSoon_End_Screen.SetActive(false);
    }
}
