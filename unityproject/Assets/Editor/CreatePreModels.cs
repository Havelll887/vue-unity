using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class CreatePreModels : EditorWindow
{

    /*    static ImportModels()
        {
            EditorApplication.update += Update;
        }

        static void Update()
        {
            Debug.Log("Updating");
        }*/

   /* [MenuItem("�ű�ִ��/�������ɵ���ģ��", false, 1000)]
    static void ShowPanel()
    {
        Rect wr = new Rect(0, 0, 100, 100);
        CreatMappingData window = (CreatMappingData)EditorWindow.GetWindowWithRect(typeof(CreatMappingData), wr, true, "CreatCtns");
    }

    private void OnGUI()
    {
    {
        this.Repaint();//ʵʱˢ��
        if (GUILayout.Button("ִ��1"))
        {
          *//*  new GameObject();
            GameObject go = new GameObject("Cube");
            Debug.Log("go" + go);*//*
        }
        if (GUILayout.Button("�ر�"))
        {
            this.Close();
        }
    }

    static void CreateModelsPre()
    {
        
        // AssetDatabase.LoadAssetAtPath("Assets/Models/Furniture/jiaju.fbx");
    }
*/

    /*   static ImportModels()
       {
         *//*  // ��Ҫ����ģ�͵��ļ�������
           string ModelFloadName = "Furniture";

           // 1. ��ȡĿ¼�µ�����FBXģ��
           // 1.1 �ļ�·��
           string orginPath = Application.dataPath + "/Models/" + ModelFloadName;
           DirectoryInfo direction = new DirectoryInfo(orginPath);
           FileInfo[] files = direction.GetFiles("*.FBX", SearchOption.AllDirectories);

           if (files.Length == 0) return;
           Debug.Log("models list length" + files.Length);

           for (int i = 0; i < files.Length; i++)
           {
               string[] pathNames = (files[i].FullName).Split(
                       new string[] { "Assets" },
                       StringSplitOptions.RemoveEmptyEntries
                   );

             *//*  string[] itemName = pathNames[1].Split(
                       new string[] { "/" },
                       StringSplitOptions.RemoveEmptyEntries
                   );*//*

               // 1.2 ��ȡ��Ӧ��ģ������
               string namePath = "Assets" + pathNames[1];
               // Debug.Log("item names " + itemName[1]);
               Debug.Log("items pathName " + i + "   " + namePath);
               ModelImporter modelImporter = AssetImporter.GetAtPath(namePath) as ModelImporter;

               //Undo.RecordObject(modelImporter, modelImporter.name);
               modelImporter.generateSecondaryUV = true;
               //EditorUtility.SetDirty(modelImporter);
               modelImporter.SaveAndReimport();

           }*//*

           // ModelImporter modelImporter = AssetImporter.GetAtPath("Assets/Models") as ModelImporter;
           // Debug.Log("fileSystemInfos" + directoryInfo);

           *//* 
           DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Models");
            * 
           FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
           Debug.Log("fileSystemInfos" + fileSystemInfos);

           Debug.Log("Up and running");*//*
       }*/
}
