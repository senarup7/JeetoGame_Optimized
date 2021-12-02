using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Updown7.Utility;

namespace Updown7.Gameplay
{
    public class _7updown_InputManager : MonoBehaviour
    {
        public Camera camera;
        public _7updown_Timer timer;
        [SerializeField] _7updown_ChipController chipController;

        bool isTimeUp;
        private void Start()
        {
            isTimeUp = true;
            timer.onTimeUp += () => isTimeUp = true;
            timer.onCountDownStart += () => isTimeUp = false;
        }
        private void Update()
        {
            //if (isTimeUp) return;
            if (Input.GetMouseButtonDown(0))
            {
                ProjectRay();
            }
        }

        void ProjectRay()
        {
            Vector3 origin = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.forward*100);
            if (hit.collider!=null)
            {
                chipController.OnUserInput(hit.transform, hit.point);
            }
        }
    }
}
