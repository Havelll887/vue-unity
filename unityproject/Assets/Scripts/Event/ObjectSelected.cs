using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ObjectSelected : MonoBehaviour , IPointerClickHandler
{
    List<ChainItemDict> chainItemsList;
    InitMainChain InitMainChain;


    [DllImport("__Internal")]
    private static extern void SelectedTarget(string targetName);


    public void OnPointerClick(PointerEventData eventData)
    {
        print("点击了===OnPointerClick(" + this.name);

        GetObjectParent();

    }

    void GetObjectParent()
    {
        // 获取盒子名称
        Transform parentTransform = transform.parent;

        // 获取当前id的信息
        ChainItemDict chainItem = InitMainChain.GetMsgByid(parentTransform.gameObject.name, chainItemsList);

        if (chainItem !=null)
        {

            // 点击的是第一级，其他一级
            if (chainItem.Level == "1")
            {
#if !UNITY_EDITOR && UNITY_WEBGL
            List<ChainItemDict> List2Web = new List<ChainItemDict>();
            List2Web.Add(chainItem);
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(List2Web, settings);
            SelectedTarget(json);
#endif
                // 获取盒子的父级
                Transform ppTransform = parentTransform.parent;

                // 获取兄弟节点
                int childCount = ppTransform.childCount;

                // 自己排第几
                int selfIndex = parentTransform.GetSiblingIndex();

                for (int i = 0; i < childCount; i++)
                {
                    // 除自己外的兄弟节点隐藏
                    if (selfIndex != i)
                    {
                       ppTransform.GetChild(i).gameObject.SetActive(false);
                    }
                }

                // 加载节点
                InitMainChain.GetLevel2ModelsById(chainItem.Id);

            }

            // 点击的是二级
            if (chainItem.Level == "2")
            {
#if !UNITY_EDITOR && UNITY_WEBGL
            List<ChainItemDict> List2Web = new List<ChainItemDict>();
            ChainItemDict level2 = InitMainChain.GetMsgByid(parentTransform.gameObject.name, chainItemsList);
            ChainItemDict level1 = InitMainChain.GetMsgByid(level2.Pid, chainItemsList);
            List2Web.Add(level1);
            List2Web.Add(level2);
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(List2Web, settings);
            SelectedTarget(json);
#endif

                Transform ppTransform = parentTransform.parent.parent.parent;

                int childCount = ppTransform.childCount;

                int selfIndex = parentTransform.parent.parent.GetSiblingIndex();

                for (int i = 0; i < childCount; i++)
                {
                    // 除自己外的兄弟节点隐藏
                    if (selfIndex != i)
                    {
                        ppTransform.GetChild(i).gameObject.SetActive(false);
                    }
                }

                // 加载节点
                InitMainChain.GetLevel2ModelsById(chainItem.Pid);

            }
            // 点击的是三级
            if (chainItem.Level == "3")
            {
                List<ChainItemDict> List2Web = new List<ChainItemDict>();
                ChainItemDict level3 = InitMainChain.GetMsgByid(parentTransform.gameObject.name, chainItemsList);
                ChainItemDict level2 = InitMainChain.GetMsgByid(level3.Pid, chainItemsList);
                ChainItemDict level1 = InitMainChain.GetMsgByid(level2.Pid, chainItemsList);
                List2Web.Add(level1);
                List2Web.Add(level2);
                List2Web.Add(level3);
#if !UNITY_EDITOR && UNITY_WEBGL
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(List2Web, settings);
            SelectedTarget(json);
#endif


                // 1. 配置携带参数
                GameObject.Find("MainScript").GetComponent<InitMainChain>().transmitList = List2Web;

                // 2. 场景跳转
                SceneManager.LoadScene("GisScene");

            }

        }
        else
        {
            Debug.Log("未检索到对象信息");
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
