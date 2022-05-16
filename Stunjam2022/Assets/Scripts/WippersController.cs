using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WippersController : MonoBehaviour
{
    public Transform leftWipperTransform, rightWipperTransform;
    public float bottomAngle, topAngle;
    public float minWippingDelay, maxWippingDelay;
    public float minWippingTime, maxWippingTime;

    private bool isLeftWipping, isRightWipping;

    private void Start() {
        isLeftWipping = false;
        isRightWipping = false;

        leftWipperTransform.rotation = Quaternion.identity;
        rightWipperTransform.rotation = Quaternion.identity;
    }

    private void Update() {
        if(!isLeftWipping && !isRightWipping){
            StartCoroutine(RunAfterTimer(UnityEngine.Random.Range(minWippingDelay, maxWippingDelay),() => {
                float wippingTime = UnityEngine.Random.Range(minWippingTime, maxWippingTime);
                isLeftWipping = true;
                StartCoroutine(WipeLeftToTop(wippingTime));
                StartCoroutine(RunAfterTimer(wippingTime / 10f, () => {
                    isRightWipping = true;
                    StartCoroutine(WipeRightToTop(wippingTime));
                }));
            }));
        }
    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
 
        return(NewValue);
    }

    #region COROUTINES
    IEnumerator RunAfterTimer(float timer, Action func){
        float endTime = Time.time + timer;
        while(Time.time < endTime){
            yield return null;
        }
        func.Invoke();
    }

    IEnumerator WipeLeftToTop(float wippingTime){
        float initTime = Time.time;
        float endTime = initTime + wippingTime;
        while(Time.time < endTime){
            Debug.Log($"scale: {scale(initTime, endTime, bottomAngle, topAngle, Time.time)} / rotation: {leftWipperTransform.rotation.eulerAngles.z}");
            leftWipperTransform.Rotate(new Vector3(0, 0, scale(initTime, endTime, bottomAngle, topAngle, Time.time) - leftWipperTransform.rotation.eulerAngles.z));
            yield return null;
        }
        //StartCoroutine(WipeLeftToBottom(wippingTime));
    }
    IEnumerator WipeRightToTop(float wippingTime){
        float initTime = Time.time;
        float endTime = initTime + wippingTime;
        while(Time.time < endTime){
            rightWipperTransform.Rotate(new Vector3(0, 0, -scale(initTime, endTime, bottomAngle, topAngle, Time.time) - rightWipperTransform.rotation.eulerAngles.z));
            yield return null;
        }
        //StartCoroutine(WipeRightToBottom(wippingTime));
    }

    IEnumerator WipeLeftToBottom(float wippingTime){
        float initTime = Time.time;
        float endTime = initTime + wippingTime;
        while(Time.time < endTime){
            leftWipperTransform.Rotate(new Vector3(0, 0, scale(initTime, endTime, topAngle, bottomAngle, Time.time) - leftWipperTransform.rotation.eulerAngles.z));
            yield return null;
        }
        isLeftWipping = false;
    }
    IEnumerator WipeRightToBottom(float wippingTime){
        float initTime = Time.time;
        float endTime = initTime + wippingTime;
        while(Time.time < endTime){
            rightWipperTransform.Rotate(new Vector3(0, 0, -scale(initTime, endTime, topAngle, bottomAngle, Time.time) - rightWipperTransform.rotation.eulerAngles.z));
            yield return null;
        }
        isRightWipping = false;
    }
    #endregion =======
}
