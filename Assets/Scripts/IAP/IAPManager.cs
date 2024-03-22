using System;
using System.Collections;
using System.Collections.Generic;
using GameArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

[Serializable]
public class ConsumableItem
{
    public string id;
    public string name;
    public int quantity;
    public bool countable = true;
    public string description;
    public string price;
    public BuyButton buyButton;
}


public class IAPManager : MonoBehaviour, IStoreListener
{
    private IStoreController storeController;

    public TextMeshProUGUI errorText;
    public TextMeshProUGUI successText;

    public List<ConsumableItem> consumableItems = new();

    private string successTextInstance;

    private void Awake()
    {
        errorText.gameObject.SetActive(false);
        successText.gameObject.SetActive(false);
        successTextInstance = successText.text;

        SetupBuilder();
    }

    private void Update()
    {
        foreach (var item in consumableItems)
        {
            if (!item.countable && PlayerPrefs.GetInt(item.name, 0) == 1)
            {
                item.buyButton.button.interactable = false;
                item.buyButton.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }
    }

    public void SetupBuilder()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        StandardPurchasingModule.Instance().useFakeStoreAlways = true;
        StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
#else
    StandardPurchasingModule.Instance().useFakeStoreAlways = false;
#endif

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var item in consumableItems)
        {
            builder.AddProduct(item.id, ProductType.Consumable);
        }

        //builder.AddProduct(subscriptionItem.id, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        AudioManager.Instance.PlayClip("purchase_error");

        errorText.gameObject.SetActive(true);
        successText.gameObject.SetActive(false);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Intialize Failed: " + message + error.ToString());
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        Debug.Log("Purchased: " + product.definition.id);

        int id = consumableItems.FindIndex(item => item.id == product.definition.id);
        if (id >= 0)
        {
            if (consumableItems[id].countable)
            {
                DiamondManager.instance.AddDiamond(consumableItems[id].quantity);
            }
            else
            {
                PlayerPrefs.SetInt(consumableItems[id].name, 1);
            }
        }

        // if (product.definition.id == subscriptionItem.id)
        // {
        //     HandleSubscription.instance.Subscribe();
        // }

        Debug.Log("Purchase Successful");
        AudioManager.Instance.PlayClip("purchase_success");
        successText.gameObject.SetActive(true);
        errorText.gameObject.SetActive(false);
        return PurchaseProcessingResult.Complete;
    }

    public void OnRestored(bool success, string message){
        Debug.Log(success + " " + message);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP Initialized Successfully");
        storeController = controller;
        foreach (var item in consumableItems)
        {
            item.price = storeController.products.WithID(item.id).metadata.localizedPriceString;
            item.buyButton.OnProductFetched(item);
            item.buyButton
                .GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    storeController.InitiatePurchase(item.id);
                });

            if (item.countable && PlayerPrefs.GetInt(item.name, 0) == 1)
            {
                item.buyButton.button.interactable = false;
            }
        }
        // subscriptionItem.price = storeController.products
        //     .WithID(subscriptionItem.id)
        //     .metadata.localizedPriceString;
        // subscriptionItem.buyButton.OnProductFetched(subscriptionItem);
        // subscriptionItem.buyButton
        //     .GetComponent<Button>()
        //     .onClick.AddListener(() =>
        //     {
        //         storeController.InitiatePurchase(subscriptionItem.id);
        //     });
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Intialize Failed: " + error.ToString());
    }
}
