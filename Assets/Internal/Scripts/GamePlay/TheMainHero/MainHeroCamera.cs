using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    public class MainHeroCamera : MonoBehaviour
    {
        [SerializeField] private float loseRotationX;
        private bool _isRotated;
        
        public IEnumerator SmoothlyRotateUp()
        {
            if (_isRotated)
                yield break;

            _isRotated = true;
            
            float timeOfRotation = 1;
            int stepsPerSec = 50;
            float totalRotationDelta = -loseRotationX;
            
            float rotationDelta = totalRotationDelta / (stepsPerSec * timeOfRotation);
            float delayBetweenSteps = 1f / stepsPerSec;
            
            while (transform.localRotation.x > loseRotationX)
            {
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x - rotationDelta, 0, 0);
                yield return new WaitForSeconds(delayBetweenSteps);
            }
        }
    }
}