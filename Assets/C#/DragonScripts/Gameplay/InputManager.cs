using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Dragon.Gameplay
{
    public class InputManager : MonoBehaviour
    {
        public Camera camera;

        bool isTimeUp;
        private void Start()
        {
            //Timer.Instance.onTimeUp += () => isTimeUp = true;
            //Timer.Instance.onCountDownStart += () => isTimeUp = false;
        }
        private void Update()
        {
            // if (isTimeUp) return;
            if (Input.GetMouseButtonDown(0))
            {
                ProjectRay();
            }
        }

        public LayerMask bettingPlaceLM;
        void ProjectRay()
        {
            Vector3 origin = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.forward * 100);
            if (hit.collider != null)
            {
                ChipController.Instance.OnUserInput(hit.transform, hit.point);

            }
        }


    }
}