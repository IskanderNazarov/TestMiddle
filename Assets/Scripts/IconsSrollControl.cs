using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IconsSrollControl : MonoBehaviour {
    [SerializeField] private Canvas canvas;
    [SerializeField] private PreviewControl previewControl;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Sprite[] sprites;

    public List<ImagesLayout> layouts;

    private RectTransform canvasTr;
    private RectTransform contentTr;
    private List<RectTransform> tr;
    private Vector2 panelScreenSpaceSize;
    private Vector2 panelCanvasSize;
    private Sprite[][] spritesMatrix;
    private bool isCoroutineActive;
    private int startSpritesIndex;

    private void Start() {
        canvasTr = canvas.GetComponent<RectTransform>();
        contentTr = (RectTransform) layouts[0].transform.parent;


        //setup panel for images
        for (var i = 0; i < layouts.Count; i++) {
            var l = layouts[i];
            l.SetUp(canvasTr);
            l.setPosition(i);

            l.OnImageClicked += OnImageClicked;
        }

        //calculate layouts screen space size
        panelCanvasSize = layouts[0].Size;
        panelScreenSpaceSize = Vector2.Scale(layouts[0].Size, transform.lossyScale);

        SetUpSpritesMatrix(layouts[0].ImagesCount);
        for (var i = 0; i < layouts.Count; i++) UpdateImages(i, 0);


        //resize content transform size to fit the
        contentTr.sizeDelta = new Vector2(contentTr.sizeDelta.x,
            (spritesMatrix.Length - 2) * layouts[0].Size.y);
    }

    private void OnImageClicked(Image image) {
        previewControl.Show(image);
    }

    private void Update() {
        //if (!isChecking) return;
        //return;
        if (layouts[0].Position.y > Screen.height + panelScreenSpaceSize.y + 1) {
            var first = layouts.First();
            layouts.RemoveAt(0);
            layouts.Add(first);

            first.anchoredPosition = new Vector2(0, layouts[^2].anchoredPosition.y - panelCanvasSize.y);


            startSpritesIndex++;
            ResetImages(layouts.Count - 1);
            if (startSpritesIndex + layouts.Count - 1 < spritesMatrix.Length) {
                UpdateImages(layouts.Count - 1, startSpritesIndex);
            }
        }
        else if (layouts[0].Position.y < Screen.height) {
            var last = layouts.Last();
            layouts.RemoveAt(layouts.Count - 1);
            layouts.Insert(0, last);

            last.anchoredPosition = new Vector2(0, layouts[1].anchoredPosition.y + panelCanvasSize.y);

            startSpritesIndex--;
            ResetImages(0);
            if (startSpritesIndex >= 0) {
                UpdateImages(0, startSpritesIndex);
            }
        }
    }


    private void SetUpSpritesMatrix(int imageCountInRow) {
        if (imageCountInRow == 0) imageCountInRow = 1;
        //spritesMatrix = new Sprite[sprites.Length / imageCountInRow][];
        var linesCount = sprites.Length / imageCountInRow;
        var rest = sprites.Length % imageCountInRow;
        if (rest != 0) linesCount++;


        spritesMatrix = new Sprite[linesCount][];
        var index = 0;
        for (var i = 0; i < spritesMatrix.Length; i++) {
            if (rest != 0 && i == spritesMatrix.Length - 1)
                spritesMatrix[i] = new Sprite[rest];
            else
                spritesMatrix[i] = new Sprite[imageCountInRow];

            for (var j = 0; j < spritesMatrix[i].Length; j++) {
                spritesMatrix[i][j] = sprites[index];
                index++;
            }
        }
    }


    private void UpdateImages(int layoutIndex, int shiftIndex) {
        var images = layouts[layoutIndex].Images;
        var index = shiftIndex + layoutIndex;
        for (var j = 0; j < spritesMatrix[index].Length; j++) {
            images[j].sprite = spritesMatrix[index][j];
            images[j].enabled = true;
        }
    }

    private void ResetImages(int layoutIndex) {
        foreach (var image in layouts[layoutIndex].Images) {
            image.enabled = false;
        }
    }

    public void UpButtonClicked() {
        if (!isCoroutineActive) {
            isCoroutineActive = true;
            StartCoroutine(ScrollAnim(1));
        }
    }

    public void DownClicked() {
        if (!isCoroutineActive) {
            isCoroutineActive = true;
            StartCoroutine(ScrollAnim(0));
        }
    }

    private IEnumerator ScrollAnim(float toScrollValue) {
        var startValue = scrollbar.value;
        const float time = 3;
        var timer = 0f;
        while (timer < time) {
            timer += Time.deltaTime;
            var t = timer / time;

            scrollbar.value = Mathf.Lerp(startValue, toScrollValue, t);
            yield return null;
        }

        isCoroutineActive = false;
    }
}