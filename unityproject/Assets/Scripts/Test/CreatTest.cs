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
    private string[] subjects = { "ţ���˶�����" };


    // Use this for initialization
    [ContextMenu("��Ϊ����")]
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
             obj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);//�����õĻ������ܵ������ط�ȥ��
             obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//�����õĻ������
             Text btnText = obj.GetComponentInChildren<Text>();
             btnText.text = "<color=#FFFFFF>" + subjects[i] + "</color>";//�ı䰴ť�ļ���ɫ*/
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
