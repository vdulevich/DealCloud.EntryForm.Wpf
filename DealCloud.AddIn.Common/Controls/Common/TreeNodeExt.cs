using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public class TreeNodeExt : TreeNode
    {
        public TreeNodeExt() : base() { }
        public TreeNodeExt(string text) : base(text) {  }
        public TreeNodeExt(string text, TreeNodeExt[] children) : base(text, children) {  }
        public TreeNodeExt(string text, int imageIndex, int selectedImageIndex) : base(text, imageIndex, selectedImageIndex) {  }
        public TreeNodeExt(string text, int imageIndex, int selectedImageIndex, TreeNodeExt[] children) : base(text, imageIndex, selectedImageIndex, children) {  }
    }
}
