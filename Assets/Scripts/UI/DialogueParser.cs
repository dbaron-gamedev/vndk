using System;
using UnityEngine;

namespace UI
{
    public class DialogueParser : MonoBehaviour
    {
        public delegate void ActionParsed(string actor, string action, string marker);
        public static event ActionParsed OnActionParsed;
        
        public delegate void DialogueParsed(string line);
        public static event DialogueParsed OnDialogueParsed;
        
        public delegate void CharacterParsed(string line);
        public static event CharacterParsed OnCharacterParsed;
        
        public delegate void MarkerParsed(string marker);
        public static event MarkerParsed OnMarkerParsed;

        public static void ParseLine(string input)
        {
            var parts = input.Split(';');
            
            try
            {
                OnCharacterParsed?.Invoke(parts[0]);
                OnActionParsed?.Invoke(parts[0], parts[1], parts[2]);
                OnDialogueParsed?.Invoke(parts[3]);
            }
            catch (IndexOutOfRangeException exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}