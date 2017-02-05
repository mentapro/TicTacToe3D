using System.Collections.Generic;

namespace TicTacToe3D
{
    public partial class BadgeModel
    {
        public class Registry
        {
            private readonly List<BadgeModel> _badges = new List<BadgeModel>();

            public IEnumerable<BadgeModel> Badges
            {
                get { return _badges; }
            }
            
            public int BadgesCount
            {
                get { return _badges.Count; }
            }
            
            public void AddBadge(BadgeModel badge)
            {
                _badges.Add(badge);
            }
        }
    }
}