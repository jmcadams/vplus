using System.Drawing;

namespace VixenPlus {
    public class GroupTagData {

        /// <summary>
        /// Is this current node a leaf node
        /// </summary>
        public bool IsLeafNode { get; set; }

        /// <summary>
        /// What color should we render the node this tag points to
        /// </summary>
        public Color NodeColor { get; set; }

        /// <summary>
        /// What is the channel this tag points to
        /// </summary>
        public string UnderlyingChannel { get; set; }

        /// <summary>
        /// What is the zoom setting for this tag
        /// </summary>
        public string Zoom { get; set; }

        /// <summary>
        /// Was this originally a SortOrder?
        /// </summary>
        public bool IsSortOrder { get; set; }
    }
}