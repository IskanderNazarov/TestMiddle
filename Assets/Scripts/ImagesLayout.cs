using System;
using UnityEngine;
using UnityEngine.UI;

public class ImagesLayout : MonoBehaviour {
    [SerializeField] Image imageSample;
    [SerializeField] RectTransform area;


    private RectTransform tr;
    private Image[] images;

    public Action<Image> OnImageClicked;
    public Vector2 Size => tr.rect.size;
    public Vector2 Position => tr.position;
    public int ImagesCount => imagesCount;
    public Image[] Images => images;

    public Vector2 anchoredPosition {
        get => tr.anchoredPosition;
        set => tr.anchoredPosition = value;
    }

    private Vector2 placeAreaSize;
    private int imagesCount;

    //----------------------------------------------------

    public void SetUp(RectTransform canvasTr) {
        tr = GetComponent<RectTransform>();

        tr.sizeDelta = new Vector2(canvasTr.rect.width, canvasTr.rect.height * 0.5f);
        placeAreaSize = new Vector2(tr.rect.width - canvasTr.rect.width * 0.2f,
            tr.rect.height - canvasTr.rect.height * 0.1f);

        //testing
        if (area != null) {
            area.anchoredPosition = Vector2.zero;
            area.sizeDelta = placeAreaSize;
        }


        imagesCount = (int) (placeAreaSize.x / (placeAreaSize.y + canvasTr.rect.width * 0.05f));
        if (imagesCount < 1) imagesCount = 1;

        images = new Image[imagesCount];
        for (var i = 0; i < images.Length; i++) {
            var image = Instantiate(imageSample, tr).GetComponent<Image>();
            var imageTr = image.GetComponent<RectTransform>();
            var imageControl = image.GetComponent<ImageControl>();
            imageTr.sizeDelta = Vector2.one * placeAreaSize.y;
            images[i] = image;
            
            imageControl.OnImageClicked += delegate(Image img) {
                OnImageClicked?.Invoke(img);
            };
            
            
            var padding = canvasTr.rect.width * 0.1f;
            var startX = (Size.x - placeAreaSize.x) / 2;
            startX = startX + (placeAreaSize.x - imagesCount * (placeAreaSize.y + canvasTr.rect.width * 0.1f)) / 2;
            imageTr.anchoredPosition = new Vector2(startX + i * (placeAreaSize.y + padding), 0);
        }
    }

    //----------------------------------------------------

    public void setPosition(int linenIndex) {
        tr.anchoredPosition = new Vector2(0, linenIndex * -tr.rect.height);
    }

    //----------------------------------------------------
}