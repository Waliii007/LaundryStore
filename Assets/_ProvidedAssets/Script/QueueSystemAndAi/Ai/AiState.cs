using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaundaryMan
{
    public enum AiState
    {
        EnteringDropQueue,
        WaitingToDropClothes,
        DropClothes,
        EnteringPickingUpQueue,
        WaitingToPickUpClothes,
        GoToHome
    }
}
