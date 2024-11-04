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
        print("�����===OnPointerClick(" + this.name);

        GetObjectParent();

    }

    void GetObjectParent()
    {
        // ��ȡ��������
        Transform parentTransform = transform.parent;

        // ��ȡ��ǰid����Ϣ
        ChainItemDict chainItem = InitMainChain.GetMsgByid(parentTransform.gameObject.name, chainItemsList);

        if (chainItem !=null)
        {

            // ������ǵ�һ��������һ��
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
                // ��ȡ���ӵĸ���
                Transform ppTransform = parentTransform.parent;

                // ��ȡ�ֵܽڵ�
                int childCount = ppTransform.childCount;

                // �Լ��ŵڼ�
                int selfIndex = parentTransform.GetSiblingIndex();

                for (int i = 0; i < childCount; i++)
                {
                    // ���Լ�����ֵܽڵ�����
                    if (selfIndex != i)
                    {
                       ppTransform.GetChild(i).gameObject.SetActive(false);
                    }
                }

                // ���ؽڵ�
                InitMainChain.GetLevel2ModelsById(chainItem.Id);

            }

            // ������Ƕ���
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
                    // ���Լ�����ֵܽڵ�����
                    if (selfIndex != i)
                    {
                        ppTransform.GetChild(i).gameObject.SetActive(false);
                    }
                }

                // ���ؽڵ�
                InitMainChain.GetLevel2ModelsById(chainItem.Pid);

            }
            // �����������
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


                // 1. ����Я������
                GameObject.Find("MainScript").GetComponent<InitMainChain>().transmitList = List2Web;

                // 2. ������ת
                SceneManager.LoadScene("GisScene");

            }

        }
        else
        {
            Debug.Log("δ������������Ϣ");
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
