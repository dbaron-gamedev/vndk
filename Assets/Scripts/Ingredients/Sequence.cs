using UnityEngine;
using System.Collections.Generic;

namespace Ingredients
{
    public abstract class Sequence : MonoBehaviour
    {
        public delegate void SequenceStarted();
        public static event SequenceStarted OnSequenceStarted;
        
        public delegate void SequenceEnded();
        public static event SequenceEnded OnSequenceEnded;
        
        [SerializeField] protected List<SpriteRenderer> stills = new();

        private void Start()
        {
            StartSequence();
        }

        public virtual void StartSequence()
        { 
            OnSequenceStarted?.Invoke();
        }
        
        protected virtual void EndSequence()
        { 
            OnSequenceEnded?.Invoke();
        }
    }
}
