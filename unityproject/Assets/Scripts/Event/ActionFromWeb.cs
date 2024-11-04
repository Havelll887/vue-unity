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


    // 接收从web发来的消息
    public void UnityItemClick(string msg)
    {
        // 跳转到首页 直接初始化场景
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

            // 点击产业链环节 跳转到gis场景层
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
        // 获取主函数
        InitMainChain = GameObject.Find("MainScript").GetComponent<InitMainChain>();
        // 获取公共类
        chainItemsList = InitMainChain.chainItemDictsList;
    }

    void Update()
    {

    }
}
