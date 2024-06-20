using UnityEngine;
using Controllers;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.Collections;
using System.Collections.Generic;
using Button = UnityEngine.UI.Button;

namespace UI
{
	public class DialogueBox : MonoBehaviour 
	{
		public Image avatar;
		public Texture2D cursor;
		
        private int _lineIndex;
        private int _lineCounter;
        private List<string> _lines = new();
        private StageController _scenarioController;
		private readonly Vector2 _hotSpot = Vector2.zero;
		
        [SerializeField] private GameObject box;
		[SerializeField] private Text textField;
		[SerializeField] private Button nextButton;
		[SerializeField] private Button audioButton;
		[SerializeField] private SpriteAtlas avatarAtlas;

        public delegate void ScriptEnded();
        public static event ScriptEnded OnScriptEnded;

        private void Awake()
        {
	        _scenarioController = FindFirstObjectByType<StageController>();
        }
        
        private void OnEnable()
        {
            DialogueParser.OnDialogueParsed += DisplayLine;
            StageController.OnCharacterActivated += DisplayAvatar;
          
            StageController.OnScenarioEnded += HideAudioButton;
            StageController.OnScenarioEnded += InitializeScenario;
            
            StageController.OnScenarioStarted += InitializeScenario;
            StageController.OnAudioClipLoaded += DisplayAudioButton;
        }

        private void OnDisable()
        {
            DialogueParser.OnDialogueParsed -= DisplayLine;
            StageController.OnCharacterActivated -= DisplayAvatar;
            
            StageController.OnScenarioEnded -= HideAudioButton;
            StageController.OnScenarioEnded -= InitializeScenario;
            
            StageController.OnScenarioStarted -= InitializeScenario;
            StageController.OnAudioClipLoaded -= DisplayAudioButton;
        }

		private void ParseScript()
		{
			var currentScenario = _scenarioController.CurrentScenario;

            if (currentScenario == null)
            {
                Debug.LogError("Scenario or script file is empty!");
                return;
            }
            
            _lines = currentScenario.script.text.Split('\n').ToList();
            _lineCounter = _lines.Count;
            _lineIndex = 0;
        }

		public void PlayAudioClip()
		{
			var audioSource = audioButton.GetComponent<AudioSource>();
			
			if (audioSource == null)
				return;
			
			audioSource.Play();
			audioButton.interactable = false;
			StartCoroutine(DeactivateAfterPlaying(audioSource));
		}

		private IEnumerator DeactivateAfterPlaying(AudioSource audioSource)
		{
			yield return new WaitForSeconds(audioSource.clip.length);
			audioButton.gameObject.SetActive(false);
		}
		
		private void DisplayAudioButton(AudioClip clip)
		{
			var audioSource = audioButton.GetComponent<AudioSource>();
			
			if (audioSource != null)
				audioSource.clip = clip;

			audioButton.interactable = true;
			audioButton.gameObject.SetActive(true);
		}

		private void HideAudioButton()
		{
			//audioButton.gameObject.SetActive(false);
		}
		
		public void OnPointerEnter()
		{
			Cursor.SetCursor(cursor, _hotSpot, CursorMode.ForceSoftware);
		}
	
		public void OnPointerExit()
		{
			Cursor.SetCursor(null, _hotSpot, CursorMode.ForceSoftware);
		}

		public void DisplayLine(string line)
		{
			if (string.IsNullOrWhiteSpace(line))
				NextLine();
			else
				textField.text = line;
		}

		public void NextLine()
		{
			if (_lineIndex > _lineCounter - 1)
			{
				nextButton.interactable = false;
                OnScriptEnded?.Invoke();
				return;
			}
			DialogueParser.ParseLine(_lines[_lineIndex++]);
		}

		private void InitializeScenario()
		{
            nextButton.interactable = true;
            ParseScript();
            NextLine();
        }

		private void DisplayAvatar()
		{
			var characterName = _scenarioController.ActiveCharacter.characterName;
            var avatarSprite = avatarAtlas.GetSprite(characterName);

            if (avatarSprite == null)
			{
				Debug.LogWarning("Character avatar missing!");
				return;
			}
			
            avatar.sprite = avatarSprite;
		}

		public void ShowDialogbox() // TODO - This needs to refactored, it should be attached to a parent container.
		{
			box.SetActive(true);
		}

		public void HideDialogbox()
		{
			box.SetActive(false);
		}

		public void UnLockDialogBox(bool lockBox)
		{
			nextButton.interactable = lockBox;
		}
	}
}