using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogobyte.ProceduralIndicators;

//Creates a made of shapes arrow from default point to ball position
public class TrackerArrow : MonoBehaviour {
    public ArrowObject arrowObject;
    public Transform ball;
    public Transform me;
    private MeshRenderer meshrenderer;
    private void Awake()
    {
        meshrenderer = GetComponent<MeshRenderer>();
    }
    void Start () {
	}
	
	void Update () {
        arrowObject.arrowPath.endPoint = -me.position + ball.position + new Vector3(0,1f,0);
        arrowObject.updateArrowMesh();
        if (ball.GetComponentInChildren<MeshRenderer>().material != meshrenderer.material)
            meshrenderer.material = ball.GetComponentInChildren<MeshRenderer>().material;
    }
}
