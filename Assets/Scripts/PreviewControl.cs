using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewControl : MonoBehaviour {
    [SerializeField] private Image previewImage;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Show(Image srcImage) {
        gameObject.SetActive(true);

        previewImage.sprite = srcImage.sprite;
        nameText.text = srcImage.sprite.name;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}