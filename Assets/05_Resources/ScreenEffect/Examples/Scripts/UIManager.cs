using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace WaterRippleForScreens {

public class UIManager : MonoBehaviour {
    public static UIManager instance; //Singleton

    public GameObject targetPositionItemPf; //Position list prefab

    //UI variables
    public Toggle detectClickToggle;
    public Toggle timeInfinityToggle;
    public InputField waveTimeInputField;
    public InputField internalRadioInputField;
    public InputField externalRadioInputField;
    public InputField scaleInputField;
    public InputField speedInputField;
    public InputField frequencyInputField;
    public InputField circleXScaleInputField;
    public InputField circleYScaleInputField;

    //Wave Count variables
    public Text waveCountText;
    public Slider waveCountSlider;

    //UI Generator variables
    public Toggle randomGenerationToggle;
    public InputField timeNextRippleInputField;
    public InputField timeOffsetInputField;
    public InputField positionXInputField, positionYInputField;
    private float positionX, positionY;

    //Position list variables
    public Transform positionListParent;
    private RectTransform positionListParentRT;
    private float positionListYSize = 40.0f;
    private float positionListYOffset = 20.0f;
    private List<GameObject> positionList;

    //Camera Effects scripts
    public RippleEffect rippleCameraEffect;
    public RippleGenerator rippleGenerator;

    void Awake() {
        if (instance == null) { //Singleton
            instance = this;
        }
        else if (instance != this) {
            Destroy(this.gameObject);
        }

        positionList = new List<GameObject>(); //Init list

        positionListParentRT = positionListParent.GetComponent<RectTransform>(); //Get rect transform reference
    }

    void Start() {
        //Init all ui elements
        detectClickToggle.isOn = rippleCameraEffect.detectClick;
        timeInfinityToggle.isOn = rippleCameraEffect.timeInfinity;
        waveTimeInputField.text = rippleCameraEffect.waveTime.ToString("f5");
        internalRadioInputField.text = rippleCameraEffect.waveInternalRadio.ToString("f5");
        externalRadioInputField.text = rippleCameraEffect.waveExternalRadio.ToString("f5");
        scaleInputField.text = rippleCameraEffect.waveScale.ToString("f5");
        speedInputField.text = rippleCameraEffect.waveSpeed.ToString("f5");
        frequencyInputField.text = rippleCameraEffect.waveFrequency.ToString("f5");
        circleXScaleInputField.text = rippleCameraEffect.circleXScale.ToString("f5");
        circleYScaleInputField.text = rippleCameraEffect.circleYScale.ToString("f5");

        randomGenerationToggle.isOn = rippleGenerator.randomGeneration;
        timeNextRippleInputField.text = rippleGenerator.timeBetweenRippleMedian.ToString("f5");
        timeOffsetInputField.text = rippleGenerator.timeBetweenRippleDesv.ToString("f5");
        positionXInputField.text = ((int)(Screen.width / 2.0f)).ToString("f5");
        positionYInputField.text = ((int)(Screen.height / 2.0f)).ToString("f5");

        waveCountText.text = "Wave Count: " + waveCountSlider.value.ToString("f5");
    }

    public void OnValueChangeDetectClick() {
        rippleCameraEffect.detectClick = detectClickToggle.isOn;
    }

    public void OnValueChangeTimeInfinity() {
        rippleCameraEffect.timeInfinity = timeInfinityToggle.isOn;
    }

    public void OnEndEditWaveTime() {
        rippleCameraEffect.waveTime = float.Parse((waveTimeInputField.text != "" && waveTimeInputField.text != "." && waveTimeInputField.text != "-") ? waveTimeInputField.text : "0.0");
    }

    public void OnEndEditInternalRadio() {
        rippleCameraEffect.waveInternalRadio = float.Parse((internalRadioInputField.text != "" && internalRadioInputField.text != "." && internalRadioInputField.text != "-") ? internalRadioInputField.text : "0.0");
    }

    public void OnEndEditExternalRadio() {
        rippleCameraEffect.waveExternalRadio = float.Parse((externalRadioInputField.text != "" && externalRadioInputField.text != "." && externalRadioInputField.text != "-") ? externalRadioInputField.text : "0.0");
    }

    public void OnEndEditScale() {
        rippleCameraEffect.waveScale = float.Parse((scaleInputField.text != "" && scaleInputField.text != "." && scaleInputField.text != "-") ? scaleInputField.text : "0.0");
    }

    public void OnEndEditSpeed() {
        rippleCameraEffect.waveSpeed = float.Parse((speedInputField.text != "" && speedInputField.text != "." && speedInputField.text != "-") ? speedInputField.text : "0.0");
    }

    public void OnEndEditFrequency() {
        rippleCameraEffect.waveFrequency = float.Parse((frequencyInputField.text != "" && frequencyInputField.text != "." && frequencyInputField.text != "-") ? frequencyInputField.text : "0.0");
    }

    public void OnEndEditCircleXScale() {
        rippleCameraEffect.circleXScale = float.Parse((circleXScaleInputField.text != "" && circleXScaleInputField.text != "." && circleXScaleInputField.text != "-") ? circleXScaleInputField.text : "0.0");
    }

    public void OnEndEditCircleYScale() {
        rippleCameraEffect.circleYScale = float.Parse((circleYScaleInputField.text != "" && circleYScaleInputField.text != "." && circleYScaleInputField.text != "-") ? circleYScaleInputField.text : "0.0");   
    }

    public void OnValueChangeWaveCount() {
        waveCountText.text = "Wave Count: " + waveCountSlider.value.ToString("f0");
        rippleCameraEffect.waveCount = (int) waveCountSlider.value;
    }

    public void OnValueChangeRandomGeneration() {
        rippleGenerator.randomGeneration = randomGenerationToggle.isOn;
    }

    public void OnEndEditTimeNextRipple() {
        rippleGenerator.timeBetweenRippleMedian = float.Parse((timeNextRippleInputField.text != "" && timeNextRippleInputField.text != "." && timeNextRippleInputField.text != "-") ? timeNextRippleInputField.text : "0.0");
    }

    public void OnEndEditTimeOffset() {
        rippleGenerator.timeBetweenRippleDesv = float.Parse((timeOffsetInputField.text != "" && timeOffsetInputField.text != "." && timeOffsetInputField.text != "-") ? timeOffsetInputField.text : "0.0");
    }

    public void OnEndEditPositionX() {
        positionX = float.Parse((positionXInputField.text != "" && positionXInputField.text != "." && positionXInputField.text != "-") ? positionXInputField.text : "0.0");
    }
    
    public void OnEndEditPositionY() {
        positionY = float.Parse((positionYInputField.text != "" && positionYInputField.text != "." && positionYInputField.text != "-") ? positionYInputField.text : "0.0");
    }

    public void OnClickAddNewTargetPosition() {
        GameObject _newListItem = Instantiate(targetPositionItemPf, positionListParent) as GameObject; //Init new item list

        _newListItem.transform.localPosition = new Vector3(82.0f, -positionListYOffset, 0.0f); //Set local position
        positionListYOffset += positionListYSize; //Update offset

        PanelPositionItem _panelPosItem = _newListItem.GetComponent<PanelPositionItem>(); //Get script reference
        _panelPosItem.ID = positionList.Count; //Set ID
        _panelPosItem.AddOnClickListener(); //Add on click delete button listener

        positionList.Add(_newListItem); //Add to list

        _panelPosItem.SetPositionText((int)positionX, (int)positionY); //Set position for text UI

        rippleGenerator.AddTargetPosition(new Vector2(positionX, positionY)); //Add position to rippleGenerator list

        //Check if content parent must increase y size
        if (positionListYSize * positionList.Count > positionListParentRT.sizeDelta.y) {
            float _sizeIncrease = positionListYSize * positionList.Count - positionListParentRT.sizeDelta.y;

            //Apply increase
            positionListParentRT.sizeDelta = new Vector2(positionListParentRT.sizeDelta.x, positionListYSize * positionList.Count);

            //Move position list elements
            for (int i = 0; i < positionList.Count; i++) {
                positionList[i].transform.localPosition = new Vector3(82.0f, positionList[i].transform.localPosition.y + _sizeIncrease / 2.0f, 0.0f);
            }
        }
    }

    public void OnClickDeleteTargetPosition(int id) {
        PanelPositionItem _panelPosItem;
        bool reducedParentSize = false;

        Destroy(positionList[id]); //Remove from UI
        positionList.RemoveAt(id); //Remove from list

        rippleGenerator.RemoveTargetPosition(id); //Remove selected element from rippleGenerator list

        //Reduce size of parent if parent Y size > 300
        if(positionListParentRT.sizeDelta.y > 300) {
            positionListParentRT.sizeDelta = new Vector2(positionListParentRT.sizeDelta.x, positionListYSize * positionList.Count);
            reducedParentSize = true;
        }

        //Move position list elements and update ids
        for (int i = 0; i < positionList.Count; i++) {
            positionList[i].transform.localPosition = 
                new Vector3(82.0f, positionList[i].transform.localPosition.y + //Current position
                                   positionListYSize * ((id <= i) ? 1.0f : 0.0f) + //If position is after deleted one
                                   ((reducedParentSize) ? -positionListYSize / 2.0f : 0.0f), //If parent size has been reduced 
                            0.0f);

            if(id <= i) {
                _panelPosItem = positionList[i].GetComponent<PanelPositionItem>();
                _panelPosItem.ID = i;
                _panelPosItem.UpdateOnClickListenerID();
            }
        }
        positionListYOffset -= positionListYSize; //Update offset
    }
}

}
