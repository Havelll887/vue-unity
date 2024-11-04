using System.Xml;
using UnityEngine;

public class XMLReader : MonoBehaviour
{
    void Start()
    {
        // ���� XML �����ļ�
        XmlReader reader = XmlReader.Create("config.xml");

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == "name")
                {
                    Debug.Log("Name: " + reader.ReadElementContentAsString());
                }
                else if (reader.Name == "health")
                {
                    Debug.Log("Health: " + reader.ReadElementContentAsInt());
                }
                else if (reader.Name == "damage")
                {
                    Debug.Log("Damage: " + reader.ReadElementContentAsInt());
                }
            }
        }

        reader.Close();
    }
}