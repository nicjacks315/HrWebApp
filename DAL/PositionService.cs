using System;
using System.Data.Entity;
using System.Linq;
using CIS.HR.Models;

namespace CIS.HR.DAL
{
    public interface IPositionService
    {
        PositionDescription GetPositionDescription(int positionId, DateTime? asOfDate = null);
        int CreatePositionDescription(int positionId, string title, DateTime dateEffective);
        int CreatePosition(string code, string title, DateTime? dateEffective = null);
    }

    public class PositionService : Service, IPositionService
    {
        #region cctors
        public PositionService(Context context) : base(context)
        {
            _context.Entry(DefaultPositionDescription).State = EntityState.Detached;
            _context.Entry(DefaultPosition).State = EntityState.Detached;
            _context.Entry(DefaultPositionAssignment).State = EntityState.Detached;
        }

        static PositionService()
        {
            //empty position description
            DefaultPositionDescription = new PositionDescription()
            {
                Id = 0,
                Title = "No Effective Title",
                DateEffective = DateTime.MinValue
            };
            
            //empty position
            DefaultPosition = new Position()
            {
                Id = 0,
                Code = "N/A"
            };
            DefaultPosition.PositionDescriptionHistory.Add(DefaultPositionDescription);

            //empty position assignment
            DefaultPositionAssignment = new PositionAssignment()
            {
                Id = 0,
                Position = DefaultPosition,
                DateEffective = DateTime.MinValue,
                DateAsPrimary = DateTime.MinValue,
                DateStarted = DateTime.MinValue,
            };
        }
        #endregion

        #region methods

        //return the position description as of a specific date
        public PositionDescription GetPositionDescription(int positionId, DateTime? asOfDate = null)
        {
            Position position = _context.Positions.Find(positionId) ?? DefaultPosition;
            DateTime testDate = ((asOfDate == null) ? DateTime.Today : asOfDate.Value);
            return position.PositionDescriptionHistory
                .Where(a => a.DateEffective <= testDate)
                .OrderByDescending(b => b.DateEffective).FirstOrDefault();
        }

        //add a new description to a position
        public int CreatePositionDescription(int positionId, string title, DateTime dateEffective )
        {
            Position position = _context.Positions.Find(positionId);
            PositionDescription positionDescription = new PositionDescription()
            {
                Title = title,
                DateEffective = dateEffective
            };
            position.PositionDescriptionHistory.Add(positionDescription);
            return positionDescription.Id;
        }

        //add a new position with position description
        public int CreatePosition(string code, string title, DateTime? dateEffective = null)
        {
            Position position = new Position(){
                Code = code
            };
            position.PositionDescriptionHistory.Add(new PositionDescription() {
                Title = title,
                DateEffective = dateEffective ?? DateTime.MinValue,
            });
            _context.Positions.Add(position);
            return position.Id;
        }
        #endregion

        #region fields
        public static Position DefaultPosition { get; set; }
        public static PositionDescription DefaultPositionDescription { get; set; }
        public static PositionAssignment DefaultPositionAssignment { get; set; }
        #endregion
    }
}
