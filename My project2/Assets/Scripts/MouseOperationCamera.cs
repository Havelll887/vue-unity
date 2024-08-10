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

    // �����¼�����
    public float scrollSpeed = 10;//���ֹ����ٶ�
    public float distance = 0;//λ��ƫ�Ƶ���������
    private Vector3 offsetPosition; //λ��ƫ��


    private Vector3 lastPosition;

    //��ת����ͷ
    void CameraRot()
    {
        rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityHor;
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
    }

    // ��������
    void CameraScroll()
    {  
        distance = offsetPosition.magnitude; //λ��ƫ�Ƶ���������
        float scroll = Input.GetAxis("Mouse ScrollWheel");  //��󻬶����ظ�ֵ  ��ǰ����������ֵ
        if (scroll != 0)
        {
            distance -= scroll * scrollSpeed;//��ȡ����ֵ�ĸı�
            // distance = Mathf.Clamp(distance, 2, 15);//���ƹ��־���ķ�Χ������ֵ�ɸ���ͬ����������Ӧ��ֵ
            // ����������ĸ߶���minY��maxY֮��
            // newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            offsetPosition = offsetPosition.normalized * distance;  //��λ����  ���򲻱� ����ı�
            Vector3 newPos = transform.position + transform.forward * scroll * scrollSpeed;
            transform.position = newPos;
        }
    }

    private Vector3 dragStartPos, dragCurrentPos;
    private Vector3 newPos;

    void CameraMove()
    {
        lastPosition = Input.mousePosition;
        // ��ȡ������ת����ֵ  ��Mouse X\ MouseY ����Դ�����ˮƽ������ƶ���ֵת������Ϊ-1 -  1 ֮�����ֵ
        float MoveY = Input.GetAxis("Mouse X"); //���ˮƽ�ƶ�����-1  -  +  1�������ֵ����������������Y����ת
        float MoveX = Input.GetAxis("Mouse Y"); //���ˮƽ�ƶ�����-1  -  +  1�������ֵ����������������X����ת

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
                //��ȡ���������ߵ�XZƽ���Ͼ�����λ����Ϣ
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
        // ���
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
            print("����������" + mouseTimer + "�룡");
            mouseTimer = 0;
        }
        // �Ҽ�
        if (Input.GetMouseButton(1))
        {
            mouseTimer += Time.deltaTime;
            CameraRot();
        }
        else if (Input.GetMouseButtonUp(1) && mouseTimer != 0)
        {
            print("����Ҽ�����" + mouseTimer + "�룡");
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

