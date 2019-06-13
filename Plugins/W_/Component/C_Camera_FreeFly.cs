/*Name:       C_Camera_Free 
 *Description:     取消控制设置的不太好
 *Author:          lwb
 *Date:            2019-06-
 *Copyright(C) 2019 by   company@zhiwyl.com*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Camera_FreeFly : MonoBehaviour
{


    public float mouseSensitivity = 3f;
    public float speedSensitivity = 10f;
    private float m_deltX = 0f;
    private float m_deltY = 0f;
    public bool lockCursor = false;
    Rigidbody cameraRig;
    SphereCollider cameraCollider;
    public Vector3 dir;
    float vertical;
    float horizontal;
    [Header("哪个键切换视角")]
    public MainButton mainButton = MainButton.左键;
    public enum MainButton {无,左键,右键,中键 }
    bool containCollider;
    bool containRig;
    bool useGravity;
    void OnEnable()
    {
        if (transform.GetComponent<Rigidbody>() == null)
        {
            containRig = false;
            transform.gameObject.AddComponent<Rigidbody>();
        }
        else
        {
            useGravity = transform.GetComponent<Rigidbody>().useGravity;
        }
        cameraRig = transform.GetComponent<Rigidbody>();
        cameraRig.useGravity = false;

        if (transform.GetComponent<Collider>() == null)
        {
            containCollider = false;
            transform.gameObject.AddComponent<SphereCollider>();
            transform.GetComponent<SphereCollider>().radius = 0.25f;
        }
    }

    private void OnDisable()
    {
        dir = Vector3.zero;
        m_deltX = 0;
        m_deltY = 0;
        vertical = 0;
        horizontal = 0;
        if (!containRig)
        {
            Destroy(transform.gameObject.GetComponent<Rigidbody>());
        }
        else
        {
            cameraRig.useGravity = useGravity;
        }
        if (!containCollider)
        {
            Destroy(transform.gameObject.GetComponent<SphereCollider>());
        }
    }

    void SetSet(int i)
    {
        if (i == -1)
        {
            LookRotation();
            ToForward();
            return;
        }
        if (Input.GetMouseButton(i))
        {
            LockCursor(lockCursor);
            LookRotation();
            ToForward();
        }
        else
            LockCursor(false);
    }
    void Update()
    {
        SetKinematic();

        LookRotation_New(transform, Camera.main.transform);
        ToForward();

        //switch (mainButton)
        //{
        //    case MainButton.无:
        //        SetSet(-1);
        //        break;
        //    case MainButton.左键:
        //        SetSet(0);
        //        break;
        //    case MainButton.右键:
        //        SetSet(1);
        //        break;
        //    case MainButton.中键:
        //        SetSet(2);
        //        break;
        //}

    }

    void SetKinematic()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            cameraRig.isKinematic = false;
        }
        else
        {
            cameraRig.isKinematic = true;
        }
    }
    void ToForward()
    {

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");


        dir = Camera.main.transform.forward * vertical;
        dir += Camera.main.transform.right * horizontal;


        //dir = transform.transform.forward * vertical;
        //dir += transform.transform.right * horizontal;

        cameraRig.velocity = dir * speedSensitivity;
    }

    private void LookRotation()
    {
        m_deltX += Input.GetAxis("Mouse X") * mouseSensitivity;
        m_deltY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        m_deltX = ClampAngle(m_deltX, -360, 360);
        m_deltY = ClampAngle(m_deltY, -70, 70);
        transform.transform.rotation = Quaternion.Euler(m_deltY, m_deltX, 0);
    }

    float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;

        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }
    void LockCursor(bool b)
    {
        //Cursor.lockState = b ? CursorLockMode.Locked : Cursor.lockState = CursorLockMode.None;
        Cursor.visible = b ? false : true;
    }





    private void Start()
    {
        Init(transform,Camera.main.transform);
    }


    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor_New = false;

    [Header("改这个")]
    public Quaternion playerRot;
    public Quaternion camRot;
    private bool m_cursorIsLocked = true;

    public void Init(Transform character, Transform camera)
    {
        playerRot = character.localRotation;
        camRot = camera.localRotation;
    }


    public void LookRotation_New(Transform character, Transform camera)
    {
        if (!Input.GetMouseButton(0))
            return;
        //float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        //float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;
        playerRot *= Quaternion.Euler(0f, yRot, 0f);
        camRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            camRot = ClampRotationAroundXAxis(camRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, playerRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, camRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = playerRot;
            camera.localRotation = camRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor_New = value;
        if (!lockCursor_New)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor_New)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }


}