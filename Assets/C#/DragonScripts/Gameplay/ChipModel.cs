using Dragon.Utility;
using Shared;
using UnityEngine;


namespace Dragon.Gameplay
{
    class ChipModel
    {
        [SerializeField] GameObject chip;
        float moveTime;
        iTween.EaseType easeType;
        Chip chipNo;
        int spwanNo;

        public ChipModel(GameObject chip, float moveTime, iTween.EaseType easeType, Chip chipNo, int spwanNo)
        {
            this.chip = chip;
            this.moveTime = moveTime;
            this.easeType = easeType;
            this.chipNo = chipNo;
            this.spwanNo = spwanNo;
        }

        public void MoveChipto(Vector3 target)
        {
            iTween.MoveTo(chip, iTween.Hash("position", target, "time", moveTime, "easetype", easeType));
        }


    }
}