using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Settings;
using Assets.Scripts.App;
using Assets.Scripts.Sound;
using System;

namespace Assets.Scripts.Metrics.View
{
    public class ResultsView : MonoBehaviour
    {
        public GameObject metricsRowPrefab;
        public Text title;
        public GameObject metricsPanel;
        private List<MetricsRow> viewRows;
        [SerializeField]
        private List<Toggle> areaFilters;
        [SerializeField]
        private Scrollbar scrollbar;


        void Start(){
            UpdateTexts();
            viewRows = new List<MetricsRow>();
            ShowMetrics();
        }     

        private void UpdateTexts()
        {
            switch (SettingsController.GetController().GetLanguage())
            {
                case 0:
                    title.text = "RESULTADOS";
                    areaFilters[0].GetComponentInChildren<Text>().text = "NUMERACIÓN";
                    areaFilters[1].GetComponentInChildren<Text>().text = "GEOMETRÍA";
                    areaFilters[2].GetComponentInChildren<Text>().text = "INGENIO";
                    areaFilters[3].GetComponentInChildren<Text>().text = "DATOS";
                    break;
                default:
                    title.text = "RESULTS";
                    areaFilters[0].GetComponentInChildren<Text>().text = "NUMBERING";
                    areaFilters[1].GetComponentInChildren<Text>().text = "GEOMETRY";
                    areaFilters[2].GetComponentInChildren<Text>().text = "ABILITY";
                    areaFilters[3].GetComponentInChildren<Text>().text = "DATA";
                    break;
            }
        }
        /*
        public void ShowMetrics()
        {
            DestroyOldMetricsRows();
            List<List<List<List<GameMetrics>>>> metrics = MetricsController.GetController().GetMetrics();
            for (int area = 0; area < metrics.Count; area++)
            {
                if(areaFilters[area].isOn) AddMetricsOfArea(metrics, area);
            }
            scrollbar.value = 1;
        }
        */

        public void ShowMetrics()
        {
            DestroyOldMetricsRows();

            List<Game> games = AppController.GetController().GetGames();
  
            List<List<Game>> areaGameList = new List<List<Game>>(4);
            for (int i = 0; i < areaGameList.Capacity; i++)
            {
                areaGameList.Add(new List<Game>());
            }

            for (int i = 0; i < games.Count; i++)
            {
                areaGameList[games[i].GetArea()].Add(games[i]);    
            }

            for (int area = 0; area < areaGameList.Count; area++)
            {
                if (areaFilters[area].isOn) AddMetricsOfArea(areaGameList, area);
            }
            scrollbar.value = 1;
        }


        private void DestroyOldMetricsRows()
        {
            for (int i = 0; i < viewRows.Count; i++)
            {
                Destroy(viewRows[i].gameObject);
            }
            viewRows.Clear();
        }
        /*
        private void AddMetricsOfArea(List<List<List<List<GameMetrics>>>> metrics, int area)
        {
            for (int game = 0; game < metrics[area].Count; game++)
            {
                for (int level = 0; level < metrics[area][game].Count; level++)
                {
                    AddMetricRow(area, game, level);
                }
            }
        }
        */

        private void AddMetricsOfArea(List<List<Game>> gamesByArea, int area)
        {
            for (int game = 0; game < gamesByArea[area].Count; game++)
            {
                for (int level = 0; level < gamesByArea[area][game].GetLevels(); level++)
                {
                    AddMetricRow(gamesByArea[area][game], level);
                }
            }
        }


        private void AddMetricRow(Game game, int level)
        {
            GameMetrics gameMetric = MetricsController.GetController().GetBestMetric(game.GetArea(), game.GetId(), level);
            if(gameMetric == null) return;
            GameObject row = Instantiate(metricsRowPrefab);
            viewRows.Add(row.GetComponent<MetricsRow>());

            if (gameMetric == null)
            {
                row.GetComponent<MetricsRow>().SetScore(0);
                row.GetComponent<MetricsRow>().SetStars(0);
                row.GetComponent<MetricsRow>().SetArea(game.GetArea());
                row.GetComponent<MetricsRow>().SetIndex(game.GetId());
                row.GetComponent<MetricsRow>().DisableViewDetails();

            } else
            {
                row.GetComponent<MetricsRow>().SetScore(gameMetric.GetScore());
                row.GetComponent<MetricsRow>().SetStars(gameMetric.GetStars());
                row.GetComponent<MetricsRow>().SetArea(gameMetric.GetArea());
                row.GetComponent<MetricsRow>().SetIndex(gameMetric.GetIndex());
            }
           // row.GetComponent<MetricsRow>().SetActivity(AppController.GetController().GetActivityName(area, game));
            row.GetComponent<MetricsRow>().SetActivity(game.GetNames()[SettingsController.GetController().GetLanguage()]);


            row.GetComponent<MetricsRow>().SetLevel(level);
            row.GetComponent<MetricsRow>().SetIcon(game.GetIcon());


            FitRowToPanel(row);
        }

        private void RemoveMetricsOfArea(int area)
        {
            for (int i = 0; i < viewRows.Count; i++)
            {
                if (viewRows[i].GetArea() == i) Destroy(viewRows[i].gameObject);
            }
        }

        private void FitRowToPanel(GameObject child)
        {
            child.transform.SetParent(metricsPanel.transform, true);
            child.transform.localPosition = Vector3.zero;
            child.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            child.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            child.transform.localScale = Vector3.one;
        }

        public void OnClickCrossBtn(){
            PlaySoundClick();
            ViewController.GetController().LoadMainMenu();
        }     

        public void PlaySoundClick()
        {
            SoundController.GetController().PlayClickSound();
        }
            
    }
}