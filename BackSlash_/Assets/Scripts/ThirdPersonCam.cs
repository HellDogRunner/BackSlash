using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform Camera;
    [SerializeField] private Transform Orientation;
    [SerializeField] private Transform CombatLookAt;
    [SerializeField] private Transform Player;
    [SerializeField] private Transform PlayerObj;
    [SerializeField] private Rigidbody PlayerRigibody;
    [SerializeField] private float RotationTime;
    [SerializeField] private GameObject PlayerServices;
    [SerializeField] private _cameraStyle CurrentStyle;
    private float _turnSpeed;

    private enum _cameraStyle
    {
        Basic,
        Combat
    }

    private void Start()
    {
        DisableCursor();
    }

    private void Update()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        Vector3 ViewDir = Player.position - new Vector3(Camera.transform.position.x, Player.position.y, Camera.transform.position.z);
        Orientation.forward = ViewDir.normalized;

        Vector2 _axis = PlayerServices.GetComponent<MovementService>().ReturnAxis();

        Vector3 inputDir = Orientation.forward * _axis.y + Orientation.right * _axis.x;
        if (inputDir != Vector3.zero)
        {
            PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir.normalized, Time.deltaTime * RotationTime);
        }


    }

    private void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
