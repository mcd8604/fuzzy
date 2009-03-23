using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Fuzzy.Season
{
    class SeasonManager : ISeasonable
    {
        protected Seasons currentSeason;

        List<ISeasonable> seasonables = new List<ISeasonable>();

        #region ISeasonable Members

        public void ChangeSeason(Seasons newSeason)
        {
            currentSeason = newSeason;

            foreach (ISeasonable s in seasonables)
            {
                s.ChangeSeason(newSeason);
            }
        }

        #endregion
    }
}
