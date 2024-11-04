using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;


public class JumpTest : MonoBehaviour, IPointerClickHandler
{
    List<ChainItemDict> chainItemsList;
    InitMainChain InitMainChain;

    public void OnPointerClick(PointerEventData eventData)
    {
        print("点击了===OnPointerClick(" + this.name);

        string tempStr = " [{\"Id\":\"D50FEED71376FB8559EA200D20ABA295\",\"Pid\":\"\",\"Code\":\"\",\"Name\":\"现代家具产业链\",\"Level\":\"1\",\"StreamType\":\"0\",\"SortInner\":0,\"ModelName\":\"09jiaju\",\"IconLocalScale\":0.8,\"IconLocalPosition\":{\"x\":0.0,\"y\":2.0,\"z\":0.0,\"normalized\":{\"x\":0.0,\"y\":1.0,\"z\":0.0,\"magnitude\":1.0,\"sqrMagnitude\":1.0},\"magnitude\":2.0,\"sqrMagnitude\":4.0},\"IconLocalEulerAngles\":{\"x\":0.0,\"y\":0.0,\"z\":0.0,\"magnitude\":0.0,\"sqrMagnitude\":0.0},\"ParentLocalPosition\":{\"x\":-10.0,\"y\":0.0,\"z\":-6.0,\"normalized\":{\"x\":-0.857493,\"y\":0.0,\"z\":-0.5144958,\"magnitude\":1.0,\"sqrMagnitude\":1.00000012},\"magnitude\":11.6619034,\"sqrMagnitude\":136.0},\"ParentPosition\":{\"x\":1.0,\"y\":1.0,\"z\":1.0,\"normalized\":{\"x\":0.577350259,\"y\":0.577350259,\"z\":0.577350259,\"normalized\":{\"x\":0.5773503,\"y\":0.5773503,\"z\":0.5773503,\"magnitude\":1.00000012,\"sqrMagnitude\":1.00000024},\"magnitude\":0.99999994,\"sqrMagnitude\":0.99999994},\"magnitude\":1.73205078,\"sqrMagnitude\":3.0},\"IsLoadChild\":\"1\",\"ParentLocLen\":16.0,\"SubSpacing\":0.0,\"SubBetween\":0.0},{\"Id\":\"3DA9DACC17AC06131B7EBCE70B51D5E4\",\"Pid\":\"D50FEED71376FB8559EA200D20ABA295\",\"Code\":\"\",\"Name\":\"金属家具\",\"Level\":\"2\",\"StreamType\":\"0\",\"SortInner\":0,\"ModelName\":\"3J_xdjj_jsjj\",\"IconLocalScale\":0.8,\"IconLocalPosition\":{\"x\":0.0,\"y\":2.0,\"z\":0.0,\"normalized\":{\"x\":0.0,\"y\":1.0,\"z\":0.0,\"magnitude\":1.0,\"sqrMagnitude\":1.0},\"magnitude\":2.0,\"sqrMagnitude\":4.0},\"IconLocalEulerAngles\":{\"x\":0.0,\"y\":0.0,\"z\":0.0,\"magnitude\":0.0,\"sqrMagnitude\":0.0},\"ParentLocalPosition\":{\"x\":0.0,\"y\":0.0,\"z\":12.0,\"normalized\":{\"x\":0.0,\"y\":0.0,\"z\":1.0,\"magnitude\":1.0,\"sqrMagnitude\":1.0},\"magnitude\":12.0,\"sqrMagnitude\":144.0},\"ParentPosition\":{\"x\":1.0,\"y\":1.0,\"z\":1.0,\"normalized\":{\"x\":0.577350259,\"y\":0.577350259,\"z\":0.577350259,\"normalized\":{\"x\":0.5773503,\"y\":0.5773503,\"z\":0.5773503,\"magnitude\":1.00000012,\"sqrMagnitude\":1.00000024},\"magnitude\":0.99999994,\"sqrMagnitude\":0.99999994},\"magnitude\":1.73205078,\"sqrMagnitude\":3.0},\"IsLoadChild\":\"1\",\"ParentLocLen\":0.0,\"SubSpacing\":12.0,\"SubBetween\":15.0}]";
        
        List<ChainItemDict> receivedList = new List<ChainItemDict>();
        
        receivedList = JsonConvert.DeserializeObject<List<ChainItemDict>>(tempStr);


        GameObject.Find("MainScript").GetComponent<InitMainChain>().transmitList = receivedList;

        SceneManager.LoadScene("IndustrialIndex");

    }

    // Start is called before the first frame update
    void Start()
    {
        // 获取主函数
        InitMainChain = GameObject.Find("MainScript").GetComponent<InitMainChain>();
        // 获取公共类
        chainItemsList = InitMainChain.chainItemDictsList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
