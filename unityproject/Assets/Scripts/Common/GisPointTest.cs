using UnityEngine;

public class GisPointTest : MonoBehaviour
{


    public Vector2 target = new Vector2(115.8540042f, 28.687547f);
    private GisPointTo3DPoint cel;

#if UNITY_EDITOR
    [ContextMenu("定位南昌")]
#endif
    private void Test()
    {
        cel = GameObject.Find("MainScript").GetComponent<GisPointTo3DPoint>();
        Vector3 vector = cel.GetWorldPoint(target);
        GameObject obj = new GameObject("located Test");
        obj.transform.position = vector;
    }

}
