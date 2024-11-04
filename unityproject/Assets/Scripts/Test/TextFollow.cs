using UnityEngine;

// 锁定标志的控制类
public class AimMark : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private Canvas ParentCanvas { get; set; }                   // 隶属于的Canvas
    private Camera CanvasCamera { get; set; }                   // Canvas的渲染相机
    private RectTransform ParentRectTransform { get; set; }     // 父节点的RectTransform    
    private Transform Target { get; set; }                      // 锁定的目标
    private Camera mainCamera;                                  // 游戏画面的主相机 
    private Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            return mainCamera;
        }
    }

    // 指定一个显示锁定标志的屏幕范围
    private Rect ViewProtRect { get; } = new Rect(0f, 0f, 1f, 1f);
    // 记录上一帧是否在范围内的状态，用于和当前状态对比得到转换的瞬间
    private bool IsInScreen { get; set; }

    public void Setup(Canvas rootCanvas, RectTransform rootRectTransform, Transform target)
    {
        ParentCanvas = rootCanvas;
        ParentRectTransform = rootRectTransform;
        CanvasCamera = ParentCanvas.worldCamera;
        SetAimTarget(target);
    }

    public void SetAimTarget(Transform target)
    {
        Target = target;
        IsInScreen = false;
    }

    public void PlayLockAnimation(bool isLock)
    {
        if (isLock)
            animator.SetTrigger("Lock");
        else
            animator.SetTrigger("UnLock");
    }

    private void LateUpdate()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (IsTargetInScreen())
        {
            if (!IsInScreen)
            {
                IsInScreen = true;
                PlayLockAnimation(true);
            }

            // 将Target的世界坐标转先转换到屏幕坐标，再将其转换到父RectTransform内的局部坐标
            var screenPoint = RectTransformUtility.WorldToScreenPoint(MainCamera, Target.position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentRectTransform, screenPoint, CanvasCamera, out var vector2))
            {
                transform.localPosition = vector2;
            }
        }
        else
        {
            if (IsInScreen)
            {
                IsInScreen = false;
                PlayLockAnimation(false);
            }
        }
    }

    // 判断锁定对象是否处于可视范围内
    private bool IsTargetInScreen()
    {
        // 转换目标的世界坐标至屏幕坐标
        var point = MainCamera.WorldToViewportPoint(Target.position);

        // 屏幕坐标的z值处于相机的进截面和远截面之间，并且处于屏幕范围内，即表示锁定对象在可视范围内
        var isInScreen = point.z > MainCamera.nearClipPlane && point.z < MainCamera.farClipPlane && ViewProtRect.Contains(point);

        return isInScreen;
    }
}
