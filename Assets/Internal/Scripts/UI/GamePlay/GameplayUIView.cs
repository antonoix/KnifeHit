using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayUIView : MonoBehaviour
    {
        [SerializeField] private GameplayResultPanel gameplayResultPanel;
    }

    public class GameplayResultPanel
    {
        [SerializeField] private TMP_Text resultHeader;
        [SerializeField] private Button goNextButton;

        private void Awake()
        {
            
        }
    }
}
