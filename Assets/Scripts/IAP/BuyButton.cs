using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public bool subscription = false;
    public bool countable = true;
    public Button button;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI priceText;

    private void Awake() {
        button  = GetComponent<Button>();
    }

    public void OnProductFetched(ConsumableItem item)
    {
        priceText.text = item.price;
        if(countable){
            titleText.text = item.quantity.ToString();
        }
    }
}