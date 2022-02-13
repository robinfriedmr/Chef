//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickSound : MonoBehaviour, IPointerDownHandler
{

    public AudioClip clickSound;
    public AudioSource audioSource;

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.PlayOneShot(clickSound);
    }
}
