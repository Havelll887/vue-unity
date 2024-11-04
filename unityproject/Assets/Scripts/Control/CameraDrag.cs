using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Diagnostics;

public class CameraDrag : MonoBehaviour
{
    private float mouseTimer = 1;


    // 相机旋转参数
    float _rotationX;
    float rotationY;
    float sensitivityHor = 1.0f;
    float sensitivityVert = 1.0f;
    float minimumHor = 0;
    float maximumHor = 360.0f;
    float minimumVert = 0;
    float maximumVert = 180.0f;

    // 相机缩放参数
    float scrollSpeed = 10;//滑轮滚动速度
    float distance = 0;//位置偏移的向量长度
    private Vector3 offsetPosition; //位置偏移

    //旋转摄像头
    void CameraRot()
    {
        rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityHor;

        rotationY = Mathf.Clamp(rotationY, minimumHor, maximumHor);

        _rotationX = transform.localEulerAngles.x +  Input.GetAxis("Mouse Y") * sensitivityVert ;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);

        this.LimitArea();
    }

    // 滚轮缩放
    void CameraScroll()
    {
        distance = offsetPosition.magnitude; //位置偏移的向量长度
        float scroll = Input.GetAxis("Mouse ScrollWheel");  //向后滑动返回负值  向前滑动返回正值
        if (scroll != 0)
        {
            distance -= scroll * scrollSpeed;//获取滚轮值的改变
            // 限制摄像机的高度在minY和maxY之间
            offsetPosition = offsetPosition.normalized * distance;  //单位向量  方向不变 距离改变
            Vector3 newPos1 = transform.position + transform.forward * scroll * scrollSpeed;
            transform.position = newPos1;
            this.LimitArea();

        }
    }


    // 相机拖拽参数
    private Vector3 lastPosition;
    private Vector3 dragStartPos, dragCurrentPos;
    private Vector3 newPos;
    float moveGap = 1.0f;

    void CameraMove()
    {
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");

        if (moveX != 0 && moveY != 0) {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                //获取到这条射线到XZ平面上具体点的位子信息
                dragCurrentPos = ray.GetPoint(distance);
                Vector3 difference = dragStartPos - dragCurrentPos;
                newPos = transform.position + difference;
            }
            transform.position = Vector3.Lerp(transform.position, newPos, moveGap * Time.deltaTime);
            this.LimitArea();
        }

    }

    // 相机范围限制
    void LimitArea()
    {
        float scaleLimit = 0.5f;

        // 限制物体
        Collider collider = GameObject.Find("AreaSphereContent").GetComponent<Collider>();
        if(collider != null)
        {
            float x = Mathf.Clamp(transform.position.x, collider.bounds.min.x * scaleLimit, collider.bounds.max.x * scaleLimit);
            float z = Mathf.Clamp(transform.position.z, collider.bounds.min.z * scaleLimit, collider.bounds.max.z * scaleLimit);
            float y = Mathf.Clamp(transform.position.y, 10, collider.bounds.max.y * scaleLimit);
            transform.position = new Vector3(x, y, z);
        }


    }

    // 鼠标事件
    void MouseEvent()
    {
        // 左键
        if (Input.GetMouseButton(0))
        {
            mouseTimer += Time.deltaTime;
            newPos = transform.position;
            CameraMove();
        }
        else if (Input.GetMouseButtonUp(0) && mouseTimer != 0)
        {
            mouseTimer = 0;
        }
        // 右键
        if (Input.GetMouseButton(1))
        {
            mouseTimer += Time.deltaTime;
            CameraRot();
        }
        else if (Input.GetMouseButtonUp(1) && mouseTimer != 0)
        {
            mouseTimer = 0;
        }
        // 滚轮缩放
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            CameraScroll();
        }
    }


    void Start()
    {

    }

    void Update()
    {
        MouseEvent();
    }


}

