using Scripts.Player;
using Scripts.Weapon.Models;
using System;
using UnityEngine;
using Zenject;

namespace Scripts.Weapon
{
	public class WeaponController : MonoBehaviour
	{
		[SerializeField] private Transform _weaponPivot;
		[SerializeField] private Transform _weaponOnBeltPivot;

		private float _timeSinceAttack;
		private bool _isAttacking;
		private bool _isBlocking;

		protected WeaponTypesDatabase _weaponTypesDatabase;
		private EWeaponType _curentWeaponType;
		private GameObject _currentWeapon;
		
		private InputController _inputController;
		private WeaponTypeModel _weaponTypeModel;
		private DiContainer _diContainer;

		public EWeaponType CurrentWeaponType => _curentWeaponType;

		public event Action OnDrawWeapon;
		public event Action OnSneathWeapon;
		public event Action<bool> OnWeaponEquip;

		[Inject]
		private void Construct(WeaponTypesDatabase weaponTypesDatabase, InputController inputController, DiContainer diContainer)
		{
			_diContainer = diContainer;
			_weaponTypesDatabase = weaponTypesDatabase;
			_inputController = inputController;
		}

		private void Awake()
		{
			_curentWeaponType = EWeaponType.None;
			
			_inputController.OnShowWeaponPressed += WeaponPressed;
		}

		private void OnDestroy()
		{
			_inputController.OnShowWeaponPressed -= WeaponPressed;
		}

		private void Start()
		{
			_weaponTypeModel = _weaponTypesDatabase.GetWeaponTypeModel(EWeaponType.Melee);
			_currentWeapon = _diContainer.InstantiatePrefab(_weaponTypeModel?.WeaponPrefab, _weaponOnBeltPivot.position, _weaponOnBeltPivot.rotation, _weaponOnBeltPivot.transform);
		}

		private void WeaponPressed()
		{
			if (_curentWeaponType == EWeaponType.None)
			{
				EquipWeapon();
			}
			else if (_curentWeaponType == EWeaponType.Melee)
			{
				UnequipWeapon();
			}
		}

		private void EquipWeapon()
		{
			_curentWeaponType = EWeaponType.Melee;
			OnWeaponEquip?.Invoke(true);
		}

		public void UnequipWeapon()
		{
			_curentWeaponType = EWeaponType.None;
			OnWeaponEquip?.Invoke(false);
		}

		private void DrawWeapon()
		{
			if (_currentWeapon == null)
			{
				return;
			}
			ChangeWeaponTransform(_weaponPivot);
			OnDrawWeapon?.Invoke();
		}

		private void SheathWeapon()
		{
			if (_currentWeapon == null)
			{
				return;
			}
			ChangeWeaponTransform(_weaponOnBeltPivot);
			OnSneathWeapon?.Invoke();
		}

		private void ChangeWeaponTransform(Transform target)
		{
			_currentWeapon.transform.parent = target.transform;
			_currentWeapon.transform.position = target.position;
			_currentWeapon.transform.rotation = target.rotation;
		}
	}
}