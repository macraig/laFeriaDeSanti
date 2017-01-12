using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Settings;
using Assets.Scripts.App;

namespace Assets.Scripts.Metrics.View
{

    public class MetricsView : MonoBehaviour
    {

        private static MetricsView metricsView;

        public DetailsView details;
        public ResultsView results;
        [SerializeField]
        private List<Sprite> areaIcons;

        void Awake()
        {
            if (metricsView == null) metricsView = this;
            else if (metricsView != this) Destroy(this);
        }

        void Start()
        {
            ShowResults();
            HideDetails();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (details.isActiveAndEnabled) details.OnClickCrossBtn();            
                else results.OnClickCrossBtn();                          
            }
        }

        private void HideDetails()
        {
            details.gameObject.SetActive(false);
        }               

        internal void ShowDetailsOf(int area, int idGame, int level)
        {
            details.gameObject.SetActive(true);
            //details.ShowDetailsOf(AppController.GetController().GetActivityName(area, idGame),SettingsController.GetController().GetUsername(), MetricsController.GetController().GetMetricsByLevel(area, idGame, level));
            Debug.Log("id: " + idGame);
            details.ShowDetailsOf(AppController.GetController().GetGameName(idGame),SettingsController.GetController().GetUsername(), MetricsController.GetController().GetMetricsByLevel(idGame, level));
        }

        internal void ShowResults()
        {
            results.gameObject.SetActive(true);
        }

        public static MetricsView GetMetricsView()
        {
            return metricsView;
        }

        internal Sprite GetAreaIcon(int area)
        {
            return areaIcons[area];
        }
    }
}