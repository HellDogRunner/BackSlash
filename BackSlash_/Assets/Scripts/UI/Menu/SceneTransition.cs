using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private TMP_Text _loadingPocentage;
        [SerializeField] private Image _loadingPragressBar;

        private static SceneTransition instance;
        private static bool _sceneIsLoad = false;

        private Animator _transitionAnimator;
        private AsyncOperation loadingSceneOperation;

        private void Start()
        {
            instance = this;
            _transitionAnimator = GetComponent<Animator>();

            if (_sceneIsLoad)
            {
                _transitionAnimator.SetTrigger("sceneOpening");
            }
        }

        private void Update()
        {
            if (loadingSceneOperation != null)
            {
            _loadingPocentage.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";
            _loadingPragressBar.fillAmount = loadingSceneOperation.progress;
            }
        }

        public static void SwichToScene(string sceneName)
        {
            instance._transitionAnimator.SetTrigger("sceneClosing");

            instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
            instance.loadingSceneOperation.allowSceneActivation = false;
        }

        public void OnAnimationOver()
        {
            _sceneIsLoad = true;
            loadingSceneOperation.allowSceneActivation = true;
        }
    }
}
