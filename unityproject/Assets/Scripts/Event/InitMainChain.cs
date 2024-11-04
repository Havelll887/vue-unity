using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System.Reflection;


using System.Linq;
using UnityEngine.Playables;
using static UnityEngine.GraphicsBuffer;



#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using static UnityEditor.Progress;
using static UnityEngine.Networking.UnityWebRequest;
#endif

[Serializable]
public class ChainItemDict
{
    // 字典项ID
    public string Id { get; set; }

    // 字典项父级ID
    public string Pid { get; set; }

    // 字典Code
    public string Code { get; set; }

    // 字典名称
    public string Name { get; set; }

    // 字典层级
    public string Level { get; set; }

    // 字典上下游类型
    public string StreamType { get; set; }

    // 字典排序顺序
    public int SortInner { get; set; }

    // 使用的模型名称
    public string ModelName { get; set; }

    public float IconLocalScale { get; set; }
    public Vector3 IconLocalPosition { get; set; }
    public Vector3 IconLocalEulerAngles { get; set; }
    public Vector3 ParentLocalPosition { get; set; }
    public Vector3 ParentPosition { get; set; }

    // 是否同步加载子级
    public string IsLoadChild { get; set; }

    // 针对第一级，与中心点的间距
    public float ParentLocLen { get; set; }

    // 针对第2 3 级 与上一级的最短距离
    public float SubSpacing { get; set; }

    // 针对第2 3 级 兄弟间的间距
    public float SubBetween {  get; set; }

}

public class InitMainChain : MonoBehaviour
{

    public List<ChainItemDict> chainItemDictsList =  new List<ChainItemDict>();

    // 场景传输用的数据
    public List<ChainItemDict> transmitList = new List<ChainItemDict>();

    // 场景初始化 是否加载图标模型
    public Boolean addICons = false;

    public static InitMainChain Instance { get; private set; }

    // 底座模型使用常量
    // 1J_Variant    2J_Variant 3J_Variant
    // private String BaseLevel1 = "2J_dizuo Variant";
    // private String BaseLevel2 = "3J_dizuo Variant";

    private String BaseLevel1 = "2J_Variant";
    private String BaseLevel2 = "3J_Variant";
    private String BaseLevel3Stream1 = "4J_Dz_Blue Variant";
    private String BaseLevel3Stream2 = "4J_Green Variant";
    private String BaseLevel3Stream3 = "4J_Yellow Variant";


void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("唤醒 场景初始化操作执行qAwake");

        if (addICons)
        {

            InitMainChain InitMainChain = GameObject.Find("MainScript").GetComponent<InitMainChain>();
            chainItemDictsList = InitMainChain.chainItemDictsList;
            transmitList = InitMainChain.transmitList;

            // 首次加载模型
            InitModelsInScenceByType(null, null, true);

            if (transmitList.Count > 0)
            {
                Debug.Log("qAwake存在需要同步加载的内容 ");
                initSubChains(transmitList[transmitList.Count - 1]);
                transmitList.Clear();

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 字典初始化加载
        chainItemDictsList = new List<ChainItemDict>();
        CreateIconByCSV();

        if (addICons)
        {

            // 首次加载模型
            InitModelsInScenceByType(null, null, true);

            if (transmitList.Count > 0)
            {
                Debug.Log("存在需要同步加载的内容 ");
                initSubChains(transmitList[transmitList.Count - 1]);
                transmitList.Clear();

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Text2Camera();
    }


#if UNITY_EDITOR
    [ContextMenu("csv文件生成测试")]
#endif
    public void CreateIconByCSV()
    {
        TextAsset DictDataText = Resources.Load("industry_chain_map") as TextAsset;

        // return;

        // 1. 获取并解析文件
        // string filePath = Application.dataPath + "/StaticData/iconData/industry_chain_map.csv";
        // string[] fileData = File.ReadAllLines(filePath);

        string text = DictDataText.text;  // 将文本内容作为字符串读取
        string[] fileData = text.Split('\n'); // 以\n为分割符将文本分割为一个数组

        /* CSV文件的第一行为Key字段，第二行开始是数据。第一个字段一定是ID。 */
        string[] keys = fileData[0].Split(',');
        
        for (int i = 1; i < fileData.Length; i++)
        {
            string[] line = fileData[i].Split(',');

            ChainItemDict chainItem = new ChainItemDict();

            PropertyInfo[] propertyInfos = typeof(ChainItemDict).GetProperties();

            foreach (var item in propertyInfos)
            {

                for (int j = 0; j < line.Length; j++)
                {
                   
                    if ((keys[j]!=null || keys[j].Length>0) && item.Name.ToLower() == keys[j].Trim().ToLower() && (line[j] != null || line[j].Length >0 ))
                    {

                        if (item.PropertyType.Name == "Int32")
                        {
                            item.SetValue(chainItem, line[j] != null && line[j].Length != 0 ? int.Parse(line[j]) : 0);
                        }
                        else if (item.PropertyType.Name == "String")
                        {
                            item.SetValue(chainItem, line[j] != null && line[j].Length != 0 ? line[j] : "");
                        }
                        else if (item.PropertyType.Name == "Single")
                        {
                            line[j] = line[j].Trim();
                            item.SetValue(chainItem, 0);
                            item.SetValue(chainItem, (line[j].Length > 0) ? float.Parse(line[j]) : 0);

                        }
                        else if (item.PropertyType.Name == "Vector3")
                        {
                            if (line[j] != null && line[j].Length > 0)
                            {
                                string[] tempList = line[j].Trim().Split('/');
                                item.SetValue(chainItem, new Vector3(
                                    tempList[0].Length > 0 ? float.Parse(tempList[0]) : 1.0f,
                                    tempList[1].Length > 0 ? float.Parse(tempList[1]) : 1.0f,
                                    tempList[2].Length > 0 ? float.Parse(tempList[2]) : 1.0f
                                    ));
                            }
                            else
                            {
                                item.SetValue(chainItem, new Vector3(1, 1, 1));

                            }
                        }
                       
                    }
                }
            }
            
            if(chainItem != null && chainItem.Id != null && chainItem.Id.Length > 0)
            {
                chainItemDictsList.Add(chainItem);

                // 去重
                Dictionary<string, ChainItemDict> result = new Dictionary<string, ChainItemDict>();

                foreach (ChainItemDict item in chainItemDictsList)//list为待去重列表
                {
                    ChainItemDict temp;
                    if (!result.TryGetValue(item.Id, out temp))
                    {
                        result.Add(item.Id, item);
                    }
                }
                chainItemDictsList = result.Values.ToList();
            }
        }
        chainItemDictsList = chainItemDictsList.OrderBy(i => i.Level).ThenBy(i=> i.StreamType).ThenBy(i => i.SortInner).ToList();
    }

    public void InitModelsInScenceByType(string pid, GameObject gameObject, bool moreLevel = false)
    {
        // 获取到对应的数据信息
        List<ChainItemDict> chains = GetListByPid(pid, chainItemDictsList);

        GameObject parentCon = gameObject;

        if (!parentCon)
        {

            parentCon = GameObject.Find("CenterMainModel");

            if (parentCon == null)
            {
                parentCon = new GameObject("CenterMainModel");
            }

        }


        // 遍历加载
        for (int i = 0;i< chains.Count;i++)
        {
            InitModelsBySingleMsg(chains[i], parentCon, chains.Count, i, moreLevel, i == 0 ? null : chains[i-1]);
        }
    }

    public void InitModelsBySingleMsg(ChainItemDict chainItemDict, GameObject parentCon, int TotalCount, int selfIndex, bool moreLevel = false, ChainItemDict lastChainItem = null)
    {
        if (chainItemDict.ModelName != null && chainItemDict.ModelName.Length > 0)
        {
            // 查找当前目标是否已存在
            GameObject IconParent = GameObject.Find(chainItemDict.Id);

            string levelBase = "";
            // line = gameObject.GetComponent<LineRenderer>();

            // 根据层级获取底座模型
            if (chainItemDict.Level == "1")
            {
                levelBase = BaseLevel1;
            }
            else if (chainItemDict.Level == "2")
            {
                levelBase = BaseLevel2;

            }
            else if (chainItemDict.Level == "3")
            {
                if (chainItemDict.StreamType == "1")
                {
                    levelBase = BaseLevel3Stream1;

                }
                else if (chainItemDict.StreamType == "2")
                {
                    levelBase = BaseLevel3Stream2;

                }
                else
                {
                    levelBase = BaseLevel3Stream3;

                }
            }

            // 不存在创建
            if (!IconParent)
            {
                IconParent = new GameObject(chainItemDict.Id);  // 设置名称
                IconParent.transform.SetParent(parentCon.transform, false);  // 设置父级

                // 设置定位
                // 一级图标模型，围绕中间均匀平分
                if (chainItemDict.Level == "1")
                {
                    // 计算角度
                    double x = Math.Sin(((Math.PI / 180) * (360 / TotalCount)) * (selfIndex));
                    double y = Math.Cos(((Math.PI / 180) * (360 / TotalCount)) * (selfIndex));

                    IconParent.transform.localPosition = new Vector3(
                        (float)(y * chainItemDict.ParentLocLen), 0, (float)(x * chainItemDict.ParentLocLen)
                    );

                    List <Vector3> posList = new List <Vector3>();
                    posList.Add(new Vector3(0, 0, 0));
                    posList.Add(IconParent.transform.position);

                    AddedLineRender(IconParent, posList);
                }
                else if (chainItemDict.Level == "2")
                {
                    // 1. 拿到父级的世界坐标计算向量
                    // 向量
                    Vector3 vectorCenter = parentCon.transform.position.normalized * (chainItemDict.SubSpacing / 2);
                    // 垂直向量
                    Vector3 vectorColumn = GetVerticalDir(vectorCenter);

                    // 计算一下需要的分布点
                    int totalGap = TotalCount * 6;
                    Vector3 start = (vectorColumn * (totalGap / 2));
                    Vector3 end = -(vectorColumn * (totalGap / 2));
                    IconParent.transform.localPosition = ((start - end) / TotalCount) * (selfIndex - 1) + vectorCenter;

                    List<Vector3> posList = new List<Vector3>();
                    posList.Add(parentCon.transform.position);
                    posList.Add(IconParent.transform.position);

                    AddedLineRender(IconParent, posList);
                }
                else
                {
                    IconParent.transform.localPosition = new Vector3(
                        chainItemDict.ParentLocalPosition[0], chainItemDict.ParentLocalPosition[1], chainItemDict.ParentLocalPosition[2]);
                    List<Vector3> posList = new List<Vector3>();
                    // 当是第一个的时候
                    if(chainItemDict.StreamType == "1" && chainItemDict.SortInner == 1)
                    {
                        // posList.Add(parentCon.transform.position);
                        GameObject obj = GameObject.Find(chainItemDict.Pid);
                        Debug.Log("rrrchainItemDict " + obj.name);

                        if (obj != null)
                        {
                            posList.Add(obj.transform.position);
                            posList.Add(IconParent.transform.position);
                        }
                        AddedLineRender(IconParent, posList);
                    }
                    else
                    {
                        if(lastChainItem != null)
                        {
                            GameObject last = GameObject.Find(lastChainItem.Id);
                            if (last != null)
                            {
                                posList.Add(last.transform.position);
                                posList.Add(IconParent.transform.position);
                            }
                            AddedLineRender(IconParent, posList);
                        }

                    }

                }

                IconParent.transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                IconParent.transform.localPosition = new Vector3(
                    chainItemDict.ParentLocalPosition[0], chainItemDict.ParentLocalPosition[1], chainItemDict.ParentLocalPosition[2]
                    );
            }

            // 底座
            GameObject go = (GameObject)Instantiate(Resources.Load(levelBase));
            go.transform.SetParent(IconParent.transform, false);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localEulerAngles = Vector3.zero;

            // 模型
            GameObject iconModel = (GameObject)Instantiate(Resources.Load(chainItemDict.ModelName + " Variant"));
            iconModel.transform.SetParent(IconParent.transform, false);
            iconModel.transform.localPosition = new Vector3(chainItemDict.IconLocalPosition[0], chainItemDict.IconLocalPosition[1], chainItemDict.IconLocalPosition[2]);
            iconModel.transform.localEulerAngles = new Vector3(chainItemDict.IconLocalEulerAngles[0], chainItemDict.IconLocalEulerAngles[1], chainItemDict.IconLocalEulerAngles[2]);
            iconModel.transform.localScale = new Vector3(1 * chainItemDict.IconLocalScale, 1 * chainItemDict.IconLocalScale, 1 * chainItemDict.IconLocalScale);

            // 标签
            AddedTextMesh(IconParent, chainItemDict, new Vector3(0,3.5f,0));
/*            GameObject TextObj = (GameObject)Instantiate(Resources.Load("TextPre"));
            TextObj.transform.SetParent(IconParent.transform, false);
            TextObj.transform.localPosition = new Vector3(0, 3.5f, 0);
            TextObj.transform.localEulerAngles = new Vector3(0, 90, 0);

            TextMeshProUGUI textMeshProUGUI = TextObj.GetComponentInChildren<TextMeshProUGUI>();
            textMeshProUGUI.text = chainItemDict.Name;*/


            // 说明需要同步加载子级
            if (chainItemDict.IsLoadChild != null && chainItemDict.IsLoadChild.Length > 0 && moreLevel)
            {
                GameObject SubChain = new GameObject("subchains");
                SubChain.transform.SetParent(IconParent.transform, false);
                SubChain.transform.localPosition = Vector3.zero;

                // 获取子级列表
                InitModelsInScenceByType(chainItemDict.Id, SubChain, false);
            }

        }
    }

    public static Vector3 GetVerticalDir(Vector3 _dir)
    {
        //（_dir.x,_dir.z）与（？，1）垂直，则_dir.x * ？ + _dir.z * 1 = 0
        if (_dir.z == 0)
        {
            return new Vector3(0, 0, -1);
        }
        else
        {
            return new Vector3(-_dir.z / _dir.x, 0, 1).normalized;
        }
    }

    public static List<ChainItemDict> GetListByPid(string pid, List<ChainItemDict> TotalList)
    {
        List<ChainItemDict> chains = new List<ChainItemDict>();
        // pid为空，则取第一级
        if (string.IsNullOrEmpty(pid))
        {
            for (int i = 0; i < TotalList.Count; i++)
            {
                if (TotalList[i].Pid.Length == 0)
                {
                    chains.Add(TotalList[i]);
                }
            }
        }
        else
        {

            for (int i = 0; i < TotalList.Count; i++)
            {
                if (TotalList[i].Pid.Length > 0 && pid == TotalList[i].Pid)
                {
                    chains.Add(TotalList[i]);

                }
            }
        }
        return chains;
    }

    public static ChainItemDict GetMsgByid(string id, List<ChainItemDict> TotalList) { 
        ChainItemDict chains = new ChainItemDict();
        if (!string.IsNullOrEmpty(id)) {
            for (int i = 0; i < TotalList.Count; i++)
            {

                if (id == TotalList[i].Id)
                {
                    chains = TotalList[i];
                    return chains;
                }
            }
            return chains;
        }
        else
        {
            return chains;

        }

    }

    // 线段材质
    public void AddedLineRender(GameObject gameObject, List<Vector3> posList, String materialType = "blue")
    {
        LineRenderer line = gameObject.AddComponent<LineRenderer>();

        // line.material = new Material(Shader.Find("Particles/Additive"));
        line.positionCount = posList.Count;
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        for (int i = 0;i < posList.Count;i++)
        {
            line.SetPosition(i, new Vector3(posList[i].x, posList[i].y + 0.4f, posList[i].z));
        }

    }

    // 标签材质
    public void AddedTextMesh(GameObject gameObject, ChainItemDict chainItem, Vector3 vector3)
    {

        GameObject TextObj = (GameObject)Instantiate(Resources.Load("TextPre"));
        TextObj.transform.SetParent(gameObject.transform, false);
        TextObj.transform.localPosition = vector3;

        TextMeshProUGUI textMeshProUGUI = TextObj.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.text = chainItem.Name;
    }

    public void Text2Camera()
    {
        GameObject[] LabelList = GameObject.FindGameObjectsWithTag("TextLabel");
        Camera cam = Camera.main;
        for(int i = 0; i < LabelList.Length ; i++)
        {
            GameObject label = LabelList[i];
            // Player.transform.LookAt(LookTarget.position);
            label.transform.LookAt(cam.transform.position);
        }
    }


    // 点击加载方法
    public void GetLevel2ModelsById(string id)
    {
        // id为空不执行方法
        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        // 根据id查找该节点的信息
 

        // 查找level1 下级的内容
        List<ChainItemDict> chainsTempLists = GetListByPid(id, chainItemDictsList);

        // 根节点 节点信息
        GameObject rootGameObject = GameObject.Find("CenterMainModel");
        Transform targetTransform = rootGameObject.transform.Find(id);
        targetTransform.gameObject.GetComponent<LineRenderer>().enabled = false;

        // activeSelf    activeInHierarchy

        // 获取二级节点
        if (chainsTempLists.Count > 0 && chainsTempLists[0].Level == "2")
        {

            for (int i = 0; i < chainsTempLists.Count; i++)
            {
                Transform transformSub = targetTransform.transform.Find("subchains/" + chainsTempLists[i].Id);

                // 子节点已加载
                if (transformSub != null)
                {
                    Transform SubChainTransform = transformSub.Find("subchainsc");

                    if (SubChainTransform == null)
                    {
                        transformSub.transform.localPosition = transformSub.transform.localPosition * 2.5f;
                        LineRenderer lineRenderer = transformSub.gameObject.GetComponent<LineRenderer>();
                        if (lineRenderer != null)
                        {
                            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transformSub.position);
                        }
                        else
                        {
                            List<Vector3> tempList = new List<Vector3>();
                            tempList.Add(targetTransform.position);
                            tempList.Add(transformSub.position);
                            AddedLineRender(transformSub.gameObject, tempList);
                        }

                        GameObject SubChain = new GameObject("subchainsc");
                        SubChain.transform.SetParent(transformSub, false);
                        SubChain.transform.localPosition = Vector3.zero;


                        // 偏移角度为与Z轴正方向的夹角

                        // 判断左右
                        
                        float dir = Vector3.Dot(targetTransform.position.normalized, new Vector3 ( 0 , 0 , 1));
                        if(dir != 0)
                        {
                            dir = dir / Mathf.Abs(dir);
                        }
                        Debug.Log("dir   " + dir);
                        Debug.Log("targetTransform.name  " + targetTransform.name);


                        float angle = Vector3.Angle(targetTransform.position, new Vector3(0, 0, 1));
                        print("角度B：" + angle);
                        if (dir > 0)
                        {
                            angle = angle - 90;
                        }
                        SubChain.transform.localRotation = Quaternion.Euler(0,  dir * angle, 0);

                        InitModelsInScenceByType(chainsTempLists[i].Id, SubChain, false);

                    }
                }
                else
                {
                    Transform SubChainItem = targetTransform.Find("subchains");

                    InitModelsBySingleMsg(chainsTempLists[i], SubChainItem.gameObject, chainsTempLists.Count, i, false);

                }
            }

        }
        else if (chainsTempLists.Count > 0 && chainsTempLists[0].Level == "3")
        {
            Debug.Log("rrrrrqqq" + "直接下级level 3    产业链环节 " + chainsTempLists.Count);
        }
        else
        {
            Debug.Log("无下级 子产业链 或 产业链环节");

        }


    }

    void initSubChains(ChainItemDict chainItem)
    {
        GameObject gameObject;
        string initId = "";

        if (chainItem.Level == "2")
        {
            gameObject = GameObject.Find(chainItem.Pid);
            initId = chainItem.Pid;

        }
        else
        {
            gameObject = GameObject.Find(chainItem.Id);
            initId = chainItem.Id;

        }

        // 获取盒子的父级
        Transform ppTransform = gameObject.transform.parent;

        // 获取兄弟节点
        int childCount = ppTransform.childCount;

        // 自己排第几
        int selfIndex = gameObject.transform.GetSiblingIndex();

        for (int i = 0; i < childCount; i++)
        {
            // 除自己外的兄弟节点隐藏
            if (selfIndex != i)
            {
                ppTransform.GetChild(i).gameObject.SetActive(false);
            }
        }

        Debug.Log("conssss " + initId);
        Debug.Log("conssss  " + gameObject.name);
        // 加载节点
        GetLevel2ModelsById(initId);
    }

    public void PutChainLinkModelByIndex(ChainItemDict chainItemDict, List<ChainItemDict> TotalList, GameObject parentObject, float ChainPanelWidth = 10.0f)
    {
        string levelBase = "";

        int totalCount = TotalList.Count;

         if (chainItemDict.StreamType == "1")
         {
             levelBase = BaseLevel3Stream1;

        }
         else if (chainItemDict.StreamType == "2")
         {
             levelBase = BaseLevel3Stream2;

        }
         else
         {
             levelBase = BaseLevel3Stream3;

         }

        // 查找当前目标是否已存在
        GameObject IconParent = GameObject.Find(chainItemDict.Id);


        // 不存在创建
        if (!IconParent)
        {
            IconParent = new GameObject(chainItemDict.Id);  // 设置名称
            IconParent.transform.SetParent(parentObject.transform, false);  // 设置父级
            IconParent.transform.localPosition = new Vector3((((float)totalCount - (float)chainItemDict.SortInner) / (float)totalCount) * ChainPanelWidth, 0 , 0);

        }
        else
        {
            IconParent.transform.localPosition = new Vector3((((float)totalCount - (float)chainItemDict.SortInner) / (float)totalCount) * ChainPanelWidth, 0, 0);

        }

        // 底座
        GameObject go = (GameObject)Instantiate(Resources.Load(levelBase));
        go.transform.SetParent(IconParent.transform, false);
        go.transform.localPosition = new Vector3(0, 0, 0);
        go.transform.localEulerAngles = Vector3.zero;

        // 模型
        GameObject iconModel = (GameObject)Instantiate(Resources.Load(chainItemDict.ModelName + " Variant"));
        iconModel.transform.SetParent(IconParent.transform, false);
        iconModel.transform.localPosition = new Vector3(chainItemDict.IconLocalPosition[0], chainItemDict.IconLocalPosition[1], chainItemDict.IconLocalPosition[2]);
        iconModel.transform.localEulerAngles = new Vector3(chainItemDict.IconLocalEulerAngles[0], chainItemDict.IconLocalEulerAngles[1], chainItemDict.IconLocalEulerAngles[2]);
        iconModel.transform.localScale = new Vector3(1 * chainItemDict.IconLocalScale, 1 * chainItemDict.IconLocalScale, 1 * chainItemDict.IconLocalScale);

        AddedTextMesh(IconParent, chainItemDict,new Vector3(0 , 3.0f, 0));

        if(chainItemDict.SortInner > 1)
        {
            GameObject tempObj = GameObject.Find(TotalList[(int)chainItemDict.SortInner - 2].Id);
/*            Debug.Log("eeeeeeeee   " + (int)chainItemDict.SortInner + "   " + TotalList[(int)chainItemDict.SortInner - 2].Id);
            Debug.Log("eeeeeeeee   " + tempObj);*/

            if (tempObj != null) 
            {
                List<Vector3> tempList = new List<Vector3>();
                tempList.Add(tempObj.transform.position);
                tempList.Add(IconParent.transform.position);
                AddedLineRender(IconParent, tempList);
            }
        }

    }

}
