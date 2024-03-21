using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameArchitecture;

public enum Numbers { }

public class BlockController : MonoBehaviour
{
    private int numIndex = 0;
    public bool clicked = false;
    public bool choosen = false;

    //private RectTransform rect;
    public GameObject outline;
    private Image image;
    private TextMeshProUGUI text;
    private Animator animator;

    public float darkenAmount;

    public float xa = -300,
        xb = 150,
        ya = -1040,
        yb = 150;

    private List<string> colors = new List<string>()
    {
        "#FF0000", // Kırmızı
        "#FFA500", // Turuncu
        "#FFFF00", // Sarı
        "#008000", // Yeşil
        "#0000FF", // Mavi
        "#4B0082", // İndigo
        "#EE82EE", // Menekşe
        "#FFC0CB", // Pembe
        "#800000", // Kahverengi
        "#808000", // Zeytin Yeşili
        "#008080", // Teal
        "#800080", // Mor
        "#00FF00", // Lime Yeşili
        "#00FFFF", // Açık Mavi
        "#000080", // Koyu Mavi
        "#C0C0C0", // Gümüş
        "#808080", // Gri
        "#FFFFFF", // Beyaz
        "#FFD700", // Altın
        "#D2691E", // Çikolata
        "#7FFF00", // Chartreuse
        "#20B2AA", // Light Deniz Yeşili
        "#7CFC00", // Lawn Green
        "#48D1CC", // Medium Deniz Yeşili
        "#ADFF2F", // Yeşil Sarı
        "#40E0D0", // Turkuaz
        "#228B22", // Orman Yeşili
        "#00FF7F", // Spring Yeşili
        "#87CEEB", // Sky Mavisi
        "#6495ED", // Yumuşak Mavi
        "#1E90FF", // Orta Mavi
        "#87CEFA", // Light Sky Mavisi
        "#4682B4", // Steel Blue
        "#B0C4DE", // Light Steel Mavisi
        "#ADD8E6", // Açık Mavi
        "#87CEEB", // Sky Mavisi
        "#6A5ACD", // Slate Blue
        "#483D8B", // Dark Slate Mavi
        "#F0E68C", // Khaki
        "#F4A460", // Sandy Brown
        "#D2B48C", // Tan
        "#DEB887", // Burlywood
        "#FFE4B5", // Moccasin
        "#FFA07A", // Light Salmon
        "#FF7F50", // Coral
        "#FF6347", // Tomato
        "#FF4500", // Orange Red
        "#FF8C00", // Dark Orange
        "#FFDAB9", // Peach Puff
        "#CD853F" // Peru
    };

    private void Awake()
    {
        image = this.GetComponent<Image>();
        text = this.GetComponentInChildren<TextMeshProUGUI>();
        animator = this.GetComponent<Animator>();
    }

    private void Start() {
        SetColor();
    }

    public void SetPosition(int i, int j)
    {
        var pos = new Vector3 { x = -300 + i * 150, y = -500 + j * 150 };
        //var pos = new Vector3 { x = -220 + i * 110, y = -300 + j * 110 };
        this.GetComponent<RectTransform>().anchoredPosition = pos;
        //this.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 1);
    }

    public void SetNumIndex(int t)
    {
        numIndex = t;
        text.text = GameMaster.instance.numbers[numIndex];
        SetColor();
    }

    public void SetColor(){
        ColorUtility.TryParseHtmlString(colors[numIndex], out Color newCol);
        image.color = newCol;
    }

    public int GetNumIndex()
    {
        return numIndex;
    }

    public void Choose(){
        choosen = true;
        animator.SetBool("clicked", true);
        outline.SetActive(true);
    }

    public void Unchoose(){
        choosen = false;
        animator.SetBool("clicked", false);
        outline.SetActive(false);
    }

    public void Click()
    {
        if (clicked == true)
            return;
        clicked = true;

        animator.SetBool("clicked", true);

        image.color = new Color(
            image.color.r - darkenAmount,
            image.color.g - darkenAmount,
            image.color.b - darkenAmount,
            1f
        );
    }

    public void Unclick()
    {
        if (clicked == false)
            return;
        clicked = false;
        animator.SetBool("clicked", false);
        SetColor();
    }

    public bool GetClicked()
    {
        return clicked;
    }

    public bool GetChoosen(){
        return choosen;
    }

    public void MovePosition(Vector2 x, float time)
    {
        this.GetComponent<RectTransform>().DOAnchorPos(x, time).SetEase(Ease.OutQuad);
    }
}
