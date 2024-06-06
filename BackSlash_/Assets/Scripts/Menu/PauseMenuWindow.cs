using TMPro;
using UnityEngine;

namespace Scripts.Menu
{
    public class PauseMenuWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _continueField;
        [SerializeField] private TMP_Text _settingsField;
        [SerializeField] private TMP_Text _exitField;

        private void Awake()
        {
            _continueField = transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            _settingsField = transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
            _exitField = transform.GetChild(2).gameObject.GetComponent<TMP_Text>();
        }
    }
}
