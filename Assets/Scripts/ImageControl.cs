using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageControl : MonoBehaviour {
    public Action<Image> OnImageClicked;

    public void clicked() {
        OnImageClicked?.Invoke(GetComponent<Image>());
    }
}