using System;
using System.Collections;
using System.Collections.Generic;
using RedMoonGames.Basics;
using UnityEngine;
using Zenject;

namespace RedMoonGames.ClickableList
{
    public abstract class AClickableList<TItem, TItemModel, TListModel> : CachedBehaviour where TItem : AClickableListItem<TItem, TItemModel>
    {
        [Header("ClickableList")]
        [SerializeField] private TItem itemPrefab;
        [SerializeField] private Transform itemsRoot;

        private IInstantiator _container;

        private TListModel _model;

        private Pool<TItem> _items;
        private Dictionary<TItem, TItemModel> _itemParams = new Dictionary<TItem, TItemModel>();

        [Inject]
        private void Construct(IInstantiator container)
        {
            _container = container;

            _items = new Pool<TItem>(() =>
            {
                var newItem = _container.InstantiatePrefabForComponent<TItem>(itemPrefab, itemsRoot);              
                newItem.gameObject.SetActive(false);

                return newItem;
            },
            0);
        }

        protected virtual void InitClickableList(TListModel listModel)
        {
            _model = listModel;
        }

        protected virtual void ItemClicked(TItem item)
        {
        }

        protected TItemModel GetItemParams(TItem item)
        {
            if(!GetItemParams(item, out var itemParams))
            {
                return default(TItemModel);
            }

            return itemParams;
        }

        protected bool GetItemParams(TItem item, out TItemModel itemParams)
        {
            return _itemParams.TryGetValue(item, out itemParams);
        }

        protected virtual TItem CreateItem(TItemModel itemModel)
        {
            var item = _items.Take();
            _itemParams.Add(item, itemModel);

            item.OnItemClicked += ItemClicked;

            if (!item.TrySetItemModel(itemModel))
            {
                RecyleItem(item);
                return null;
            }

            item.gameObject.SetActive(true);
            item.ItemConstruct();

            return item;
        }

        protected virtual void RecyleItem(TItem item)
        {
            item.OnItemClicked -= ItemClicked;

            if (_itemParams.ContainsKey(item))
            {
                _itemParams.Remove(item);
            }

            item.gameObject.SetActive(false);
            item.ItemRecycle();

            _items.Recycle(item);
        }

        protected void RecyleItems()
        {
            foreach(var activePoolItem in _items.GetActive())
            {
                RecyleItem(activePoolItem);
            }
        }

        private void OnDestroy()
        {
            RecyleItems();
        }
    }
}
