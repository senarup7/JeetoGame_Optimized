
using Shared;

namespace Updown7.Gameplay
{
    interface IChipMovement
    {
        void OnOtherPlayerMove(object data);
        void CreateBotsChips(ChipDate data);
    } 
    interface ITimer
    {
        void OnTimerStart(object data);
        void OnTimeUp(object data);
        void OnWait(object data);
        void OnCurrentTime(object data);
    }

    interface IBot
    {
        void ChipCreator(int dataNo);
    }
}
