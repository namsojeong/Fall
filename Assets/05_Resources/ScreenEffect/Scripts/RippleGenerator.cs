using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WaterRippleForScreens {

[RequireComponent(typeof(RippleEffect))]
public class RippleGenerator : MonoBehaviour {
    public bool randomGeneration = true; //true -> ripples are generated randomly / false -> user sets target ripple positions
    
    public float timeBetweenRippleMedian = 1.0f; //Average time to spawn next ripple effect in seconds
    public float timeBetweenRippleDesv = 0.3f; //Offset time to add randomness to timeBetweenRippleMedian (Note: must be positive)

    public List<Vector2> targetPosition; //Positions for all ripple effects
    private float pivotTime; //Used to count how much time has passed since a ripple effect started

    private int currentID = 0; //Current position on control arrays

    private RippleEffect rippleCameraEffect; //Internal reference to main script

    void Awake() {
        rippleCameraEffect = GetComponent<RippleEffect>(); //Get component reference

        if(targetPosition == null) targetPosition = new List<Vector2>();
    }

	// Use this for initialization
	void Start () {
	    //Init time status
        pivotTime = Time.time + Random.Range(-timeBetweenRippleDesv, timeBetweenRippleDesv);
	}
	
	// Update is called once per frame
	//void Update () {
 //       if(randomGeneration) { //Automatically generated ripple effects
 //           if (pivotTime + timeBetweenRippleMedian < Time.time) {
 //               pivotTime = Time.time + Random.Range(-timeBetweenRippleDesv, timeBetweenRippleDesv); ;

 //               //Set new ripple effect
 //               rippleCameraEffect.SetNewRipplePosition(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
 //           }
 //       }
 //       else { //User provided coordinates
 //           if (pivotTime + timeBetweenRippleMedian < Time.time) {
 //               pivotTime = Time.time + Random.Range(-timeBetweenRippleDesv, timeBetweenRippleDesv); ;

 //               //In case array has not been set on inspector
 //               if (targetPosition.Count <= 0) return;
                
 //               //Set new ripple effect
 //               //rippleCameraEffect.SetNewRipplePosition(targetPosition[(currentID < targetPosition.Count) ? currentID++ : targetPosition.Count -1]);
 //               //currentID = (currentID > targetPosition.Count - 1) ? 0 : currentID; //Clamp currentID
 //           }
 //       }
	//}

    //Adds new position to the position list array
    public void AddTargetPosition(Vector2 _targetPos) {
        targetPosition.Add(_targetPos); //Add new position to list
    }

    //Replaces element on '_index' position with '_targetPos'
    public void ReplaceTargetPosition(int _index, Vector2 _targetPos) {
        if (_index < targetPosition.Count && _index > -1) { //Check if index is inside array
            targetPosition[_index] = _targetPos; //Replace element
        }
        else { //Error
            Debug.Log("Index is out of bounds");
        }
    }

    //Removes element of targetPosition list on '_index'
    public void RemoveTargetPosition(int _index) {
        if (_index < targetPosition.Count && _index > -1) { //Check if index is inside array
            targetPosition.RemoveAt(_index); //Remove element
        }
        else { //Error
            Debug.Log("Index is out of bounds");
        }
    }

    //Searchs for _targetPos on targetPosition list and deletes that element
    public void RemoveTargetPosition(Vector2 _targetPos) {
        for (int i = 0; i < targetPosition.Count; i++) { //Search for _targetPos
            if (targetPosition[i] == _targetPos) {
                targetPosition.RemoveAt(i); return;
            }
        }

        //Position not found
        Debug.Log("Position not found - No element was deleted");
    }
}

}
