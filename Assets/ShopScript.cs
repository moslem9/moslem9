using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Purchasing;

[Serializable]
public class ConsumableItem {
    public string name;
    public string id;
    public string desc;
    public float price;
}

[Serializable]
public class NonConsumableItem
{
    public string name;
    public string id;
    public string desc;
    public float price;
}

[Serializable]
public class SubscriptionItem
{
    public string name;
    public string id;
    public string desc;
    public float price;
    public int timeDuration; //in days
}

public class ShopScript : MonoBehaviour, IStoreListener
{
    IStoreController storeController;
    public TextMeshProUGUI coinText;

    public ConsumableItem cItem;
    public NonConsumableItem ncItem;
    public SubscriptionItem sItem;

    public Data data;
    public Payload payload;
    public PayloadData payloadData;

    void Start()
    {
        int coins = PlayerPrefs.GetInt("totalCoins");
        coinText.text = coins.ToString();
        SetupBuilder();
    }

    void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(cItem.id, ProductType.Consumable);
        builder.AddProduct(ncItem.id, ProductType.NonConsumable);
        builder.AddProduct(sItem.id, ProductType.Subscription);
        UnityPurchasing.Initialize(this, builder);
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Initialized...");
        storeController = controller;
        CheckNonConsumable(ncItem.id);
        CheckSubscription(sItem.id);
    }

    public void Consumable_Btn_Pressed()
    {
        //AddCoins(50);
        storeController.InitiatePurchase(cItem.id);
    }

    private void AddCoins(int num)
    {
        int coins = PlayerPrefs.GetInt("totalCoins");
        coins += num;
        PlayerPrefs.SetInt("totalCoins", coins);
        StartCoroutine(StartCoinShakeEvent(coins - num, coins, 0.5f));
    }

    public GameObject adsPurchasedWindow;
    public GameObject adsBanner;
    float val;
    IEnumerator StartCoinShakeEvent(int oldValue, int newValue, float animTime)
    {
        float ct = 0;
        float nt;
        float tot = animTime;
        coinText.GetComponent<Animation>().Play("textShake");
        while (ct < tot)
        {
            ct += Time.deltaTime;
            nt = ct / tot;
            val = Mathf.Lerp(oldValue, newValue, nt);
            coinText.text = ((int)val).ToString();
            yield return null;
        }
        coinText.GetComponent<Animation>().Stop();

    }

    public void NonConsumable_Btn_Pressed()
    {
        //RemoveAds();
        storeController.InitiatePurchase(ncItem.id);
    }

    private void RemoveAds()
    {
        DisplayAds(false);
    }
    private void ShowAds()
    {
        DisplayAds(true);
    }

    void DisplayAds(bool x)
    {
        if (!x)
        {
            adsPurchasedWindow.SetActive(true);
            adsBanner.SetActive(false);
        }
        else
        {
            adsPurchasedWindow.SetActive(false);
            adsBanner.SetActive(true);
        }
    }

    public GameObject subActivatedWindow;
    public GameObject premiumBanner;
    public void Subscription_Btn_Pressed()
    {
        //ActivateElitePass();
        storeController.InitiatePurchase(sItem.id);
    }

    private void ActivateElitePass()
    {
        SetupElitePass(true);
    }

    private void DeActivateElitePass()
    {
        SetupElitePass(false);
    }

    private void SetupElitePass(bool x)
    {
        if (x)
        {
            subActivatedWindow.SetActive(true);
            premiumBanner.SetActive(true);
        }
        else
        {
            subActivatedWindow.SetActive(false);
            premiumBanner.SetActive(false);
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        print($"Purchase Completed {product.definition.id}");

        if (product.definition.id == cItem.id)
        {
            string receipt = product.receipt;
            data = JsonUtility.FromJson<Data>(receipt);
            payload = JsonUtility.FromJson<Payload>(data.payload);
            payloadData = JsonUtility.FromJson<PayloadData>(payload.json);
            int quantity = payloadData.quantity;
            for (int i = 0; i < quantity; i++)
            {
                AddCoins(50);
            }
        }
        else if (product.definition.id == ncItem.id)
        {
            RemoveAds();
        }
        else if (product.definition.id == sItem.id)
        {
            ActivateElitePass();
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print($"Initialize failed {error}");
    }



    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print($"Purchase failed");
    }

    void CheckNonConsumable(string id) {
        if (storeController!=null)
        {
            var product = storeController.products.WithID(id);
            if (product != null) {
                if (product.hasReceipt)
                {
                    RemoveAds();
                }
                else {
                    ShowAds();
                }
            } 
        }
    }

    void CheckSubscription(string id)
    {
        if (storeController != null)
        {
            var product = storeController.products.WithID(id);
            if (product != null)
            {
                try
                {
                    if (product.hasReceipt)
                    {
                        var subscriptionManager = new SubscriptionManager(product, null);
                        var info = subscriptionManager.getSubscriptionInfo();
                        print(info.getExpireDate());
                        if (info.isSubscribed() == Result.True)
                        {
                            print("We are subscribed");
                            ActivateElitePass();
                        }
                        else {
                            print("UnSubscribed");
                            DeActivateElitePass();
                        }
                    }
                    else
                    {
                        print("Receipt not found");
                    }
                }
                catch (Exception)
                {
                    print("It only work with google play, apple store and amazon store!");
                }
               
            }
            else {
                print("Product not found");
            }
        }
    }
}

[Serializable]
public class SkuDetails
{
    public string productId;
    public string type;
    public string title;
    public string name;
    public string iconUrl;
    public string description;
    public string price;
    public long price_amount_micros;
    public string price_currency_code;
    public string skuDetailsToken;
}

[Serializable]
public class PayloadData
{
    public string orderId;
    public string packageName;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
    public int quantity; 
    public bool acknowledged; 
}

[Serializable]
public class Payload
{
    public string json;
    public string signature;
    public List<SkuDetails> skuDetails;
    public PayloadData payloadData;
}

[Serializable]
public class Data
{
    public string payload;
    public string store;
    public string transactionId;
}
