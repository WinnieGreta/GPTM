using System;
using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class StatisticsDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string[] _ids;

        [Inject] private IStatisticsManager _statisticsManager;

        private void Reset()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Awake()
        {
            _statisticsManager.StatisticsUpdatedEvent += OnStatisticsUpdated;
        }

        private void Start()
        {
            OnStatisticsUpdated();
        }

        private void OnStatisticsUpdated()
        {
            int value = 0;
            foreach (var id in _ids)
            {
                value += _statisticsManager.GetStatistics(id);
            }

            _text.text = value.ToString();
        }
    }
}