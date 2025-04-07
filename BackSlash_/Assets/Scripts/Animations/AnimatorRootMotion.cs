using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class AnimatorRootMotion : MonoBehaviour
{
    public bool ApplyRootMotion;
    [SerializeField] private NavMeshAgent _agent;
    private Animator _animator;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    //TODO switch root motion depending on the condition
    
    private void Start()
    {
        _agent.updatePosition = false;
        _agent.updateRotation = true;
    }

    private void OnAnimatorMove()
    {
        if (ApplyRootMotion)
        {
            MoveRootMotion();
        }
    }
    
    private void MoveRootMotion()
    {
        Vector3 rootPosition = _animator.rootPosition;
        rootPosition.y = _agent.nextPosition.y;
        _agent.gameObject.transform.position = rootPosition;
        _agent.nextPosition = rootPosition;
    }
}
