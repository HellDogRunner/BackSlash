using Scripts.Inventory;
using UnityEngine;
using Zenject;

public class WeaponTab : MonoBehaviour
{
    [SerializeField] WeaponPartView _bladeFrame;
    [SerializeField] WeaponPartView _guardFrame;

    private InventoryDatabase _playerInventory;

    [Inject]
    private void Construct(InventoryDatabase playerInventory)
    {
        _playerInventory = playerInventory;
    }

    private void Awake()
    {
        _bladeFrame.OnViewActive += AttachmentsList;
    }

    private void AttachmentsList(bool isActive)
    {
        if (isActive)
        {

        }
    }
}
