using UnityEngine;
using System.Collections;

public class ScrollingTexture : MonoBehaviour {
    MeshRenderer Mesh;
    public Vector2 Speed;

	// Use this for initialization
	void Start () {
        Mesh = GetComponent<MeshRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
        Mesh.material.SetTextureOffset( "_MainTex", Speed * Time.time);
	}
}
