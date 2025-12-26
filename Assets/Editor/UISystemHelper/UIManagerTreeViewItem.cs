using UnityEditor.IMGUI.Controls;
namespace Editor
{
    public class UIManagerTreeViewItem : TreeViewItem
    {
        public int Rank;
        public string WindowName;
        public int ComponentNum;
        public string ParentNodeName;
        public long UsedCount;
        public long UsedTimestamp;
        public bool IsActive;
        public bool IsPermanent;

        public UIManagerTreeViewItem()
        {
        
        }
    }
}