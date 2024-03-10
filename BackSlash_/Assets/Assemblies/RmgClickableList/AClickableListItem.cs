using RedMoonGames.Basics;
using System;
using UnityEngine;

namespace RedMoonGames.ClickableList
{
    public abstract class AClickableListItem<TItem, TModel> : CachedBehaviour where TItem : AClickableListItem<TItem, TModel>
    {
        protected TModel _model;
        protected bool _isConstructed;

        public event Action<TItem> OnItemClicked;

        public virtual void ItemConstruct()
        {
            _isConstructed = true;
        }

        public virtual void ItemRecycle()
        {
            if (!_isConstructed)
            {
                return;
            }

            _isConstructed = false;
        }

        public TryResult TrySetItemModel(TModel model)
        {
            _model = model;

            if (!IsValidModel(_model))
            {
                return TryResult.Fail;
            }

            InitItemModel(_model);
            return TryResult.Successfully;
        }

        protected virtual void InitItemModel(TModel model)
        {

        }

        protected virtual bool IsValidModel(TModel model)
        {
            return NullCast.ToNullCast(model);
        }

        protected void ItemClicked()
        {
            OnItemClicked?.Invoke(this as TItem);
        }

        protected virtual void ItemDestroy()
        {
        }

        private void OnDestroy()
        {
            if (_isConstructed)
            {
                ItemRecycle();
            }

            ItemDestroy();
        }
    }
}
