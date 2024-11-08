using Scripts.Inventory;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeaponTab : MonoBehaviour
{
    [SerializeField] WeaponPartView _bladeFrame;
    [SerializeField] RectTransform _bladeFrameRoot;
    //[SerializeField] WeaponPartView _guardFrame;
    //[SerializeField] RectTransform _guardFrameRoot;
    [Space]
    [SerializeField] GameObject _attachmentPrefab;

    private InventoryDatabase _playerInventory;
    private DiContainer _diContainer;
    private List<GameObject> _items = new List<GameObject>();

    [Inject]
    private void Construct(InventoryDatabase playerInventory, DiContainer diContainer)
    {
        _playerInventory = playerInventory;
        _diContainer = diContainer;
    }

    private void Awake()
    {
        _bladeFrame.OnViewActive += ShowAttachmentsList;
    }

    private void OnDisable()
    {
        _bladeFrame.OnViewActive -= ShowAttachmentsList;
    }

    private void ShowAttachmentsList(bool isActive, EItemType itemType)
    {
        if (isActive)
        {
            foreach (var item in _playerInventory.GetData())
            {
                if (item.ItemType == itemType)
                {
                    var prefab = _diContainer.InstantiatePrefab(_attachmentPrefab, _bladeFrameRoot);
                    _items.Add(prefab);
                    var itemView = prefab.GetComponent<AttachmentItemView>();

                    itemView.SetValues(item);
                }
            }
        }
        else
        {
            foreach (var item in _items)
            {
                Destroy(item);
            }
        }
    }
}
