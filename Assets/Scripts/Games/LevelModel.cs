
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Games
{
    public abstract class LevelModel
    {
        // These three instance variables are used to the metrics and have to be
        // initialized in each child class
        // deprecated
        protected int minSeconds;
        protected int pointsPerSecond;
        protected int pointsPerError;


        protected int currentSubLevel;
        protected MetricsTable MetricsTable;

        // This method have to initalize the model
        public abstract void StartGame();
        // This method have to realize the logic to generate a new challenge
        public abstract void NextChallenge();
        // This method have to restart the model
        public abstract void RestartGame();

        public LevelModel() { }

        public int GetPointsPerError(){
             return pointsPerError;
        }

        public int GetPointsPerSecond(){
            return pointsPerSecond;
        }

        public int GetMinSeconds()
        {
            return minSeconds;
        }

        public int GetCurrentSubLevel()
        {
            return currentSubLevel;
        }

        public void SetCurrentSubLevel(int sublevel)
        {          
            currentSubLevel = sublevel;
        }

        public MetricsTable GetMetricsTable()
        {
            return MetricsTable;
        }

    }
}
