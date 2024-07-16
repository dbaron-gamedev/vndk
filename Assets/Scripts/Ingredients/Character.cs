using UnityEngine;

namespace Ingredients
{
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "VNDK/Character", order = 1)]
    public class Character : ScriptableObject
    {
        public string characterName;
        public GameObject characterPrefab;
    }
}