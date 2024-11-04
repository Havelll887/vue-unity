using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/*
public class CreatTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
public class CreatTest : MonoBehaviour
{
    private string[] subjects = { "牛顿运动定律" };


    // Use this for initialization
    [ContextMenu("行为测试")]
    void Testing()
    {
        for (int i = 0; i < subjects.Length; i++)
        {
            Debug.Log(subjects.Length);
            GameObject obj =   (GameObject)Instantiate(Resources.Load("TextPre"));
            Transform[] transforms = obj.GetComponentsInChildren<Transform>();
            Debug.Log("Rgaeg  -- " + transforms.Length);
            for (int j = 1; j < transforms.Length; j++) {
                transforms[j].gameObject.GetComponent<TextMeshProUGUI>().text = "25898";
                // Debug.Log(transforms[j].name);
                // Debug.Log(transforms[j].name + "  --!!!!  " + transforms[j].gameObject.GetComponent<TextMeshProUGUI>().text);
                // transforms[j].gameObject.GetComponent<TextMeshPro>().text = "5889" + i;
            }
            /* obj.transform.SetParent(transform);
             obj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);//不设置的话可能跑到其他地方去了
             obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//不设置的话变大了
             Text btnText = obj.GetComponentInChildren<Text>();
             btnText.text = "<color=#FFFFFF>" + subjects[i] + "</color>";//改变按钮文件颜色*/
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
