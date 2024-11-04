using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Diagnostics;

public class CameraDrag : MonoBehaviour
{
    private float mouseTimer = 1;


    // �����ת����
    float _rotationX;
    float rotationY;
    float sensitivityHor = 1.0f;
    float sensitivityVert = 1.0f;
    float minimumHor = 0;
    float maximumHor = 360.0f;
    float minimumVert = 0;
    float maximumVert = 180.0f;

    // ������Ų���
    float scrollSpeed = 10;//���ֹ����ٶ�
    float distance = 0;//λ��ƫ�Ƶ���������
    private Vector3 offsetPosition; //λ��ƫ��

    //��ת����ͷ
    void CameraRot()
    {
        rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityHor;

        rotationY = Mathf.Clamp(rotationY, minimumHor, maximumHor);

        _rotationX = transform.localEulerAngles.x +  Input.GetAxis("Mouse Y") * sensitivityVert ;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);

        this.LimitArea();
    }

    // ��������
    void CameraScroll()
    {
        distance = offsetPosition.magnitude; //λ��ƫ�Ƶ���������
        float scroll = Input.GetAxis("Mouse ScrollWheel");  //��󻬶����ظ�ֵ  ��ǰ����������ֵ
        if (scroll != 0)
        {
            distance -= scroll * scrollSpeed;//��ȡ����ֵ�ĸı�
            // ����������ĸ߶���minY��maxY֮��
            offsetPosition = offsetPosition.normalized * distance;  //��λ����  ���򲻱� ����ı�
            Vector3 newPos1 = transform.position + transform.forward * scroll * scrollSpeed;
            transform.position = newPos1;
            this.LimitArea();

        }
    }


    // �����ק����
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
                //��ȡ���������ߵ�XZƽ���Ͼ�����λ����Ϣ
                dragCurrentPos = ray.GetPoint(distance);
                Vector3 difference = dragStartPos - dragCurrentPos;
                newPos = transform.position + difference;
            }
            transform.position = Vector3.Lerp(transform.position, newPos, moveGap * Time.deltaTime);
            this.LimitArea();
        }

    }

    // �����Χ����
    void LimitArea()
    {
        float scaleLimit = 0.5f;

        // ��������
        Collider collider = GameObject.Find("AreaSphereContent").GetComponent<Collider>();
        if(collider != null)
        {
            float x = Mathf.Clamp(transform.position.x, collider.bounds.min.x * scaleLimit, collider.bounds.max.x * scaleLimit);
            float z = Mathf.Clamp(transform.position.z, collider.bounds.min.z * scaleLimit, collider.bounds.max.z * scaleLimit);
            float y = Mathf.Clamp(transform.position.y, 10, collider.bounds.max.y * scaleLimit);
            transform.position = new Vector3(x, y, z);
        }


    }

    // ����¼�
    void MouseEvent()
    {
        // ���
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
        // �Ҽ�
        if (Input.GetMouseButton(1))
        {
            mouseTimer += Time.deltaTime;
            CameraRot();
        }
        else if (Input.GetMouseButtonUp(1) && mouseTimer != 0)
        {
            mouseTimer = 0;
        }
        // ��������
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

