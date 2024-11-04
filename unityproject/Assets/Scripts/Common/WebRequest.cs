using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


[SerializeField]
public class RequestObject
{
    public string url = "enterpris/v1/enterpriseLinkForm/listPage";
    public string method = "get";
    public string contentType = "application/json;charset=utf-8";
    public string requestDataType = "json";
    public Dictionary<string, string> requestParams;
    public Dictionary<string, string> requestData;

}

// 配置文件实体类
public class DataReceive
{
    public string tokenHeaderName;
    public string tokenStoreName;
    public string tokenPrefix;
    public string dicStoreName;
}

// 接口数据接收实体类
public class DataReceiveConfig
{
    public int code;
    public string msg;
    public object data;
}


public class WebRequest : MonoBehaviour
{

    public static string APIBasePath = "http://111.75.240.74:56794/industry_chain/api/";

    public string TokenPrefix = "";
    public string TokenKey = "";

    public string Token = "bearer 958e2d81-8291-421e-bb6b-b2a1af084228";


    [DllImport("__Internal")]
    private static extern void TokenInvalid(string str);


    public IEnumerator HttpRequestFunc(RequestObject requestObject, UnityAction<string> callBack = null)
    {
        if (string.IsNullOrEmpty(requestObject.url))
        {
            yield break;
        }
        Debug.Log("requestObjectrequestObject" + requestObject.url + "  " + requestObject.method);
        UnityWebRequest webRequest = new UnityWebRequest();


        // 请求方法
        if (requestObject.method.ToLower() == "get")
        {
            webRequest = UnityWebRequest.Get(APIBasePath + requestObject.url);
        }
        else if (requestObject.method.ToLower().Trim() == "post")
        {
            if(requestObject.requestDataType.ToLower().Trim() == "json")
            {
                string jsonData = JsonConvert.SerializeObject(requestObject.requestData);
                webRequest = UnityWebRequest.Post(APIBasePath + requestObject.url, jsonData);
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

            }
            /* WWWForm form = new WWWForm();
             if(requestObject.requestData.Count > 0)
             {
                 foreach (var item in requestObject.requestData)
                 {
                     form.AddField(item.Key, item.Value);
                 }
             }*/
        }

        // 设置token
        if (!string.IsNullOrEmpty(Token))
        {
            webRequest.SetRequestHeader(TokenKey, Token);
        }
        else
        {
        #if !UNITY_EDITOR && UNITY_WEBGL
            DataReceiveConfig dataTran = new DataReceiveConfig();
            dataTran.code = 401;
            dataTran.msg = "需要新的token";
            string strTran = JsonConvert.SerializeObject(dataTran);
            TokenInvalid(strTran);
        #endif
            // 没有获取到token，向web端重新请求code
            yield return null;

        }

            // 设置Content-Type
            webRequest.SetRequestHeader("Content-Type", !string.IsNullOrEmpty(requestObject.contentType) ?
                requestObject.contentType : "application/x-www-form-urlencoded;charset=utf-8");


        // 发送请求
        yield return webRequest.SendWebRequest();

        // 请求出现错误
        if (UnityWebRequest.Result.ProtocolError == webRequest.result || UnityWebRequest.Result.ConnectionError == webRequest.result)
        {
            Debug.Log("webRequest.error    " + webRequest.error);
            Debug.Log("webRequest.responseCode    " + webRequest.responseCode);
        }
        else
        {
            // 请求成功
            string text = webRequest.downloadHandler.text;

            if (string.IsNullOrEmpty(text))
            {
                Debug.Log(" 接口未返回参数  " + text);
            }
            else
            {
                DataReceiveConfig responeObject = JsonConvert.DeserializeObject<DataReceiveConfig>(text);
                Debug.Log(" 111111  " + responeObject + "   -----   " + responeObject.data);
                if ((int)responeObject.code == 200)
                {
                    Debug.Log("success  ");
                    if (callBack != null)
                    {
                        callBack(text);

                    }
                }
                else
                {
                    if ((int)responeObject.code == 401)
                    {
                        #if !UNITY_EDITOR && UNITY_WEBGL
                            TokenInvalid(text);
                        #endif
                        if (callBack != null)
                        {
                            callBack(text);

                        }
                    }
                }
            }
        }
    }

    void TestGetData()
    {
        RequestObject requestObject = new RequestObject();

        requestObject.method = "POST";

        Dictionary<string, string> data = new Dictionary<string, string>();

        data.Add("page", "1");
        data.Add("size", "10");

        requestObject.requestData = data;

        StartCoroutine(HttpRequestFunc(requestObject, Logging));
    }

    private static void  Logging(string theProcessedStr)
    {
        // dynamic responeObject = Newtonsoft.Json.JsonConvert.DeserializeObject(theProcessedStr);
        Debug.Log("12 " + theProcessedStr);
        // Debug.Log("12 " + responeObject);
    }

    // 获取本地配置文件
    public void GetRequestConfig()
    {

        TextAsset jsonTextAsset = Resources.Load<TextAsset>("requestConfig");

        string jsonFileContent = jsonTextAsset.text;

        DataReceive configObject = JsonConvert.DeserializeObject<DataReceive>(jsonFileContent);

        TokenKey = configObject.tokenHeaderName;
        TokenPrefix = configObject.tokenPrefix;
    }


    public void GetTokenFromWeb(string token)
    {
        Token = TokenPrefix + token;
        Debug.Log("token   " + Token);
    }


    void Start()
    {

        GetRequestConfig();

        TestGetData();
    }

    void Update()
    {
        
    }
}