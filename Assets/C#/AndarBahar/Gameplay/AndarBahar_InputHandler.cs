using System;
using UnityEngine;
using UnityEngine.UI;

namespace AndarBahar.Gameplay
{
    class AndarBahar_InputHandler:MonoBehaviour
    {
        public AndarBahar_Timer timer;
         bool isTimeUp;
        public void Start()
        {
            chipController = GetComponent<AndarBahar_ChipController>();

            isTimeUp = false;
            timer.onTimeUp += () => isTimeUp = true;
            timer.onTimerStart += () => isTimeUp = false;
        }
        private void Update()
        {
            if (isTimeUp) return;
            if (Input.GetMouseButtonDown(0))
            {
                ProjectRay();
            }
        }

        public Camera camera;
        AndarBahar_ChipController chipController;
        void ProjectRay()
        {
            Vector3 origin = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.forward * 100);
            if (hit.collider != null)
            {
                print("onclicked");
                chipController.OnUserInput(hit.transform, hit.point);
            }
        }
    }
}
