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
    // icon模型自己的名称
    public string ModelName { set; get; }

    // icon模型合并创建的对象名称，即盒子（icon + 底座）
    public string BoxName { set; get; }

    // label对象的名称内容
    public string LabelName { set; get; }

    // 盒子（icon + 底座）的场景位置信息 世界坐标
    public float[] ParentPosition { set; get; }

    // 盒子（icon + 底座）的场景位置信息相对盒子父节点 相对坐标 
    public float[] ParentLocalPosition { set; get; }

    // icon模型相对父节点的位置
    public float[] IconLocalPosition { set; get; }

    // icon模型相对box 的旋转角度
    public float[] IconLocalEulerAngles { set; get; }

    // icon模型的缩放大小
    public float[] IconLocalScale { set; get; }

}



// 该类用来创建模型名称与创建的空对象映射存储文件
public class CombineModels : EditorWindow
{
    // 图标的字典信息
    public static Dictionary<string, IconModelGroup> IconDicMsg = new Dictionary<string, IconModelGroup>();


    [MenuItem("脚本执行/批量组合底座模型", false, 1000)]
    static void Apply()
    {
        Rect wr = new Rect(0, 0, 100, 100);
        CombineModels window = (CombineModels)EditorWindow.GetWindowWithRect(typeof(CombineModels), wr, true, "CreatCtns");
    }


    private void OnGUI()
    {
        this.Repaint();//实时刷新
        if (GUILayout.Button("生成"))
        {
            // 获取字典脚本
            GetConfigMapData();

            // 创建组合体模型
            CreateIconBaseCombine();
        }
        if (GUILayout.Button("close"))
        {
            this.Close();
        }
    }


    private void CreateIconBaseCombine()
    {
        // 1. 获取对应文件夹下的模型列表
        string typeName = "Furniture";
        FileInfo[] fileInfosList = GetFilesInFolder(typeName);
        GameObject parentCon = GameObject.Find("CenterMainModel/" + typeName);

        // 模块的根节点
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

            // 获取该模型的配置信息
            IconModelGroup iconModelGroup = IconDicMsg[itemName[0]];

            Debug.Log("name123  " + iconModelGroup.LabelName);

            // continue;

            // 2. 判断名称 2J 3J sy zy xy
            // 二级
            if (itemName[0].IndexOf("2J") > -1)
            {
                GenerateCombinesModels(parentCon, iconModelGroup, "2J_dizuo Variant");
            }
            // 三级
            else if (itemName[0].IndexOf("3J") > -1)
            {
                // 模型自己的对象
                GameObject Object3J = GenerateCombinesModels(parentCon, iconModelGroup, "3J_dizuo Variant");

                // 三级下 每一级都要创建上下游链
                // 1. 获取文件夹下的模型列表
                FileInfo[] subFilesList = GetFilesInFolder(typeName + "/SubStream");
                if (subFilesList.Length > 0)
                {
                    // 创建一个空对象放置链
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


                        // 上游
                        if (subItemName[0].IndexOf("sy") > -1)
                        {
                            GenerateCombinesModels(subStreamObj, subIconModelGroup, "4J_dizuo_blue Variant", itemName[0] + "/");
                        }
                        // 中游
                        else if (subItemName[0].IndexOf("zy") > -1)
                        {
                            GenerateCombinesModels(subStreamObj, subIconModelGroup, "4J_dizuo_green Variant", itemName[0] + "/");
                        }
                        // 下游
                        else if (subItemName[0].IndexOf("xy") > -1)
                        {
                            GenerateCombinesModels(subStreamObj, subIconModelGroup, "4J_dizuo_yellow Variant", itemName[0] + "/");
                        }

                    }
                }
            }
        }
    }

    // 列表文件获取方法
    FileInfo[] GetFilesInFolder(string ModelFloadName)
    {
        string orginPath = Application.dataPath + "/Models/" + ModelFloadName;
        DirectoryInfo direction = new DirectoryInfo(orginPath);
        FileInfo[] files = direction.GetFiles("*.FBX");
        return files;
    }


    // 模型字典生成脚本
    public static void GetConfigMapData()
    {
        // 1. JSON数据获取
        string fileUrl = Application.dataPath + "/StaticData/iconData/Dictionary.json";
        string JSONData = File.OpenText(fileUrl).ReadToEnd();

        // 2. JSON数据解析
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
        // 查找当前目标是否已存在
        GameObject IconParent = GameObject.Find(basePath + iconModelGroup.BoxName);

        // 不存在创建
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

        // 底座
        GameObject go = PrefabUtility.InstantiatePrefab(Resources.Load(baseName)) as GameObject;
        go.transform.parent = IconParent.transform;
        go.transform.localPosition = new Vector3(0, 0, 0);
        go.transform.localEulerAngles = Vector3.zero;

        // 模型
        GameObject iconModel = PrefabUtility.InstantiatePrefab(Resources.Load(iconModelGroup.BoxName + " Variant")) as GameObject;
        iconModel.transform.parent = IconParent.transform;
        iconModel.transform.localPosition = new Vector3(iconModelGroup.IconLocalPosition[0], iconModelGroup.IconLocalPosition[1], iconModelGroup.IconLocalPosition[2]);
        iconModel.transform.localEulerAngles = new Vector3(iconModelGroup.IconLocalEulerAngles[0], iconModelGroup.IconLocalEulerAngles[1], iconModelGroup.IconLocalEulerAngles[2]);
        iconModel.transform.localScale = new Vector3(iconModelGroup.IconLocalScale[0], iconModelGroup.IconLocalScale[1], iconModelGroup.IconLocalScale[2]);


        return IconParent;
    }
}