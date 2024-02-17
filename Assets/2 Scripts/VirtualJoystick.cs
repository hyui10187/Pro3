using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

    private Image backgroundImage; // 조이스틱의 뒷판 이미지
    private Image handleImage;     // 조이스틱의 핸들 이미지
    private Vector2 touchPosition;
    
    private void Awake() {
        backgroundImage = GetComponent<Image>();
        handleImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        
    }

    public void OnDrag(PointerEventData eventData) {

        touchPosition = Vector2.zero;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition)) {
            touchPosition.x = (touchPosition.x / backgroundImage.rectTransform.sizeDelta.x); // 뒷판 이미지의 가로 길이로 나눠주기
            touchPosition.y = (touchPosition.y / backgroundImage.rectTransform.sizeDelta.y); // 뒷판 이미지의 세로 길이로 나눠주기

            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition; // 터치한 지점이 뒷판 이미지를 넘어가면 정규화 해주기
            handleImage.rectTransform.anchoredPosition = new Vector2(touchPosition.x * backgroundImage.rectTransform.sizeDelta.x / 2, touchPosition.y * backgroundImage.rectTransform.sizeDelta.y / 2);
        } // 핸들을 터치한 지점으로 옮겨주기
    }

    public void OnPointerUp(PointerEventData eventData) {
        handleImage.rectTransform.anchoredPosition = Vector2.zero; // 조이스틱에서 손을 떼면 핸들을 중앙 위치로 초기화 시켜주기
        touchPosition = Vector2.zero;
    }

    public float TouchHorizontal() {
        return touchPosition.x;
    }

    public float TouchVertical() {
        return touchPosition.y;
    }
    
}