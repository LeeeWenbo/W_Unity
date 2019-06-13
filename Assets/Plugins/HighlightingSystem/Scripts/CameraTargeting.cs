/*Name:					CameraTargeting		
 *Description: 			摄像头检测，配合HightlightingSystem插件。
 *Author:       		HightlightingSystem
 *Date:         		2018-06-21
 *Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraTargeting : MonoBehaviour
{
	// Which layers targeting ray must hit (-1 = everything)
	public LayerMask targetingLayerMask = -1;
	
	// Targeting ray length
	private float targetingRayLength = Mathf.Infinity;
	
	// Camera component reference
	private Camera cam;
    private HighlightableObject formerHo;

    void Awake()
	{
		cam = GetComponent<Camera>();
	}
	
	void Update()
	{
		TargetingRaycast();
	}
	
	public void TargetingRaycast()
	{
		// Current mouse position on screen
		Vector3 mp = Input.mousePosition;
		
		// Current target object transform component
		Transform targetTransform = null;
		
		// If camera component is available
		if (cam != null)
		{
			RaycastHit hitInfo;
			
			// Create a ray from mouse coords
			Ray ray = cam.ScreenPointToRay(new Vector3(mp.x, mp.y, 0f));
			
			// Targeting raycast
			if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, targetingRayLength, targetingLayerMask.value))
			{
				// Cache what we've hit
				targetTransform = hitInfo.collider.transform;
			}
		}
		
		// If we've hit an object during raycast
		if (targetTransform != null)
		{
            // And this object has HighlightableObject component
            //HighlightableObject ho = targetTransform.root.GetComponentInChildren<HighlightableObject>();
            if (formerHo!=null && Input.GetButtonDown("Fire1"))
            {
                formerHo.FlashingOff();
	            
            }
			HighlightableObject ho = targetTransform.GetComponent<HighlightableObject>();
            
            if (ho != null)
			{
                // If left mouse button down
                if (Input.GetButtonDown("Fire1"))
                // Start flashing with frequency = 2
                { ho.FlashingOn(2f);
                    formerHo = ho;
                }
				
				// If right mouse button is up
				if (Input.GetButtonUp("Fire2"))
					// Stop flashing
					ho.FlashingOff();
				
				// If middle mouse button is up
				if (Input.GetButtonUp("Fire3"))
					// Switch flashing
					ho.FlashingSwitch();
				
				// One-frame highlighting (to highlight an object which is currently under mouse cursor)
				ho.On(Color.red);
			}
		}
	}
	
	void OnGUI()
	{
		//GUI.Label(new Rect(10, Screen.height - 100, 500, 100), "Left mouse button - turn on flashing on object under mouse cursor\nMiddle mouse button - switch flashing on object under mouse cursor\nRight mouse button - turn off flashing on object under mouse cursor\n'Tab' - fade in/out constant highlighting\n'Q' - turn on/off constant highlighting immediately\n'Z' - turn off all types of highlighting immediately");
	}
}
