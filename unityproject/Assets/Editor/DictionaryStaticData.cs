/*using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


public class IconModelGroup
{
    // icon模型自己的名称
    public string ModelName { set; get; }

    // icon模型合并创建的对象名称，即盒子（icon + 底座）
    public string BoxName { set; get; }

    // label对象的名称内容
    public string LabelName { set; get;}

    // 盒子（icon + 底座）的场景位置信息 世界坐标
    public float[] ParentPosition { set; get; }

    // 盒子（icon + 底座）的场景位置信息相对盒子父节点 相对坐标 
    public float[] ParentLocalPosition { set; get; }

    // icon模型相对父节点的位置
    public float[] IconLocalPosition { set; get; }

    // icon模型的缩放大小
    public float[] IconLocalScale { set; get; }

} 

public class DictionaryStaticData : MonoBehaviour
{
    public static Dictionary<string, IconModelGroup> IconDicMsg = new Dictionary<string, IconModelGroup>();

    public string jsonVariable = "{}"; // JSON variable

    // [MenuItem("脚本执行/JSON", false, 1000)]
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
            IconDicMsg.Add(item.Name.ToString(), obj);
        }
        Debug.Log("IconDicMsg" + IconDicMsg);


        Debug.Log(JSONClass);
    }

    // var dataChildren = JSONClass.Children().Cast<JProperty>();

    *//*Dictionary<string, object> IconDicMsg = dataChildren
   .ToDictionary(x => x.Name, x => x.Value["val"].Value<JValue>().Value);*/

    /* var dataChildren = jObject["data"].Children().Cast<JProperty>();
     Dictionary<string, object> result = dataChildren
            .ToDictionary(x => x.Name, x => x.Value["val"].Value<JValue>().Value);*//*
    // 1. 获取字典信息JSON
    // string JSONData = JsonUtility.ToJson(m_Data);

    // 数据信息
    // public Dictionary<string, IconModelGroup> IconGroupMsg;


}
*/