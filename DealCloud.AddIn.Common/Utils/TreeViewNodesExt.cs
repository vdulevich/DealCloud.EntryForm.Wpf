using DealCloud.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Utils
{
    public static class TreeViewNodesExt
    {
        public static List<TreeNode> Filter(this IEnumerable<TreeNode> nodes, string filter)
        {
            List<TreeNode> filteredNodes = new List<TreeNode>();
            Dictionary<TreeNode, TreeNode> mappingNodes = new Dictionary<TreeNode, TreeNode>();
            foreach (TreeNode treeNode in nodes)
            {
                FilterTreeRecursive(treeNode, filter, mappingNodes);
                TreeNode filteredNode = mappingNodes.GetValueOrDefault(treeNode);
                if (filteredNode != null)
                {
                    filteredNodes.Add(filteredNode);
                }
            }
            return filteredNodes;
        }

        private static bool FilterTreeRecursive(TreeNode curentNode, string filter, Dictionary<TreeNode, TreeNode> mappingNodes)
        {
            bool textFound = curentNode.Text.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1;
            foreach (TreeNode node in curentNode.Nodes)
            {
                textFound = FilterTreeRecursive(node, filter, mappingNodes) || textFound;
            }
            if (textFound)
            {
                TreeNode filteredNode = mappingNodes.GetValueOrDefault(curentNode);
                if (filteredNode == null)
                {
                    filteredNode = Clone(curentNode);
                    filteredNode.Nodes.Clear();
                    mappingNodes.Add(curentNode, filteredNode);
                }
                if (curentNode.Parent != null)
                {
                    TreeNode filteredNodeParent = mappingNodes.GetValueOrDefault(curentNode.Parent);
                    if (filteredNodeParent == null)
                    {
                        filteredNodeParent = Clone(curentNode.Parent);
                        filteredNodeParent.Nodes.Clear();
                        mappingNodes.Add(curentNode.Parent, filteredNodeParent);
                    }
                    filteredNodeParent.Nodes.Add(filteredNode);
                }
            }
            return textFound;
        }

        private static TreeNode Clone(TreeNode node)
        {
            return new TreeNode()
            {
                Text = node.Text,
                ForeColor = node.ForeColor,
                Tag = node.Tag,
                ImageIndex = node.ImageIndex,
                SelectedImageIndex = node.SelectedImageIndex,
            };
        }
    }
}
