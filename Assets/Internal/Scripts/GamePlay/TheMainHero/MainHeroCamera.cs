using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    public class MainHeroCamera : MonoBehaviour
    {
        private const float LOOSE_ROTATION_X = -90;
        private bool _isRotated;
        
        public IEnumerator SmoothlyRotateUp()
        {
            if (_isRotated)
                yield break;

            _isRotated = true;
            
            float timeOfRotation = 1;
            int stepsPerSec = 50;
            float totalRotationDelta = LOOSE_ROTATION_X;
            
            float rotationDelta = totalRotationDelta / (stepsPerSec * timeOfRotation);
            float delayBetweenSteps = 1f / stepsPerSec;

            float currentRotation = transform.localRotation.eulerAngles.x;
            while (currentRotation > LOOSE_ROTATION_X)
            {
                currentRotation += rotationDelta;
                transform.localRotation = Quaternion.Euler(currentRotation, 0, 0);
                yield return new WaitForSeconds(delayBetweenSteps);
            }
        }
    }
}