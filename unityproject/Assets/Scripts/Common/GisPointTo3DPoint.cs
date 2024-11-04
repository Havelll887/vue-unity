
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GisPointTo3DPoint : MonoBehaviour
{

    public static GisPointTo3DPoint Instance = null;

    public Transform BottomRightPoint; //Unity中右下点  （X正方向和Z轴的负方向之间）
    public Transform TopLeftPoint;//Unity中左上点  （Z轴正方向和X轴负方向之间）

    private Vector2 BottomRightSai;//地图中对应的左上经纬度点  
    private Vector2 TopLeftSai;//地图中对应的右下经纬度点  

    private float z_offset, x_offset, z_w_offset, x_w_offset;

    private RaycastHit rayHit;

    private void Awake()
    {            
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
        InitBasicNum();//初始化参数 
    }


    private void InitBasicNum()
    {
        //通过传过来的第一个点，确定坐标范围。以导弹为中心
        //右下
        BottomRightSai = new Vector2(118.62f, 24.37f);
        //左上
        TopLeftSai = new Vector2(113.30f, 30.20f);
        z_offset = BottomRightSai.y - TopLeftSai.y;//地图中的维度差  
        x_offset = BottomRightSai.x - TopLeftSai.x;//地图中的经度差  
        z_w_offset = BottomRightPoint.position.z - TopLeftPoint.position.z;//unity中的维度差  
        x_w_offset = BottomRightPoint.position.x - TopLeftPoint.position.x;//unity中的经度差 
    }

    public Vector3 GetWorldPoint(Vector2 se)
    {
#if UNITY_EDITOR
        InitBasicNum();
#endif
        float tempX = se.x - TopLeftSai.x;
        float tempZ = se.y - BottomRightSai.y;
        float _tempX = (tempX * x_w_offset / x_offset + TopLeftPoint.position.x);
        float _tempZ = (tempZ * z_w_offset / z_offset + BottomRightPoint.position.z);

        Debug.Log("chaned world Postition " + _tempX + "   " + _tempZ);
        //坐标偏差
        return new Vector3(_tempX, 0, _tempZ);
    }

    public Vector3 GetLatLon(Vector3 curPoint)
    {
#if UNITY_EDITOR
        InitBasicNum();
#endif
        //坐标偏差
        float _x_offset = (curPoint.x - BottomRightPoint.position.x) * x_offset / x_w_offset;
        float _z_offset = (curPoint.z - TopLeftPoint.position.z) * z_offset / z_w_offset;
        float resultX = _x_offset + BottomRightSai.x;
        float resultZ = _z_offset + TopLeftSai.y;

        Debug.Log("chaned GisPos " + resultX  + "  " + resultZ);
        return new Vector2(resultX, resultZ);
    }
}
