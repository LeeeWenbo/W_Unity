/*EquipmentSignId:		C_Camera_BaseDisplay		
 *Description: 			三维展示常用摄像头
 *Author:       		李文博
 *Motto:				
 *Date:         		2019-05-29
 *Copyright(C) 2017 by 	智网易联*/

#region Namespaces

using UnityEngine;
using DG.Tweening;
using System;
using W_Enum;

#endregion
public class C_Camera_BaseDisplay : MonoBehaviour
{
    [Header("目标物体")]
    public Transform target;
    [Header("切换视角的按键")]
    public MainButton mainButton = MainButton.左键;
    [Header("键盘控制镜头")]
    public bool isKeyboardContorl = false;

    [Header("开始角度")]
    public float startAngle = 270;

    [HideInInspector]
    //摄像头离目标当前距离
    public float curDistance = 0f;

    [Header("离目标最小最大距离")]
    public float DistanceMin = 0.5f;
    public float DistanceMax = 1f;
    [Header("二级拉近，这个是改变了摄像机的视野范围，不会出现穿透的bug")]
    public bool isAllowChangeCameraFieldOfView = true;
    //视角改变的极值
    float cameraNearEST = 20;
    private float cameraFieldInital;
    float startDistance = 0.0f;

    float desiredDistance = 0.0f;

    [Header("视角旋转速度")]
    public float X_MouseSensitivity = 5.0f;
    public float Y_MouseSensitivity = 5.0f;
    float mouseX = 0.0f;
    float mouseY = 0.0f;
    [Header("视角缩放速度")]
    public float MouseWheelSensitivity = 1f;
    float yWheelValue;
    float xWheelValue;

    [Header("按键缩放速度")] public float KeyControlZoomSpeed = 0.01f;
    [Header("按键旋转速度")] public float KeyControlRotationSpeed = 2f;

    [Header("俯角最小和最大")]
    public float Y_MinLimit = -10f; //可以设置从下向上看
    public float Y_MaxLimit = 70.0f; //可以设置从上往下看,设置89,不要吵过90,超过就有问题
    [Header("缓冲特效距离")]
    public float DistanceSmooth = 0.025f;
    float velocityDistance = 0.0f;
    Vector3 desiredPosition = Vector3.zero;
    [Header("切换视角延缓效果")]
    public float X_Smooth = 0.05f;
    public float Y_Smooth = 0.1f;
    [Space(20)]
    [Header("摄像头重置")]
    public bool RESET;


    [Space(20)]
    [Header("是否显示3D展示鼠标操作指示")]
    public bool isShowIndicator = true;
    [Header("请拖入modelIndicator，这个是之前做过的那种")]
    public Transform modelIndicator;
    bool isShowDragIndicator;
    bool isAngleIndicator;
    GameObject modelDragIndicator;
    GameObject modelAngleIndicator;
    GameObject modelZoomIndicator;

    private byte leftKeyOrRightKey = 0;
    float velX = 0.0f;
    float velY = 0.0f;
    float velZ = 0.0f;
    Vector3 position = Vector3.zero;
    Vector3 coreInitialPosion;
    bool boolZoomEnd;

    /// 0-正常伸缩  1-伸到最大，开始调焦距    2-调到最大焦距    3-回收焦距到正常(其实也就是0)
    byte cameraFieldSTATE = 0;

    float zoomTimer = 0.5f;




    [Space(20)]
    [Header("是否开启水平拖拽,这个写的不太好")]
    public bool isXDrag = false;
    [Tooltip("注意：摄像头跟随中心点，中心点越靠右，显示的物体越靠左")]
    public float xCoreLeftEST = -0.1f;
    public float xCoreRightEST = 0.1f;
    public float xDragSpeed = 0.003f;
    [Header("是否开启垂直拖拽")]
    public bool isYDrag = false;
    [Tooltip("注意：摄像头跟随中心点，中心点越靠下，显示的物体就越靠上")]
    public float yCoreLowEST = 0.05f;
    public float yCoreHeightEST = 0.1f;
    public float yDragSpeed = 0.003f;


    void Awake()
    {
        //        this.transform.localEulerAngles = new Vector3(5, 270, 0);
        coreInitialPosion = target.position;
        InitIndicator();
    }

    /// <summary>
    /// 开始
    /// </summary>
    void Start()
    {
        //Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
        curDistance = Vector3.Distance(target.transform.position, gameObject.transform.position);
        if (curDistance > DistanceMax)
            DistanceMax = curDistance;
        startDistance = curDistance;
        coreInitialPosion = target.position;
        cameraFieldInital = transform.GetComponent<Camera>().fieldOfView;
        InitCamera();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void LateUpdate()
    {
        //判断是否有目标
        if (target == null)
            return;
        //判断是否在UI上
        if (IsOnUI())
            return;
        //鼠标控制左右键切换判断
        RightOrLeft();
        //滑轮拖拽
        WheelDrag();
        HandlePlayerInput();
        CalculateDesiredPosition();
        UpdatePosition();
        ResetCamera();
        if (!isKeyboardContorl)
            return;
        KeyControlCameraFarAndNear();
        KeyControlCameraRotation();
    }

    /// <summary>
    /// 按键控制旋转效果
    /// </summary>
    private void KeyControlCameraRotation()
    {
        if (Input.GetKey(KeyCode.S))
        {
            mouseY += KeyControlRotationSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            mouseY -= KeyControlRotationSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            mouseX += KeyControlRotationSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            mouseX -= KeyControlRotationSpeed;
        }
    }

    /// <summary>
    /// 按键控制拉远拉近效果
    /// </summary>
    private void KeyControlCameraFarAndNear()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            desiredDistance = Mathf.Clamp(curDistance - KeyControlZoomSpeed, DistanceMin, DistanceMax);
            KeyControlZoomSpeed += Time.deltaTime * 2;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            desiredDistance = Mathf.Clamp(curDistance + KeyControlZoomSpeed, DistanceMin, DistanceMax);
            KeyControlZoomSpeed += Time.deltaTime * 2;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            KeyControlZoomSpeed = 0.01f;
        }
    }

    /// <summary>
    /// 摄像头重置
    /// </summary>
    public void ResetCamera()
    {
        if (RESET)
        {
            target.position = coreInitialPosion;
            InitCamera();
            RESET = false;
            cameraFieldSTATE = 0;
            transform.GetComponent<Camera>().fieldOfView = cameraFieldInital;
        }
    }

    protected virtual bool IsOnUI()
    {
        return false;
    }
    /// <summary>
    /// 滚轮拖拽
    /// </summary>
    void WheelDrag()
    {
        DragEND();
        AngleEND();

        //这个写的不太好，影响拆分

        if (!Input.GetMouseButton(2))
            return;
        if (isXDrag)
        {
            WheelXDrag();
        }

        if (isYDrag)
        {
            WheelYDrag();
        }
    }

    /// <summary>
    /// 水平滚轮拖拽
    /// </summary>
    void WheelXDrag()
    {
        xWheelValue = Input.GetAxis("Mouse X");
        if (target.position.x < xCoreLeftEST)
        {
            target.position = new Vector3(xCoreLeftEST, target.position.y, target.position.z);
            return;
        }

        else if (target.position.x > xCoreRightEST)
        {
            target.position = new Vector3(xCoreRightEST, target.position.y, target.position.z);
            return;
        }

        DragINGIndicator();
        target.position += new Vector3(-xWheelValue * xDragSpeed, 0, 0);
        xWheelValue = 0;
    }

    /// <summary>
    ///  竖直滚轮拖拽
    /// </summary>
    void WheelYDrag()
    {
        yWheelValue = -Input.GetAxis("Mouse Y");
        if (target.position.y < yCoreLowEST)
        {
            target.position = new Vector3(target.position.x, yCoreLowEST, target.position.z);
            return;
        }
        else if (target.position.y > yCoreHeightEST)
        {
            target.position = new Vector3(target.position.x, yCoreHeightEST, target.position.z);
            return;
        }

        DragINGIndicator();
        target.position += new Vector3(0, yWheelValue * yDragSpeed, 0);
        yWheelValue = 0;
    }

    /// <summary>
    /// 正在拖拽指示
    /// </summary>
    void DragINGIndicator()
    {
        if (!isShowDragIndicator || (!isXDrag && !isYDrag) || modelDragIndicator == null)
            return;

        modelDragIndicator.SetActive(true);

        if (yWheelValue < -0.3f)
        {
            modelDragIndicator.transform.GetChild(1).gameObject.SetActive(true);
            modelDragIndicator.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (yWheelValue > 0.3f)
        {
            modelDragIndicator.transform.GetChild(2).gameObject.SetActive(true);
            modelDragIndicator.transform.GetChild(1).gameObject.SetActive(false);
        }

        if (xWheelValue < -0.3f)
        {
            modelDragIndicator.transform.GetChild(3).gameObject.SetActive(true);
            modelDragIndicator.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (xWheelValue > 0.3f)
        {
            modelDragIndicator.transform.GetChild(4).gameObject.SetActive(true);
            modelDragIndicator.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 拖拽结束
    /// </summary>
    void DragEND()
    {
        if (!isShowDragIndicator || (!isXDrag && !isYDrag) || modelDragIndicator == null)
            return;

        if (Input.GetMouseButtonUp(2))
        {
            modelDragIndicator.transform.GetChild(1).gameObject.SetActive(false);
            modelDragIndicator.transform.GetChild(2).gameObject.SetActive(false);
            modelDragIndicator.transform.GetChild(3).gameObject.SetActive(false);
            modelDragIndicator.transform.GetChild(4).gameObject.SetActive(false);
            modelDragIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// 正在切换视角指示
    /// </summary>
    void AngleINGIndicator()
    {
        if (!isAngleIndicator || modelAngleIndicator == null)
            return;
        modelAngleIndicator.SetActive(true);
        if (Input.GetAxis("Mouse Y") > 0.3f)
        {
            modelAngleIndicator.transform.GetChild(1).gameObject.SetActive(true);
            modelAngleIndicator.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (Input.GetAxis("Mouse Y") < -0.3f)
        {
            modelAngleIndicator.transform.GetChild(2).gameObject.SetActive(true);
            modelAngleIndicator.transform.GetChild(1).gameObject.SetActive(false);
        }

        if (Input.GetAxis("Mouse X") < -0.3f)
        {
            modelAngleIndicator.transform.GetChild(4).gameObject.SetActive(false);
            modelAngleIndicator.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (Input.GetAxis("Mouse X") > 0.3f)
        {
            modelAngleIndicator.transform.GetChild(3).gameObject.SetActive(false);
            modelAngleIndicator.transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 切换视角结束
    /// </summary>
    void AngleEND()
    {
        if (!isAngleIndicator || modelAngleIndicator == null)
            return;
        if (Input.GetMouseButtonUp(leftKeyOrRightKey))
        {
            modelAngleIndicator.transform.GetChild(1).gameObject.SetActive(false);
            modelAngleIndicator.transform.GetChild(2).gameObject.SetActive(false);
            modelAngleIndicator.transform.GetChild(3).gameObject.SetActive(false);
            modelAngleIndicator.transform.GetChild(4).gameObject.SetActive(false);
            modelAngleIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// 缩放指示
    /// </summary>
    void ZoomINGIndicator()
    {
        if (modelZoomIndicator == null)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            boolZoomEnd = false;
            modelZoomIndicator.SetActive(true);
            modelZoomIndicator.transform.GetChild(1).gameObject.SetActive(false);
            modelZoomIndicator.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            boolZoomEnd = false;
            modelZoomIndicator.SetActive(true);
            modelZoomIndicator.transform.GetChild(2).gameObject.SetActive(false);
            modelZoomIndicator.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            boolZoomEnd = true;
        }
    }

    /// <summary>
    ///  缩放结束
    /// </summary>
    void ZoomEND()
    {
        if (!boolZoomEnd)
            return;
        zoomTimer -= Time.deltaTime;
        if (zoomTimer <= 0)
        {
            zoomTimer = 0.5f;
            modelZoomIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// 初始，判断是否使用手势提示
    /// </summary>
    void InitIndicator()
    {
        if (isShowIndicator && modelIndicator != null)
        {
            isShowDragIndicator = true;
            isAngleIndicator = true;
            modelAngleIndicator = modelIndicator.GetChild(0).GetChild(0).gameObject;
            modelZoomIndicator = modelIndicator.GetChild(0).GetChild(1).gameObject;
            modelDragIndicator = modelIndicator.GetChild(0).GetChild(2).gameObject;
        }
        else
        {
            isShowDragIndicator = false;
            isAngleIndicator = false;
        }
    }

    /// <summary>
    /// 左右键切换
    /// </summary>
    void RightOrLeft()
    {
        if (mainButton == MainButton.左键)
            leftKeyOrRightKey = 0;
        else if (mainButton == MainButton.右键)
            leftKeyOrRightKey = 1;
        else
            leftKeyOrRightKey = 2;
    }

    /// <summary>
    /// 输入控制方法
    /// </summary>
    void HandlePlayerInput()
    {
        // mousewheel deadZone
        float deadZone = 0.01f;
        if (Input.GetMouseButton(leftKeyOrRightKey))
        {
            mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * Y_MouseSensitivity;
            AngleINGIndicator();
        }

        // this is where the mouseY is limited - Helper script
        mouseY = ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);
        ZoomINGIndicator();
        ZoomEND();
        // get Mouse Wheel Input
        if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            if (cameraFieldSTATE == 0)
            {
                desiredDistance = Mathf.Clamp(curDistance - (Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity),
                    DistanceMin, DistanceMax);
            }
        }

        if (isAllowChangeCameraFieldOfView && desiredDistance == DistanceMin)
        {
            cameraFieldSTATE = 1;
            CameraZoomByFieldOfView();
        }
    }

    /// <summary>
    /// 二级放大
    /// </summary>
    void CameraZoomByFieldOfView()
    {
        if (transform.GetComponent<Camera>().fieldOfView >= cameraFieldInital)
        {
            cameraFieldSTATE = 0;
        }
        else if (transform.GetComponent<Camera>().fieldOfView <= cameraNearEST)
        {
            cameraFieldSTATE = 2;
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                return;
        }

        transform.GetComponent<Camera>().fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity * 50;
    }

    /// <summary>
    /// 计算位置方法
    /// </summary>
    void CalculateDesiredPosition()
    {
        // Evaluate distance
        curDistance = Mathf.SmoothDamp(curDistance, desiredDistance, ref velocityDistance, DistanceSmooth);

        // Calculate desired position -> Note : mouse inputs reversed to align to WorldSpace Axis
        desiredPosition = CalculatePosition(mouseY, mouseX, curDistance);
    }

    /// <summary>
    /// 计算位置
    /// </summary>
    /// <param name="rotationX"></param>
    /// <param name="rotationY"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY + startAngle, 0);
        return target.position + (rotation * direction);
    }

    /// <summary>
    /// //更新位置
    /// </summary>
    void UpdatePosition()
    {
        float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, X_Smooth);
        float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, Y_Smooth);
        float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, X_Smooth);
        position = new Vector3(posX, posY, posZ);
        transform.position = position;
        transform.LookAt(target);
    }

    /// <summary>
    /// 摄像头初始化
    /// </summary>
    void InitCamera()
    {
        mouseX = 0;
        mouseY = 0;
        curDistance = startDistance;
        desiredDistance = curDistance;
    }

    /// <summary>
    /// 角度计算方法
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360.0f || angle > 360.0f)
        {
            if (angle < -360.0f)
                angle += 360.0f;
            if (angle > 360.0f)
                angle -= 360.0f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// 检测3D物体 未用
    /// </summary>
    void Ray3DDetect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            Debug.Log(hit.collider.name);
        }
    }

    /// <summary>
    /// 配置相机旋转
    /// </summary>
    /// <param name="obj"></param>
    public void MainCameraRotation(GameObject obj)
    {
        target.DOMove(obj.transform.position, 1);
    }

    /// <summary>
    /// 配置相机旋转
    /// </summary>
    /// <param name="trans"></param>
    public void MainCameraRotation(Transform trans)
    {
        target.DOMove(trans.position, 0.8f);
    }

    /// <summary>
    /// 配置相机旋转
    /// </summary>
    /// <param name="vector3"></param>
    public void MainCameraRotation(Vector3 vector3)
    {
        target.DOLocalMove(vector3, 1);
    }
}