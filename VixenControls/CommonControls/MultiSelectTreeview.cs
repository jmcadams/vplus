using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CommonControls {
    public class MultiSelectTreeview : TreeView {

        private readonly List<TreeNode> _selectedNodes;
        private Rectangle _nodeRect;

        private TreeNode _selectedNode;

        public List<TreeNode> SelectedNodes {
            get { return _selectedNodes; }
        }

        // Note we use the new keyword to Hide the native treeview's SelectedNode property.
        public new TreeNode SelectedNode {
            get { return _selectedNode; }
            set {
                ClearSelectedNodes();
                if (value != null) {
                    SelectNode(value);
                }
            }
        }


        public MultiSelectTreeview() {
            _nodeRect = new Rectangle(0, 0, 0, 0);
            _selectedNodes = new List<TreeNode>();
            base.SelectedNode = null;
        }


        protected override void OnGotFocus(EventArgs e) {
            // Make sure at least one node has a selection
            // this way we can tab to the ctrl and use the 
            // keyboard to select nodes
            if (_selectedNode == null && TopNode != null) {
                ToggleNode(TopNode, true);
            }

            base.OnGotFocus(e);
        }


        protected override void OnMouseDown(MouseEventArgs e) {
            // If the user clicks on a node that was not
            // previously selected, select it now.

            base.SelectedNode = null;

            var node = GetNodeAt(e.Location);
            if (node != null) {
                if ((ModifierKeys == Keys.None) && (_selectedNodes.Contains(node))) {
                    // Potential Drag Operation
                    // Let Mouse Up do select
                }
                else {
                    SelectNode(node);
                }
            }

            base.OnMouseDown(e);
        }


        protected override void OnMouseUp(MouseEventArgs e) {
            // If the user clicked on a node that WAS previously
            // selected then, reselect it now. This will clear
            // any other selected nodes. e.g. A B C D are selected
            // the user clicks on B, now A C & D are no longer selected.
            // Check to see if a node was clicked on 
            var node = GetNodeAt(e.Location);
            if (node != null) {
                if ((ModifierKeys == Keys.None) && _selectedNodes.Contains(node)) {
                    SelectNode(node);
                }
            }

            base.OnMouseUp(e);
        }


        protected override void OnItemDrag(ItemDragEventArgs e) {
            // If the user drags a node and the node being dragged is NOT
            // selected, then clear the active selection, select the
            // node being dragged and drag it. Otherwise if the node being
            // dragged is selected, drag the entire selection.
            var node = e.Item as TreeNode;

            if (node != null) {
                if (!_selectedNodes.Contains(node)) {
                    SelectSingleNode(node);
                    ToggleNode(node, true);
                }
            }

            base.OnItemDrag(e);
        }


        protected override void OnBeforeSelect(TreeViewCancelEventArgs e) {
            // Never allow base.SelectedNode to be set!
            base.SelectedNode = null;
            e.Cancel = true;

            base.OnBeforeSelect(e);

        }


        protected override void OnAfterSelect(TreeViewEventArgs e) {
            // Never allow base.SelectedNode to be set!
            base.OnAfterSelect(e);
            base.SelectedNode = null;
        }


        protected override void OnKeyDown(KeyEventArgs e) {
            // Handle all possible key strokes for the control.
            // including navigation, selection, etc.

            base.OnKeyDown(e);

            if (e.KeyCode == Keys.ShiftKey) {
                return;
            }

            var bShift = (ModifierKeys & Keys.Shift) == Keys.Shift;

            // Nothing is selected in the tree, this isn't a good state
            // select the top node
            if (_selectedNode == null && TopNode != null) {
                ToggleNode(TopNode, true);
            }

            // Nothing is still selected in the tree, this isn't a good state, leave.
            if (_selectedNode == null) {
                return;
            }

            switch (e.KeyCode) {
                case Keys.Left:
                    if (_selectedNode.IsExpanded && _selectedNode.Nodes.Count > 0) {
                        // Collapse an expanded node that has children
                        _selectedNode.Collapse();
                    }
                    else if (_selectedNode.Parent != null) {
                        // Node is already collapsed, try to select its parent.
                        SelectSingleNode(_selectedNode.Parent);
                    }
                    break;
                case Keys.Right:
                    if (!_selectedNode.IsExpanded) {
                        // Expand a collpased node's children
                        _selectedNode.Expand();
                    }
                    else {
                        // Node was already expanded, select the first child
                        SelectSingleNode(_selectedNode.FirstNode);
                    }
                    break;
                case Keys.Up:
                    if (_selectedNode.PrevVisibleNode != null) {
                        SelectNode(_selectedNode.PrevVisibleNode);
                    }
                    break;
                case Keys.Down:
                    if (_selectedNode.NextVisibleNode != null) {
                        SelectNode(_selectedNode.NextVisibleNode);
                    }
                    break;
                case Keys.Home:
                    if (bShift) {
                        if (_selectedNode.Parent == null) {
                            // Select all of the root nodes up to this point 
                            if (Nodes.Count > 0) {
                                SelectNode(Nodes[0]);
                            }
                        }
                        else {
                            // Select all of the nodes up to this point under this nodes parent
                            SelectNode(_selectedNode.Parent.FirstNode);
                        }
                    }
                    else {
                        // Select this first node in the tree
                        if (Nodes.Count > 0) {
                            SelectSingleNode(Nodes[0]);
                        }
                    }
                    break;
                case Keys.End:
                    if (bShift) {
                        if (_selectedNode.Parent == null) {
                            // Select the last ROOT node in the tree
                            if (Nodes.Count > 0) {
                                SelectNode(Nodes[Nodes.Count - 1]);
                            }
                        }
                        else {
                            // Select the last node in this branch
                            SelectNode(_selectedNode.Parent.LastNode);
                        }
                    }
                    else {
                        if (Nodes.Count <= 0) {
                            return;
                        }
                        // Select the last node visible node in the tree.
                        // Don't expand branches incase the tree is virtual
                        var ndLast = Nodes[0].LastNode;
                        while (ndLast.IsExpanded && (ndLast.LastNode != null)) {
                            ndLast = ndLast.LastNode;
                        }
                        SelectSingleNode(ndLast);
                    }
                    break;
                case Keys.PageUp: {
                    // Select the highest node in the display
                    var nCount = VisibleCount;
                    var ndCurrent = _selectedNode;
                    while ((nCount) > 0 && (ndCurrent.PrevVisibleNode != null)) {
                        ndCurrent = ndCurrent.PrevVisibleNode;
                        nCount--;
                    }
                    SelectSingleNode(ndCurrent);
                }
                    break;
                case Keys.PageDown: {
                    // Select the lowest node in the display
                    var nCount = VisibleCount;
                    var ndCurrent = _selectedNode;
                    while ((nCount) > 0 && (ndCurrent.NextVisibleNode != null)) {
                        ndCurrent = ndCurrent.NextVisibleNode;
                        nCount--;
                    }
                    SelectSingleNode(ndCurrent);
                }
                    break;
                default: {
                    // Assume this is a search character a-z, A-Z, 0-9, etc.
                    // Select the first node after the current node that 
                    // starts with this character
                    var sSearch = ((char) e.KeyValue).ToString(CultureInfo.InvariantCulture);

                    var ndCurrent = _selectedNode;
                    while ((ndCurrent.NextVisibleNode != null)) {
                        ndCurrent = ndCurrent.NextVisibleNode;
                        if (!ndCurrent.Text.StartsWith(sSearch)) {
                            continue;
                        }
                        SelectSingleNode(ndCurrent);
                        break;
                    }
                }
                    break;
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        private void SelectNode(TreeNode node) {
            if (_selectedNode == null || (ModifierKeys & Keys.Control) == Keys.Control) {
                // Ctrl+Click selects an unselected node, or unselects a selected node.
                var isSelected = _selectedNodes.Contains(node);
                ToggleNode(node, !isSelected);
            }
            else if ((ModifierKeys & Keys.Shift) == Keys.Shift) {
                // Shift+Click selects nodes between the selected node and here.
                var ndStart = _selectedNode;
                var ndEnd = node;

                if (ndStart.Parent == ndEnd.Parent) {
                    // Selected node and clicked node have same parent, easy case.
                    if (ndStart.Index < ndEnd.Index) {
                        // If the selected node is beneath the clicked node walk down
                        // selecting each Visible node until we reach the end.
                        while (ndStart != ndEnd) {
                            ndStart = ndStart.NextVisibleNode;
                            if (ndStart == null) {
                                break;
                            }
                            ToggleNode(ndStart, true);
                        }
                    }
                    else if (ndStart.Index == ndEnd.Index) {
                        // Clicked same node, do nothing
                    }
                    else {
                        // If the selected node is above the clicked node walk up
                        // selecting each Visible node until we reach the end.
                        while (ndStart != ndEnd) {
                            ndStart = ndStart.PrevVisibleNode;
                            if (ndStart == null) {
                                break;
                            }
                            ToggleNode(ndStart, true);
                        }
                    }
                }
                else {
                    // Selected node and clicked node have same parent, hard case.
                    // We need to find a common parent to determine if we need
                    // to walk down selecting, or walk up selecting.

                    var ndStartP = ndStart;
                    var ndEndP = ndEnd;
                    var startDepth = Math.Min(ndStartP.Level, ndEndP.Level);

                    // Bring lower node up to common depth
                    while (ndStartP.Level > startDepth) {
                        ndStartP = ndStartP.Parent;
                    }

                    // Bring lower node up to common depth
                    while (ndEndP.Level > startDepth) {
                        ndEndP = ndEndP.Parent;
                    }

                    // Walk up the tree until we find the common parent
                    while (ndStartP.Parent != ndEndP.Parent) {
                        ndStartP = ndStartP.Parent;
                        ndEndP = ndEndP.Parent;
                    }

                    // Select the node
                    if (ndStartP.Index < ndEndP.Index) {
                        // If the selected node is beneath the clicked node walk down
                        // selecting each Visible node until we reach the end.
                        while (ndStart != ndEnd) {
                            ndStart = ndStart.NextVisibleNode;
                            if (ndStart == null) {
                                break;
                            }
                            ToggleNode(ndStart, true);
                        }
                    }
                    else if (ndStartP.Index == ndEndP.Index) {
                        if (ndStart.Level < ndEnd.Level) {
                            while (ndStart != ndEnd) {
                                ndStart = ndStart.NextVisibleNode;
                                if (ndStart == null) {
                                    break;
                                }
                                ToggleNode(ndStart, true);
                            }
                        }
                        else {
                            while (ndStart != ndEnd) {
                                ndStart = ndStart.PrevVisibleNode;
                                if (ndStart == null) {
                                    break;
                                }
                                ToggleNode(ndStart, true);
                            }
                        }
                    }
                    else {
                        // If the selected node is above the clicked node walk up
                        // selecting each Visible node until we reach the end.
                        while (ndStart != ndEnd) {
                            ndStart = ndStart.PrevVisibleNode;
                            if (ndStart == null) {
                                break;
                            }
                            ToggleNode(ndStart, true);
                        }
                    }
                }
            }
            else {
                // Just clicked a node, select it
                SelectSingleNode(node);
            }

            OnAfterSelect(new TreeViewEventArgs(_selectedNode));
        }


        private void ClearSelectedNodes() {
            foreach (var node in _selectedNodes) {
                InvalidateNode(node);
            }
            _selectedNodes.Clear();
            _selectedNode = null;
        }


        private void SelectSingleNode(TreeNode node) {
            if (node == null) {
                return;
            }

            ClearSelectedNodes();
            ToggleNode(node, true);
            node.EnsureVisible();
        }


        private void ToggleNode(TreeNode node, bool bSelectNode) {
            if (bSelectNode) {
                _selectedNode = node;
                if (!_selectedNodes.Contains(node)) {
                    _selectedNodes.Add(node);
                    node.EnsureVisible();
                }
            }
            else {
                _selectedNodes.Remove(node);
            }
            InvalidateNode(node);
        }


        public void InvalidateNode(TreeNode node) {
            _nodeRect.Location = node.Bounds.Location;
            _nodeRect.Width = Width;
            _nodeRect.Height = ItemHeight;
            Invalidate(_nodeRect);
        }
    }
}
