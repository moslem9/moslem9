using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class MyPurchasement : MonoBehaviour, IStoreListener
{
    IStoreController storeController;

    [Serializable]
    public class NonConsumableItem
    {
        public string name;
        public string id;
        public string desc;
        public float price;
    }

    public List<NonConsumableItem> nonConsumableItems;

    void Start()
    {
        SetupBuilder();
    }

    void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < nonConsumableItems.Count; i++)
        {
            CheckNonConsumable(nonConsumableItems[i].id);
        }
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Initialized...");
        storeController = controller;
        for (int i = 0; i < nonConsumableItems.Count; i++)
        {
            CheckNonConsumable(nonConsumableItems[i].id);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print($"Initialize failed {error}");
    }

    void CheckNonConsumable(string id)
    {
        if (storeController != null)
        {
            var product = storeController.products.WithID(id);
            if (product != null)
            {
                if (product.hasReceipt)
                {
                    print($"{id} Product Bought");
                }
                else
                {
                    print($"{id} Product Not Bought");
                }
            }
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        print($"Purchase Completed {product.definition.id}");
        for (int i = 0; i < nonConsumableItems.Count; i++)
        {
            if (product.definition.id == nonConsumableItems[i].id)
            {
                print($"{nonConsumableItems[i].id} Product Bought");
            }
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print($"Purchase failed");
    }

    public void PurchaseItem(int i)
    {
        storeController.InitiatePurchase(nonConsumableItems[i].id);
    }
}
