using UnityEngine;
using System.Collections.Generic;

namespace Ingredients
{
    [CreateAssetMenu(fileName = "NewScenario", menuName = "VNDK/Scenario", order = 3)]
    public class Scenario : ScriptableObject
    {
        public TextAsset script;
        public GameObject backdrop;
        public AudioClip audioClip;
        public List<Character> characters;
    }
}