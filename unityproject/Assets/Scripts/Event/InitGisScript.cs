using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InitGisScript : MonoBehaviour
{
    List<ChainItemDict> chainItemsList;
    InitMainChain InitMainChain;

    List<ChainItemDict> GettedChainList;

    private float ChainLinkGap = 4.5f;
    private float ChainPanelWidth = 3.0f;

    // transmitList

    private void InitChainLinks()
    {
        if(InitMainChain.transmitList.Count > 1)
        {
            GettedChainList = InitMainChain.GetListByPid(InitMainChain.transmitList[InitMainChain.transmitList.Count - 2].Id, chainItemsList);
            GameObject parentCon = GameObject.Find("CenterMainModel");

            if (!parentCon)
            {
                parentCon = new GameObject("CenterMainModel");
                parentCon.transform.localPosition = new Vector3(-3 , -3, -11);
                parentCon.transform.localEulerAngles = new Vector3(0 , 50 , 0);
                parentCon.transform.localScale = new Vector3(0.8f , 0.8f,0.8f);
            }

            // ��������  ��Ӧ��
            List<ChainItemDict> syChain = getChainLink("1", GettedChainList);

            // ��������  ������
            List<ChainItemDict> zyChain = getChainLink("2", GettedChainList);

            // ��������  ������
            List<ChainItemDict> xyChain = getChainLink("3", GettedChainList);

            float totalLength = Mathf.Max(syChain.Count,zyChain.Count, xyChain.Count) * ChainPanelWidth;

            Debug.Log("totalLength         " + totalLength);

            // ����
            if (syChain.Count > 0) {
                GameObject syChainBox = new GameObject("syChainBox");
                syChainBox.transform.parent = parentCon.transform;
                syChainBox.transform.localPosition = new Vector3(0 , ChainLinkGap * 3, 0);
                addPanelsShowIcons(syChainBox, totalLength);

                foreach (ChainItemDict item in syChain)
                {
                    InitMainChain.PutChainLinkModelByIndex(item, syChain, syChainBox, totalLength);
                }
            }

            // ����
            if (zyChain.Count > 0) {
                GameObject zyChainBox = new GameObject("zyChainBox");
                zyChainBox.transform.parent = parentCon.transform;
                zyChainBox.transform.localPosition = new Vector3(0, ChainLinkGap * 2, 0);
                addPanelsShowIcons(zyChainBox, totalLength);

                foreach (ChainItemDict item in zyChain)
                {
                    InitMainChain.PutChainLinkModelByIndex(item, zyChain, zyChainBox, totalLength);
                }
            }

            // ����
            if(xyChain.Count > 0)
            {
                GameObject xyChainBox = new GameObject("xyChainBox");
                xyChainBox.transform.parent = parentCon.transform;
                xyChainBox.transform.localPosition = new Vector3(0, ChainLinkGap * 1, 0);
                addPanelsShowIcons(xyChainBox, totalLength);
                foreach (ChainItemDict item in xyChain)
                {
                    InitMainChain.PutChainLinkModelByIndex(item, xyChain, xyChainBox, totalLength);
                }
            }
 
        }

    }

    private void addPanelsShowIcons(GameObject parentObject, float length)
    {
        GameObject Panel = (GameObject)Instantiate(Resources.Load("PanelBehavior"));
        Panel.transform.parent = parentObject.transform;
        Panel.transform.localScale = new Vector3 (length + 2 ,Panel.transform.localScale.y, Panel.transform.localScale.z);
        Panel.transform.localPosition = new Vector3((length / 2 )- 1.5f, 0, 0);
    }

    // ���ݲ�ҵ�����ڻ�ȡ��Ӧ��ͼ����Ϣ
    private List<ChainItemDict> getChainLink(string streamType, List<ChainItemDict> FilterList)
    {
        List <ChainItemDict> tempList = new List <ChainItemDict>();
        for (int i = 0; i < FilterList.Count; i++)
        {
            if (FilterList[i].StreamType == streamType)
            {
                tempList.Add (FilterList[i]);
            }
        }

        return tempList;
    }




    // Start is called before the first frame update
    void Start()
    {
        // ��ȡ������
        InitMainChain = GameObject.Find("MainScript").GetComponent<InitMainChain>();
        // ��ȡ������
        chainItemsList = InitMainChain.chainItemDictsList;
        // ��ʼ��
        InitChainLinks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
