using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace WaterRippleForScreens {

[RequireComponent(typeof(Camera))]
public class RippleEffect : MonoBehaviour {
    public bool detectClick = true; //true -> This script detects left mouse click / false -> Won't detect mouse click

    [Range(0, 10)]
    public int waveCount = 1; //Quantity of ripple effects supported at the same time

    public bool timeInfinity = false; //Flag: tells if ripple effect doesn't change throught time
    public float waveTime = 2.0f; //Time of the effect in seconds
    //Curve used to animate waveAmplitude
    public AnimationCurve waveAnimCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.219f, 0.529f, Mathf.Tan(Mathf.Deg2Rad * -65.5f), Mathf.Tan(Mathf.Deg2Rad * -65.5f)), new Keyframe(1, 0));

    public float waveInternalRadio = 0.4f; //Value to increase the wave over time
    public float waveExternalRadio = 0.07f; //Wave initial size
    private float[] currentInternalRadio; //Shader value of wave internal radio

    private float[] waveAmplitude; //Used to control the wave effect amplitude throughout time
    private float[] pivotTime; //Used to count how much time has passed before waveTime ends

    //Customize this for different kinds of Ripple Effects
	public float waveScale = 6.0f, waveSpeed = 0.34f, waveFrequency = 2.6f;

    public float circleXScale = 1.0f, circleYScale = 1.0f; //Circle Scale ratio / if x == 1 && y == 1 -> perfect circle, else oval

    private Vector2[] targetPosition; //All positions to do the effect

    private int lastIDUsed = 0; //Stores the ID of the last ripple effect

    //Reference to the Shader
	private Material material;

    //Used on detecting player input
    private bool playerClick;
    private Vector2 playerClickPosition;
        private PlayerController playerController;

	// Creates a private material used to the effect
	void Awake () {
        //Init all arrays
        currentInternalRadio = new float[10];
        waveAmplitude = new float[10];
        pivotTime = new float[10];
        targetPosition = new Vector2[10];

        for (int i = 0; i < targetPosition.Length; i++) {
            targetPosition[i] = new Vector2(-1.0f, -1.0f);
        }

        //Init status variables
        playerClick = false;

        lastIDUsed = -1;

        //Find the Shader
        material = new Material(Shader.Find("Hidden/RippleDiffuse"));
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    void Update() {
        if (detectClick) { //true -> Detect mouse left click / false -> Skip mouse click detection
#if UNITY_EDITOR //Detect player input on editor
            if (playerController.shootAction.IsPressed()) {
                playerClickPosition = Input.mousePosition;
                playerClick = true;
                //Debug.Log("Mouse Pos: " + Input.mousePosition);
            }

#elif UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_WP_8_1 //Detect player input for mobiles
             if (playerController.shootAction.IsPressed()) {
                playerClickPosition = Input.GetTouch(0).position;
                playerClick = true;
            }
#else                                                         //Detect player input from mouse
             if (playerController.shootAction.IsPressed()) {
                playerClickPosition = Input.mousePosition;
                playerClick = true;
                //Debug.Log("Mouse Pos: " + Input.mousePosition);
            }
#endif
                if (playerClick) { //Check if player did click
                playerClick = false; //Reset player click

                //Set new ID
                lastIDUsed = (lastIDUsed + 1 >= waveCount) ? 0 : lastIDUsed + 1;

                //Convert to (0, 1) Space & set TargetPosition
                targetPosition[lastIDUsed] = ConvertToTextureSpace(playerClickPosition);

                //Init Amplitude
                waveAmplitude[lastIDUsed] = 1.0f;

                //Init Time
                pivotTime[lastIDUsed] = Time.time;

                //Init grow
                currentInternalRadio[lastIDUsed] = 0.0f;
            }
        }

        //if (pivotTime + waveTime > Time.time) { //Check if wave effect ends (might be useful in the future)
            
        //}

        if (!timeInfinity) { //Wave has a lifetime
            for (int i = 0; i < waveCount; i++) { //Calculate all waves
                //Check if position is asigned
                if (targetPosition[i].x == -1.0f) {
                    waveAmplitude[i] = 0.0f;
                    continue;
                }

                //Calculate waveAmplitude time stamp
                if(waveTime > 0) //Check waveTime for division
                    waveAmplitude[i] = (Time.time - pivotTime[i]) / waveTime;
                else waveAmplitude[i] = 0.0f;

                if (waveAmplitude[i] > 1) waveAmplitude[i] = 1.0f;

                //Set max value
                material.SetFloat("_MaxValue" + i, waveAmplitude[i] * waveExternalRadio / 2.0f);

                //Set internal ratio
                currentInternalRadio[i] = Mathf.Clamp(waveInternalRadio * waveAmplitude[i], 0.0f, waveInternalRadio);

                //Get waveAmplitide value from animation curve
                waveAmplitude[i] = waveAnimCurve.Evaluate(Mathf.Clamp01(waveAmplitude[i]));
            }
        }
            else
            { //Wave has no lifetime -> Time Infinity == true
                for (int i = 0; i < waveCount; i++)
                { //Calculate all waves
                  //Check if position is asigned
                    if (targetPosition[i].x == -1.0f)
                    {
                        waveAmplitude[i] = 0.0f;
                        continue;
                    }

                    waveAmplitude[i] = 1.0f;
                    currentInternalRadio[i] = waveInternalRadio;
                }
            }
        }

    //Gets screen position in pixels (Ex: Input.mousePosition) and plays new ripple effect on target position
    public void SetNewRipplePosition(Vector2 _targetPos) {
        //Set new ID
        lastIDUsed = (lastIDUsed + 1 >= waveCount) ? 0 : lastIDUsed + 1;

        //Convert to (0, 1) Space & set TargetPosition
        targetPosition[lastIDUsed] = ConvertToTextureSpace(_targetPos);

        //Init Amplitude
        waveAmplitude[lastIDUsed] = 1.0f;

        //Init Time
        pivotTime[lastIDUsed] = Time.time;

        //Init grow
        currentInternalRadio[lastIDUsed] = 0.0f;
    }

    //Converts the vector to texture space
    Vector2 ConvertToTextureSpace(Vector2 value) {
#if UNITY_EDITOR //Unity editor inverts 'y' axis
        float _width = 1.0f - (Screen.width - value.x) / Screen.width; //Clamp values to (0, 1)
        float _height = 1.0f - value.y / Screen.height;
        //float _height = 1.0f - (Screen.height - value.y) / Screen.height; //Use this if 'y' axis is inverted

        return new Vector2(_width, _height); //Returns new position
#else
        float _width = 1.0f - (Screen.width - value.x) / Screen.width; //Clamp values to (0, 1)
        float _height = 1.0f - (Screen.height - value.y) / Screen.height; //Use this if 'y' axis is inverted

        return new Vector2(_width, _height); //Returns new position
#endif
    }

    public void StopAllEffects() {
        for (int i = 0; i < pivotTime.Length; i++) { //Reset all pivot times and all amplitudes
            pivotTime[i] = Time.time - waveTime;
            waveAmplitude[i] = 0.0f;
        }
    }
	
	//Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (waveSpeed == 0 || waveScale == 0 || material == null) { //Do nothing
            if (material == null) Debug.Log("Material is null");

			Graphics.Blit (source, destination);
			return;
		}

        //Pass parameters to shader
        material.SetInt("_WaveCount", waveCount);
        material.SetFloat("_Speed", waveSpeed);
        material.SetFloat("_Scale", waveScale);
        material.SetFloat("_Frequency", waveFrequency);
        material.SetFloat("_ExternalRadio", waveExternalRadio);
        material.SetFloat("_AspectRatio", (float)Screen.width / Screen.height);
        material.SetFloat("_CircleXScale", circleXScale);
        material.SetFloat("_CircleYScale", circleYScale);
        for (int i = 0; i < targetPosition.Length; i++) {
            material.SetFloat("_InternalRadio" + i, currentInternalRadio[i]);
            material.SetFloat("_TargetPosX" + i, targetPosition[i].x);
            material.SetFloat("_TargetPosY" + i, targetPosition[i].y);
            material.SetFloat("_Amplitude" + i, (i >= waveCount || lastIDUsed == -1) ? 0.0f : waveAmplitude[i]);
        }

        //RenderTexture tempTexture = RenderTexture.GetTemporary(Screen.width, Screen.height);
        //tempTexture.filterMode = FilterMode.Bilinear;

        //Graphics.Blit(source, tempTexture, material);
        //Graphics.Blit(tempTexture, destination);
        //RenderTexture.ReleaseTemporary(tempTexture);

        Graphics.Blit(source, destination, material);
	}
}

}
