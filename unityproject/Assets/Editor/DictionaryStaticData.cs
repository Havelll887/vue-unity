/*using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


public class IconModelGroup
{
    // iconģ���Լ�������
    public string ModelName { set; get; }

    // iconģ�ͺϲ������Ķ������ƣ������ӣ�icon + ������
    public string BoxName { set; get; }

    // label�������������
    public string LabelName { set; get;}

    // ���ӣ�icon + �������ĳ���λ����Ϣ ��������
    public float[] ParentPosition { set; get; }

    // ���ӣ�icon + �������ĳ���λ����Ϣ��Ժ��Ӹ��ڵ� ������� 
    public float[] ParentLocalPosition { set; get; }

    // iconģ����Ը��ڵ��λ��
    public float[] IconLocalPosition { set; get; }

    // iconģ�͵����Ŵ�С
    public float[] IconLocalScale { set; get; }

} 

public class DictionaryStaticData : MonoBehaviour
{
    public static Dictionary<string, IconModelGroup> IconDicMsg = new Dictionary<string, IconModelGroup>();

    public string jsonVariable = "{}"; // JSON variable

    // [MenuItem("�ű�ִ��/JSON", false, 1000)]
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
    // 1. ��ȡ�ֵ���ϢJSON
    // string JSONData = JsonUtility.ToJson(m_Data);

    // ������Ϣ
    // public Dictionary<string, IconModelGroup> IconGroupMsg;


}
*/