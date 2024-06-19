using UI;
using Enums;
using System.Linq;
using Ingredients;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class StageController: MonoBehaviour
    {
        // Publics
        public List<Scenario> endings;
        public List<Scenario> scenarios;
        public Scenario CurrentScenario { private set; get; }
        public Character ActiveCharacter { private set; get; }

        // Privates
        private int _scenarioIndex;
        private List<Character> _characters = new();
        private readonly Dictionary<string, GameObject> _spawns = new();
        private readonly Dictionary<string, Transform> _markers = new();
        
        // Events
        public delegate void AudioClipLoaded(AudioClip clip);
        public static event AudioClipLoaded OnAudioClipLoaded;
        public delegate void ScenarioStarted();
        public static event ScenarioStarted OnScenarioStarted;

        public delegate void ScenarioEnded();
        public static event ScenarioEnded OnScenarioEnded;

        public delegate void CharacterActivated();
        public static event CharacterActivated OnCharacterActivated;
        
        private void OnEnable()
        {
            DialogueBox.OnScriptEnded += LoadNextScenario;
            DialogueParser.OnActionParsed += ActivateActor;
            DialogueParser.OnCharacterParsed += SetActiveCharacter;
        }

        private void OnDisable()
        {
            DialogueBox.OnScriptEnded -= LoadNextScenario;
            DialogueParser.OnActionParsed -= ActivateActor;
            DialogueParser.OnCharacterParsed -= SetActiveCharacter;
        }

        private void Awake()
        {
            LoadScenario();
        }

        private void Start()
        {
            SetStage();
        }

        private void SetMarkers()
        {
            var markers = FindObjectsByType<Marker>(FindObjectsSortMode.None);
            
            foreach (var marker in markers)
            {
                _markers.TryAdd(marker.position.ToString(), marker.transform);
            }
        }
        
        private void LoadScenario()
        {
            if (scenarios.Count <= 0)
                Debug.LogError("No scenarios to load.");
            
            if (endings.Count <= 0)
                Debug.LogError("No endings to load.");
            
            var randomIndex = Random.Range(0, endings.Count);
            scenarios.Add(endings[randomIndex]);
            
            CurrentScenario = scenarios[_scenarioIndex];
        }
        
        private void SetStage()
        {
            SetMarkers();
            DisplayBackdrop();
            
            if (CurrentScenario.audioClip)
                OnAudioClipLoaded?.Invoke(CurrentScenario.audioClip);
            
            OnScenarioStarted?.Invoke();
        }
        
        private void Teardown()
        {
            var gameObjects = FindObjectsOfType<GameObject>();
            
            foreach (var gameObject in gameObjects)
            {
                var renderer = gameObject.GetComponent<Renderer>();
                
                if (renderer != null && renderer.sortingLayerName == "Backdrop")
                    Destroy(gameObject);
                
                if (renderer != null && renderer.sortingLayerName == "Character") 
                    Destroy(gameObject);
            }
            
            _spawns.Clear();
        }
        
        private void LoadNextScenario()
        {
            OnScenarioEnded?.Invoke();

            _scenarioIndex += 1;
            
            if (_scenarioIndex == scenarios.Count)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }
            
            CurrentScenario = scenarios[_scenarioIndex];
            
            Teardown();
            SetStage();
        }

        private void DisplayBackdrop()
        {
            if (CurrentScenario.backdrop)
                Instantiate(scenarios[_scenarioIndex].backdrop, transform.position, Quaternion.identity);
        }

        // TODO: The CharacterController class should should set the current active Character.;
        private void SetActiveCharacter(string characterName) 
        {
            ActiveCharacter = 
                scenarios[_scenarioIndex].characters.FirstOrDefault(x => x.characterName == characterName);

            if (ActiveCharacter == null)
            {
                Debug.LogWarning("No active character found.");
                return;
            }

            OnCharacterActivated?.Invoke();
        }
        
        private void ActivateActor(string actor, string action, string marker)
        {
            switch (action)
            {
                case nameof(Action.Move):
                    MoveActor(actor, action, marker);
                    break;
                case nameof(Action.Spawn):
                    SpawnActor(actor, marker);
                    break;
            }
        }

        private void SpawnActor(string actor, string marker)
        {
            _characters = scenarios[_scenarioIndex].characters;

            var targetActor = _characters.Find(x => x.characterName == actor);
            var markerTransform = _markers[marker];
            var spawnActor = Instantiate(targetActor.characterPrefab, markerTransform.transform.position, Quaternion.identity);
            spawnActor.transform.position = new Vector3(markerTransform.transform.position.x, targetActor.characterPrefab.transform.position.y, transform.position.z);
            
            _spawns.TryAdd(targetActor.characterName, spawnActor);
        }
        
        private void MoveActor(string actor, string action, string marker)
        {
            if (action != nameof(Action.Move))
                return;
            
            var markerTransform = _markers[marker];
            var targetCharacter = _spawns[actor];
            //targetCharacter.transform.DOMoveX(markerTransform.transform.position.x, 1);
            
            StartCoroutine(MoveCharacterToX(targetCharacter.transform, markerTransform.position.x, 1f));
        }
        
        private static IEnumerator MoveCharacterToX(Transform target, float targetX, float duration)
        {
            var elapsedTime = 0f;
            var startPos = target.position;
            var endPos = new Vector3(targetX, startPos.y, startPos.z);

            while (elapsedTime < duration)
            {
                target.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            target.position = endPos;
        }
    }
}