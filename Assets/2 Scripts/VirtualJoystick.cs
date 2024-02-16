using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

    private Image backgroundImage;
    private Image handleImage;
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
            touchPosition.x = (touchPosition.x / backgroundImage.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / backgroundImage.rectTransform.sizeDelta.y);

            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;
            handleImage.rectTransform.anchoredPosition = new Vector2(touchPosition.x * backgroundImage.rectTransform.sizeDelta.x / 2, touchPosition.y * backgroundImage.rectTransform.sizeDelta.y / 2); }
    }

    public void OnPointerUp(PointerEventData eventData) {
        handleImage.rectTransform.anchoredPosition = Vector2.zero; // 조이스틱을 터치하지 않으면 중앙 위치로 초기화 시켜주기
        touchPosition = Vector2.zero;
    }

    public float Horizontal() {
        return touchPosition.x;
    }

    public float Vertical() {
        return touchPosition.y;
    }

}