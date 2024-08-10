using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class CameraDrag : MonoBehaviour
{
    float _rotationX;
    float rotationY;
    public float sensitivityHor = 3.0f;
    public float sensitivityVert = 3.0f;
    public float minimumVert = -35.0f;
    public float maximumVert = 35.0f;
    private float mouseTimer = 1;

    // 滚轮事件参数
    public float scrollSpeed = 10;//滑轮滚动速度
    public float distance = 0;//位置偏移的向量长度
    private Vector3 offsetPosition; //位置偏移


    private Vector3 lastPosition;

    //旋转摄像头
    void CameraRot()
    {
        rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityHor;
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
    }

    // 滚轮缩放
    void CameraScroll()
    {  
        distance = offsetPosition.magnitude; //位置偏移的向量长度
        float scroll = Input.GetAxis("Mouse ScrollWheel");  //向后滑动返回负值  向前滑动返回正值
        if (scroll != 0)
        {
            distance -= scroll * scrollSpeed;//获取滚轮值的改变
            // distance = Mathf.Clamp(distance, 2, 15);//限制滚轮距离的范围，此数值可根不同需求设置相应的值
            // 限制摄像机的高度在minY和maxY之间
            // newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            offsetPosition = offsetPosition.normalized * distance;  //单位向量  方向不变 距离改变
            Vector3 newPos = transform.position + transform.forward * scroll * scrollSpeed;
            transform.position = newPos;
        }
    }

    private Vector3 dragStartPos, dragCurrentPos;
    private Vector3 newPos;

    void CameraMove()
    {
        lastPosition = Input.mousePosition;
        // 获取鼠标的旋转输入值  ，Mouse X\ MouseY 是来源鼠标在水平方向的移动数值转换返回为-1 -  1 之间的数值
        float MoveY = Input.GetAxis("Mouse X"); //鼠标水平移动返回-1  -  +  1，这个数值将来用来给物体绕Y轴旋转
        float MoveX = Input.GetAxis("Mouse Y"); //鼠标水平移动返回-1  -  +  1，这个数值将来用来给物体绕X轴旋转

        if (MoveY != 0 && MoveX != 0)
        {
            //Texture2D cursorTex = Utils.LoadTexture("hand");
            // Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);
            Vector3 moveDir = (MoveX * - transform.right + MoveY * - transform.up);
            // moveDir.y = 0;
            transform.position -= moveDir * 0.5f;

            /* Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * Time.deltaTime, -delta.y * Time.deltaTime, 0);
            lastPosition = Input.mousePosition;

            print("RotaY" + MoveY);
            print("RotaX" + MoveX);
            print(" Input.mousePosition" + Input.mousePosition + "   " + Time.deltaTime);*/
        }
        
/*        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                dragStartPos = ray.GetPoint(distance);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                //获取到这条射线到XZ平面上具体点的位子信息
                dragCurrentPos = ray.GetPoint(distance);
                Vector3 difference = dragStartPos - dragCurrentPos;
                Debug.Log(difference);
                newPos = transform.position + difference;
            }
        }
        transform.position = Vector3.Lerp(transform.position, newPos, 1 * Time.deltaTime);*/

        print("transform.position" + transform.position);

    }

    void OnMouseDown()
    {
        lastPosition = Input.mousePosition;
    }
    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastPosition;
        transform.Translate(-delta.x * Time.deltaTime, -delta.y * Time.deltaTime, 0);
        lastPosition = Input.mousePosition;
    }

    void MouseEvent()
    {
        // 左键
        if (Input.GetMouseButton(0))
        {
            mouseTimer += Time.deltaTime;
            newPos = transform.position;
            CameraMove();
            // OnMouseDown();
            // OnMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0) && mouseTimer != 0)
        {
            print("鼠标左键长按" + mouseTimer + "秒！");
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
            print("鼠标右键长按" + mouseTimer + "秒！");
            mouseTimer = 0;
        }

        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
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

