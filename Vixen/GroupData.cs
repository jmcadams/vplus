using System.Drawing;

namespace VixenPlus {
    /// <summary>
    /// Provides access to GroupData
    /// </summary>
    public class GroupData {

        /// <summary>
        /// Name of the Group
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Color of the Group
        /// </summary>
        public Color GroupColor { get; set; }
    
        /// <summary>
        /// Comma delimited channels for the group, each can be prefixed with ~ to indicate another group
        /// </summary>
        public string GroupChannels { get; set; }
    
        /// <summary>
        /// The zoom level to show when this group is rendered
        /// </summary>
        public string Zoom { get; set; }

        /// <summary>
        /// When sort orders are brought over, this flag is set to repersist as a sort order in Vixen 2.x
        /// </summary>
        public bool IsSortOrder { get; set; }

    }
}