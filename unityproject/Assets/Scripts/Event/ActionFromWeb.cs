using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ActionFromWeb : MonoBehaviour
{
    List<ChainItemDict> chainItemsList;
    InitMainChain InitMainChain;
    public string tagString = "home page";


    // ���մ�web��������Ϣ
    public void UnityItemClick(string msg)
    {
        // ��ת����ҳ ֱ�ӳ�ʼ������
        if (msg.Length > 0 && msg.ToLower() == "indexhomepage") {
            SceneManager.LoadScene("IndustrialIndex");
        }
        else
        {
            List<ChainItemDict> receivedList = new List<ChainItemDict>();
            receivedList = JsonConvert.DeserializeObject<List<ChainItemDict>>(msg);
            Debug.Log(receivedList.Count);

            Debug.Log("current Type    " + tagString);
            for (int i = 0; i < receivedList.Count; i++) 
            {
                Debug.Log("receive   "  + receivedList[i]);
                Debug.Log("receive   "  + receivedList[i].Id + receivedList[i].Name);
            }

            // �����ҵ������ ��ת��gis������
            if (receivedList[receivedList.Count-1].Level == "3")
            {
                GameObject.Find("MainScript").GetComponent<InitMainChain>().transmitList = receivedList;

                SceneManager.LoadScene("GisScene");

            }else if (receivedList[receivedList.Count - 1].Level == "2" || receivedList[receivedList.Count - 1].Level == "1")
            {
                GameObject.Find("MainScript").GetComponent<InitMainChain>().transmitList = receivedList;

                SceneManager.LoadScene("IndustrialIndex");
                
            }

        }

    }



    void Start()
    {
        // ��ȡ������
        InitMainChain = GameObject.Find("MainScript").GetComponent<InitMainChain>();
        // ��ȡ������
        chainItemsList = InitMainChain.chainItemDictsList;
    }

    void Update()
    {

    }
}
