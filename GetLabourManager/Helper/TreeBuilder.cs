using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Helper
{
    public class TreeBuilder
    {
        public int id { get; set; }
        public string text { get; set; }
        public TreeState state { get; set; }
        public List<TreeNode> children { get; set; }
    }

    public class TreeParent
    {
        public int id { get; set; }
        public string text { get; set; }
        public bool expanded { get; set; }
        public List<TreeNodes> items { get; set; }
    }
    public class TreeNodes
    {
        public string id { get; set; }
        public string text { get; set; }
        public string tag { get; set; }
    }
    public class TreeNode
    {
        public int id { get; set; }
        public string text { get; set; }
    }
    public class TreeState
    {
        public bool opened { get; set; }
    }
}