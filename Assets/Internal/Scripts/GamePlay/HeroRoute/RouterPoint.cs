using Internal.Scripts.GamePlay.TheMainHero;
using UnityEditor;
using UnityEngine;

namespace Internal.Scripts.GamePlay.HeroRoute
{
    [RequireComponent(typeof(Collider))]
    public class RouterPoint : MonoBehaviour
    {
        private const float GIZMOS_RADIUS = 0.1f;

        public bool IsReached { get; private set; }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.TryGetComponent(out MainHero _))
                IsReached = true;
        }

#if UNITY_EDITOR
        [DrawGizmo(GizmoType.Selected)]
        private Color _color = new Color(0f, 0.09f, 1f);
            
        public void SetGizmosColor(Color color)
        {
            _color = color;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, GIZMOS_RADIUS);
        }
#endif
    }
}
