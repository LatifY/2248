using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiamondInfo : MonoBehaviour
{
    public TextMeshProUGUI diamondText;
    private float diamondCurrentValue = 0;
    private void Update() {
        diamondCurrentValue = Mathf.Lerp(diamondCurrentValue,DiamondManager.instance.GetDiamond(), Time.deltaTime * 5);
        diamondText.text = Mathf.RoundToInt(diamondCurrentValue).ToString();
    }
}
