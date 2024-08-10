using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        print("µã»÷ÁË£º£º" + this.name);

        #if !UNITY_EDITOR && UNITY_WEBGL
            PostScore("helly mooo");
        #endif
    }

    [DllImport("__Internal")]
    private static extern void PostScore(string currentScene);
}