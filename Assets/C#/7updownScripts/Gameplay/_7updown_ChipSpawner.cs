﻿using UpDown7.Utility;
using System.Collections.Generic;
using UnityEngine;
using Shared;
namespace Updown7.Gameplay
{
    class _7updown_ChipSpawner : MonoBehaviour
    {
        [SerializeField] GameObject[] chips;
        [SerializeField] Transform[] spawnPostions;

        Dictionary<Chip, GameObject> chipContainer =
            new Dictionary<Chip, GameObject>();


        int chipOrderInLayer = 10;
        _7updown_Timer timer;

        public void Start()
        {
            timer = GetComponent<_7updown_Timer>();
            chipContainer.Add(Chip.Chip10, chips[0]);
            chipContainer.Add(Chip.Chip50, chips[1]);
            chipContainer.Add(Chip.Chip100, chips[2]);
            chipContainer.Add(Chip.Chip500, chips[3]);
            chipContainer.Add(Chip.Chip1000, chips[4]);
            chipContainer.Add(Chip.Chip5000, chips[5]);
            //reset the value
            timer.onTimeUp += () => chipOrderInLayer = 10;
            
        }
        public GameObject Spawn(int positinIndex, Chip chipType, Transform parent)
        {
            var chip = Instantiate(chipContainer[chipType], parent);
            chip.GetComponent<SpriteRenderer>().sortingOrder = chipOrderInLayer++;
            chip.SetActive(true);
            chip.transform.position = spawnPostions[positinIndex].position;
            return chip;
        }

    }
}
