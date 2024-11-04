using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Security.AccessControl;
using log4net.Util;

[Serializable]
public class IconModelGroup
{
    // iconģ���Լ�������
    public string ModelName { set; get; }

    // iconģ�ͺϲ������Ķ������ƣ������ӣ�icon + ������
    public string BoxName { set; get; }

    // label�������������
    public string LabelName { set; get; }

    // ���ӣ�icon + �������ĳ���λ����Ϣ ��������
    public float[] ParentPosition { set; get; }

    // ���ӣ�icon + �������ĳ���λ����Ϣ��Ժ��Ӹ��ڵ� ������� 
    public float[] ParentLocalPosition { set; get; }

    // iconģ����Ը��ڵ��λ��
    public float[] IconLocalPosition { set; get; }

    // iconģ�����box ����ת�Ƕ�
    public float[] IconLocalEulerAngles { set; get; }

    // iconģ�͵����Ŵ�С
    public float[] IconLocalScale { set; get; }

}



// ������������ģ�������봴���Ŀն���ӳ��洢�ļ�
public class CombineModels : EditorWindow
{
    // ͼ����ֵ���Ϣ
    public static Dictionary<string, IconModelGroup> IconDicMsg = new Dictionary<string, IconModelGroup>();


    [MenuItem("�ű�ִ��/������ϵ���ģ��", false, 1000)]
    static void Apply()
    {
        Rect wr = new Rect(0, 0, 100, 100);
        CombineModels window = (CombineModels)EditorWindow.GetWindowWithRect(typeof(CombineModels), wr, true, "CreatCtns");
    }


    private void OnGUI()
    {
        this.Repaint();//ʵʱˢ��
        if (GUILayout.Button("����"))
        {
            // ��ȡ�ֵ�ű�
            GetConfigMapData();

            // ���������ģ��
            CreateIconBaseCombine();
        }
        if (GUILayout.Button("close"))
        {
            this.Close();
        }
    }


    private void CreateIconBaseCombine()
    {
        // 1. ��ȡ��Ӧ�ļ����µ�ģ���б�
        string typeName = "Furniture";
        FileInfo[] fileInfosList = GetFilesInFolder(typeName);
        GameObject parentCon = GameObject.Find("CenterMainModel/" + typeName);

        // ģ��ĸ��ڵ�
        if (fileInfosList.Length > 0)
        {
            if (!parentCon)
            {
                GameObject parent = GameObject.Find("CenterMainModel");
                parentCon = new GameObject(typeName);
                parentCon.transform.parent = parent.transform;
                parentCon.transform.localPosition = Vector3.zero;
                parentCon.transform.localEulerAngles = Vector3.zero;
            }

        }
        else
        {
            return;
        }


        for (int i = 0; i < fileInfosList.Length; i++)
        {

            string[] itemName = (fileInfosList[i].Name).Split(new char[] { '.' });

            // ��ȡ��ģ�͵�������Ϣ
            IconModelGroup iconModelGroup = IconDicMsg[itemName[0]];

            Debug.Log("name123  " + iconModelGroup.LabelName);

            // continue;

            // 2. �ж����� 2J 3J sy zy xy
            // ����
            if (itemName[0].IndexOf("2J") > -1)
            {
                GenerateCombinesModels(parentCon, iconModelGroup, "2J_dizuo Variant");
            }
            // ����
            else if (itemName[0].IndexOf("3J") > -1)
            {
                // ģ���Լ��Ķ���
                GameObject Object3J = GenerateCombinesModels(parentCon, iconModelGroup, "3J_dizuo Variant");

                // ������ ÿһ����Ҫ������������
                // 1. ��ȡ�ļ����µ�ģ���б�
                FileInfo[] subFilesList = GetFilesInFolder(typeName + "/SubStream");
                if (subFilesList.Length > 0)
                {
                    // ����һ���ն��������
                    GameObject subStreamObj = GameObject.Find(itemName[0] + "/SubStreamBox");
                    if (!subStreamObj)
                    {
                        subStreamObj = new GameObject("SubStreamBox");
                        subStreamObj.transform.parent = Object3J.transform;
                        subStreamObj.transform.localPosition = Vector3.zero;
                        subStreamObj.transform.localEulerAngles = Vector3.zero;
                    }

                    for (int j = 0; j < subFilesList.Length; j++)
                    {
                        string[] subItemName = (subFilesList[j].Name).Split(new char[] { '.' });
                        IconModelGroup subIconModelGroup = IconDicMsg[subItemName[0]];

                        Debug.Log("Object3JObject3J==  " + Object3J.name);


                        // ����
                        if (subItemName[0].IndexOf("sy") > -1)
                        {
                            GenerateCombinesModels(subStreamObj, subIconModelGroup, "4J_dizuo_blue Variant", itemName[0] + "/");
                        }
                        // ����
                        else if (subItemName[0].IndexOf("zy") > -1)
                        {
                            GenerateCombinesModels(subStreamObj, subIconModelGroup, "4J_dizuo_green Variant", itemName[0] + "/");
                        }
                        // ����
                        else if (subItemName[0].IndexOf("xy") > -1)
                        {
                            GenerateCombinesModels(subStreamObj, subIconModelGroup, "4J_dizuo_yellow Variant", itemName[0] + "/");
                        }

                    }
                }
            }
        }
    }

    // �б��ļ���ȡ����
    FileInfo[] GetFilesInFolder(string ModelFloadName)
    {
        string orginPath = Application.dataPath + "/Models/" + ModelFloadName;
        DirectoryInfo direction = new DirectoryInfo(orginPath);
        FileInfo[] files = direction.GetFiles("*.FBX");
        return files;
    }


    // ģ���ֵ����ɽű�
    public static void GetConfigMapData()
    {
        // 1. JSON���ݻ�ȡ
        string fileUrl = Application.dataPath + "/StaticData/iconData/Dictionary.json";
        string JSONData = File.OpenText(fileUrl).ReadToEnd();

        // 2. JSON���ݽ���
        // JObject JSONClass = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(JSONData);
        JObject JSONClass = JObject.Parse(JSONData);

        foreach (JProperty item in JSONClass.Properties())
        {
            IconModelGroup obj = JsonConvert.DeserializeObject<IconModelGroup>(item.Value.ToString());
            if (IconDicMsg.ContainsKey(item.Name.ToString()))
            {
                IconDicMsg[item.Name] = obj;
            }
            else
            {
                IconDicMsg.Add(item.Name.ToString(), obj);
            }
        }
    }


    private GameObject GenerateCombinesModels(GameObject parentCon, IconModelGroup iconModelGroup, string baseName, string basePath = "")
    {
        // ���ҵ�ǰĿ���Ƿ��Ѵ���
        GameObject IconParent = GameObject.Find(basePath + iconModelGroup.BoxName);

        // �����ڴ���
        if (!IconParent)
        {
            IconParent = new GameObject(iconModelGroup.BoxName);
            IconParent.transform.parent = parentCon.transform;
            IconParent.transform.localPosition = new Vector3(
                iconModelGroup.ParentLocalPosition[0], iconModelGroup.ParentLocalPosition[1], iconModelGroup.ParentLocalPosition[2]
                );
            IconParent.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            IconParent.transform.localPosition = new Vector3(10, 0, 10);
        }

        // ����
        GameObject go = PrefabUtility.InstantiatePrefab(Resources.Load(baseName)) as GameObject;
        go.transform.parent = IconParent.transform;
        go.transform.localPosition = new Vector3(0, 0, 0);
        go.transform.localEulerAngles = Vector3.zero;

        // ģ��
        GameObject iconModel = PrefabUtility.InstantiatePrefab(Resources.Load(iconModelGroup.BoxName + " Variant")) as GameObject;
        iconModel.transform.parent = IconParent.transform;
        iconModel.transform.localPosition = new Vector3(iconModelGroup.IconLocalPosition[0], iconModelGroup.IconLocalPosition[1], iconModelGroup.IconLocalPosition[2]);
        iconModel.transform.localEulerAngles = new Vector3(iconModelGroup.IconLocalEulerAngles[0], iconModelGroup.IconLocalEulerAngles[1], iconModelGroup.IconLocalEulerAngles[2]);
        iconModel.transform.localScale = new Vector3(iconModelGroup.IconLocalScale[0], iconModelGroup.IconLocalScale[1], iconModelGroup.IconLocalScale[2]);


        return IconParent;
    }
}