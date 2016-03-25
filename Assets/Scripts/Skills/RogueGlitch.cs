using UnityEngine;
using System.Collections;

public class RogueGlitch : ASkill
{
    // Camera References
    private GameObject mainCameraObject = null;
    private Camera mainCamera = null;
    private Transform mainCameraTransform = null;

    // Frame Position Of The Camera
    private Vector3 framePosition = Vector3.zero;

    // TeleportLimits
    private Vector3 lowerLimit = Vector3.zero;
    private Vector3 upperLimit = Vector3.zero;

    // Percentile Of Screen Dimensions
    private float minLengthPercentile = 0;
    private float maxLengthPercentile = 0;

    // Textures To Get System Working
    private RenderTexture worldViewTexture = null;
    private RenderTexture glitchGetTexture = null;
    private Texture2D texture = null;
    private Texture2D finalFrameTexture = null;

    // Glitch Interval
    private float glitchInterval = 0.25f;

    // Control Variable
    private bool isGlitching = false;

    private Vector3 initialPosition;

    private Vector3 targetPosition;

    public void Initialize(GameObject mainCameraObject, Vector3 minPosition, Vector3 maxPosition, float duration, float interval, float cooldown, float minLengthPercentile, float maxLengthPercentile)
	{
        this.mainCameraObject = mainCameraObject;

        this.lowerLimit = minPosition;
        this.upperLimit = maxPosition;
        
        this.duration = duration;
		this.cooldown = cooldown;

        this.minLengthPercentile = minLengthPercentile;
        this.maxLengthPercentile = maxLengthPercentile;

        glitchInterval = interval;

        mainCamera = mainCameraObject.GetComponent<Camera>();
        mainCameraTransform = mainCameraObject.GetComponent<Transform>();

        worldViewTexture = new RenderTexture(Screen.width, Screen.height, (int)mainCamera.farClipPlane);
        glitchGetTexture = new RenderTexture(Screen.width, Screen.height, (int)mainCamera.farClipPlane);

        texture = new Texture2D(Screen.width, Screen.height);
        finalFrameTexture = new Texture2D(Screen.width, Screen.height);

        initialPosition = mainCameraTransform.position;
	}

	public override bool UseSkill()
	{
		StartCoroutine( SkillLogic(duration) );
		StartCoroutine( SkillCooldown() );
        return true;
	}
	
	public IEnumerator SkillLogic(float time)
	{
        isGlitching = true;
        StartCoroutine(RandomizePosition(duration, 0.1f));
        yield return new WaitForSeconds(duration);
        isGlitching = false;
        mainCameraTransform.position = initialPosition;
    }

    private IEnumerator RandomizePosition(float duration, float interval)
    {
        while(duration > 0)
        {
            targetPosition = new Vector3(Random.Range(lowerLimit.x, upperLimit.x), Random.Range(lowerLimit.y, upperLimit.y), Random.Range(lowerLimit.z, upperLimit.z));
            duration -= interval;
            yield return new WaitForSeconds(interval);
        }
    }

    private void Update()
    {
        if (isGlitching)
        {
            //System.Random r = new Random();
            mainCameraTransform.position = Vector3.Lerp(mainCameraTransform.position, targetPosition, Time.deltaTime * 50);
        }
    }

    //private void InitializeTexture(Color color, Texture2D texture)
    //{
    //    for (int i = 0; i < texture.width; i++)
    //    {
    //        for (int j = 0; j < texture.height; j++)
    //        {
    //            texture.SetPixel(i, j, color);
    //        }
    //    }
    //}
    //private IEnumerator GlitchSimulator(float finishTime, float intervalTime)
    //{
    //    isGlitching = true;
    //    while (Time.time < finishTime)
    //    {
    //        framePosition = mainCameraTransform.position;

    //        // Generate World And Glitched RenderViews
    //        mainCamera.targetTexture = worldViewTexture;
    //        mainCamera.Render();
    //        mainCamera.targetTexture = null;

    //        mainCameraTransform.position = new Vector3(Random.Range(lowerLimit.x, upperLimit.x), Random.Range(lowerLimit.z, upperLimit.z), Random.Range(lowerLimit.z, upperLimit.z));

    //        mainCamera.targetTexture = glitchGetTexture;
    //        mainCamera.Render();
    //        mainCamera.targetTexture = null;

    //        // Glitch Onto Texture
    //        int x = -Screen.width / 2 + Random.Range(0, Screen.width);
    //        int y = -Screen.height / 2 + Random.Range(0, Screen.height);

    //        int lengthX = Random.Range((int)(Screen.width * minLengthPercentile), (int)(Screen.width * maxLengthPercentile));
    //        int lengthY = Random.Range((int)(Screen.height * minLengthPercentile), (int)(Screen.height * maxLengthPercentile));

    //        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
    //        RenderTexture.active = glitchGetTexture;
    //        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    //        renderedTexture.Apply();

    //        CopyPixelsToTexture(renderedTexture, texture, x, y, lengthX, lengthY);

    //        // Glitch To Main Camera Render Texture
    //        renderedTexture = new Texture2D(Screen.width, Screen.height);
    //        RenderTexture.active = worldViewTexture;
    //        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    //        renderedTexture.Apply();

    //        finalFrameTexture = OverwriteGlitchTexture(texture, renderedTexture);
    //        finalFrameTexture.Apply();

    //        mainCameraTransform.position = framePosition;

    //        yield return new WaitForSeconds(intervalTime);
    //    }
    //    isGlitching = false;
    //}

    //void OnGUI()
    //{
    //    if (isGlitching)
    //    {
    //        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), finalFrameTexture);
    //    }
    //}

    //private Texture2D CopyPixelsToTexture(Texture2D copyFrom, Texture2D copyTo, int startPixelX, int startPixelY, int blockLengthX, int blockLengthY)
    //{
    //    for (int x = startPixelX; x < (startPixelX + blockLengthX); x++)
    //    {
    //        if (x > copyTo.width || x > copyFrom.width)
    //        {
    //            break;
    //        }

    //        for (int y = startPixelY; y < (startPixelY + blockLengthY); y++)
    //        {
    //            if (y > copyTo.height || y > copyFrom.height)
    //            {
    //                break;
    //            }

    //            copyTo.SetPixel(x, y, copyFrom.GetPixel(x, y));
    //        }
    //    }

    //    return copyTo;
    //}
    //private Texture2D OverwriteGlitchTexture(Texture2D copyFrom, Texture2D copyTo)
    //{
    //    Texture2D returnValue = new Texture2D(Screen.width, Screen.height);
    //    for (int x = 0; x < copyFrom.width; x++)
    //    {
    //        for (int y = 0; y < copyFrom.height; y++)
    //        {
    //            if (copyFrom.GetPixel(x, y).a == 0)
    //            {
    //                returnValue.SetPixel(x, y, copyTo.GetPixel(x, y));
    //            }
    //            else
    //            {
    //                Color color = copyFrom.GetPixel(x, y);
    //                color.a = 1;
    //                returnValue.SetPixel(x, y, color);
    //            }
    //        }
    //    }
    //    return returnValue;
    //}
}