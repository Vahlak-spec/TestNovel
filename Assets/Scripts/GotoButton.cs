using System.Text;
using UnityEngine;
using Naninovel.Commands;
using System.Threading;
using UnityEngine.UI;

namespace Naninovel.UI
{

    public class GotoButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [Space]
        [SerializeField] private string _file;
        [SerializeField] private string _key;
        [Space]
        [SerializeField] private string[] _hideUI;
        [SerializeField] private string[] _hideCharacters;

        private IUIManager uiManager;
        private ICharacterManager characterManager;

        protected void Awake()
        {
            characterManager = Engine.GetService<ICharacterManager>();
            uiManager = Engine.GetService<IUIManager>();
            _button.onClick.AddListener(OnClick);
        }

        private async void OnClick()
        {
            foreach (var item in _hideCharacters)
            {
                if (characterManager.ActorExists(item))
                {
                    characterManager.GetActor(item).Visible = false;
                }
            }
            foreach (var item in _hideUI)
            {
                uiManager.GetUI(item).Hide();
            }
            string onClick;
            StringBuilder builder = new StringBuilder();
            builder.Clear();

            builder.AppendLine($"{Parsing.Identifiers.CommandLine}{nameof(Goto)} {_file ?? string.Empty}{(_key.Length > 0 ? $".{_key}" : string.Empty)}");
            onClick = builder.ToString().TrimFull();
            var script = Script.FromScriptText($"Go to", onClick);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }
    }
}