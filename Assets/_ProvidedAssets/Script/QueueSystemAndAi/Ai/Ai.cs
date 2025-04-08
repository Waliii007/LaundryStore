using Pathfinding;
using UnityEngine;

namespace LaundaryMan
{
    public class Ai : Character
    {
        
        

        protected override void OnAiEnable()
        {
        }

        public override void OnStartGame()
        {
        }

        protected override void OnAnimationUpdate()
        {
        }

        public virtual void SetDestination(GameObject target)
        {
        }

        public virtual float speed()
        {
            return 0;
        }
    }
}