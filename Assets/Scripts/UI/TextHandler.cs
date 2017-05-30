using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private Text buttonText;

	void Start () {
        buttonText = GetComponentInChildren<Text>();
	}
	
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = UnityEngine.Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = UnityEngine.Color.white;
    }

}
