using UnityEngine;
using System.Collections;

public class EmissionTranslation : MonoBehaviour 
{
    [SerializeField] private AnimationCurve colorCurve = null;
    [SerializeField] private Gradient colorGradient = null;

    [SerializeField] private float animationTime = 1;

    private float startTime;

    private MeshRenderer myMeshRenderer;

    private void Start () 
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        myMeshRenderer.material.EnableKeyword("_EMISSION");

        startTime = Time.time;
	}
	
	private void Update () 
    {
        float normalizedTime = Mathf.Clamp01((Time.time - startTime) / (animationTime));

        Color emissionColor = colorGradient.Evaluate( colorCurve.Evaluate( normalizedTime ) );

        myMeshRenderer.material.SetColor("_EmissionColor", emissionColor);

        if (normalizedTime == 1)
        {
            startTime = Time.time;
        }
	}
}
