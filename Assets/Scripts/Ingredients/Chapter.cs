using System.Collections.Generic;
using UnityEngine;

namespace Ingredients
{
    [CreateAssetMenu(fileName = "NewChapter", menuName = "VNDK/Chapter", order = 1)]
    public class Chapter : ScriptableObject
    {
        public string chapterName;
        public List<Scenario> scenarios;
    }
}
