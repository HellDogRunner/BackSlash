using DG.Tweening;
using UnityEngine;

public class InteractionAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _lookAtDuration = 0.3f;

    private Transform _npcTR;
    private Vector3 _defaultRotation;

    public void SetNPCTransform(Transform transform, Vector3 rotation)
    {
        _npcTR = transform;
        _defaultRotation = rotation;
    }

    public void LookAtEachOther(Transform playerTR)
    {
        Vector3 playerPos = new Vector3(playerTR.position.x, _npcTR.position.y, playerTR.position.z);
        Vector3 npcPos = new Vector3(_npcTR.position.x, playerTR.position.y, _npcTR.position.z);

        _npcTR.DOLookAt(playerPos, _lookAtDuration);
        playerTR.DOLookAt(npcPos, _lookAtDuration);
    }

    public void RotateToDefault()
    {
        _npcTR.DORotate(_defaultRotation, _lookAtDuration);
    }
}