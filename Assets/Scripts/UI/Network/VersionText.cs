using Network;
using TMPro;
using UnityEngine;

namespace UI.Network
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private VersionSetter _versionSetter;

        private void Awake()
        {
            _versionSetter = FindObjectOfType<VersionSetter>();
            _text.SetText(_versionSetter.Version);
        }
    }
}
