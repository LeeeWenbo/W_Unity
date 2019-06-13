using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IOCcam : MonoBehaviour {
	public string tags;
	public LayerMask layerMsk;
	public int occludeeLayer;
	public int samples;
	public float raysFov;
	public bool preCullCheck;
	public float viewDistance;
	public int hideDelay;
	public bool realtimeShadows;
	public float lod1Distance;
	public float lod2Distance;
	public float lodMargin;
	public int lightProbes;
	public float probeRadius;

	private RaycastHit hit;
	private Ray r;
	private int layerMask;
	private IOCcomp iocComp;
	private int haltonIndex;
	private float[] hx;
	private float[] hy;
	private int pixels;
	private Camera cam;
	private Camera rayCaster;

	void Awake () {
		cam = GetComponent<Camera>();
		hit = new RaycastHit();
		if(viewDistance == 0) viewDistance = 100;
		cam.farClipPlane = viewDistance;
		haltonIndex = 0;
		if(this.GetComponent<SphereCollider>() == null)
		{
			var coll = gameObject.AddComponent<SphereCollider>();
			coll.radius = 1f;
			coll.isTrigger = true;
		}
	}
	
	void Start () {
		pixels = Mathf.FloorToInt(Screen.width * Screen.height / 4f);
		hx = new float[pixels];
		hy = new float[pixels];
		for(int i=0; i < pixels; i++)
		{
			hx[i] = HaltonSequence(i, 2);
			hy[i] = HaltonSequence(i, 3);
		}
		foreach(GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if(tags.Contains(go.tag))
			{
				if(go.GetComponent<Light>() != null)
				{
					if(go.GetComponent<IOClight>() == null)
					{
						go.AddComponent<IOClight>();
					}
				}
				else if(go.GetComponent<Terrain>() != null)
				{
					go.AddComponent<IOCterrain>();
				}
				else
				{
					if(go.GetComponent<IOClod>() == null)
					{
						go.AddComponent<IOClod>();
					}
				}
			}
		}
		GameObject goRayCaster = new GameObject("RayCaster");
		goRayCaster.transform.Translate(transform.position);
		goRayCaster.transform.rotation = transform.rotation;
		rayCaster = goRayCaster.AddComponent<Camera>();
		rayCaster.enabled = false;
		rayCaster.clearFlags = CameraClearFlags.Nothing;
		rayCaster.cullingMask = 0;
		rayCaster.aspect = cam.aspect;
		rayCaster.nearClipPlane = cam.nearClipPlane;
		rayCaster.farClipPlane = cam.farClipPlane;
		rayCaster.fieldOfView = raysFov;
		goRayCaster.transform.parent = transform;
	}
	
	void Update () {
		for(int k=0; k <= samples; k++)
		{
			r = rayCaster.ViewportPointToRay(new Vector3(hx[haltonIndex], hy[haltonIndex], 0f));
			haltonIndex++;
			if(haltonIndex >= pixels) haltonIndex = 0;
			if(Physics.Raycast(r, out hit, viewDistance, layerMsk.value))
			{
				if(iocComp = hit.transform.GetComponent<IOCcomp>()) {
					iocComp.UnHide(hit);
				}
				else if(iocComp = hit.transform.parent.GetComponent<IOCcomp>()){
					iocComp.UnHide(hit);
				}
			}
		}
	}

	private float HaltonSequence(int index, int b)
	{
		float res = 0f;
		float f = 1f / b;
		int i = index;
		while(i > 0)
		{
			res = res + f * (i % b);
			i = Mathf.FloorToInt(i/b);
			f = f / b;
		}
		return res;
	}
}