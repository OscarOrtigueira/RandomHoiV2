using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace RandomHoiV2
{
    // An instance of the SortWrapper class is created for
    // each item and added to the ArrayList for sorting.
    public class SortWrapper
    {
        internal ListViewItem sortItem;
        internal int sortColumn;


        // A SortWrapper requires the item and the index of the clicked column.
        public SortWrapper(ListViewItem Item, int iColumn)
        {
            sortItem = Item;
            sortColumn = iColumn;
        }

        // Text property for getting the text of an item.
        public string Text
        {
            get
            {
                return sortItem.SubItems[sortColumn].Text;
            }
        }

        // Implementation of the IComparer
        // interface for sorting ArrayList items.
        public class SortComparer : IComparer
        {
            bool ascending;

            // Constructor requires the sort order;
            // true if ascending, otherwise descending.
            public SortComparer(bool asc)
            {
                this.ascending = asc;
            }

            // Implemnentation of the IComparer:Compare
            // method for comparing two objects.
            public int Compare(object x, object y)
            {
                SortWrapper xItem = (SortWrapper)x;
                SortWrapper yItem = (SortWrapper)y;

                string xText = xItem.sortItem.SubItems[xItem.sortColumn].Text;
                string yText = yItem.sortItem.SubItems[yItem.sortColumn].Text;
                double retnum;
                double xNum = 0;
                double yNum = 0;

                if (Double.TryParse(xText, out retnum) && Double.TryParse(yText, out retnum))
                {
                    return Convert.ToDouble(xText).CompareTo(Convert.ToDouble(yText)) * (this.ascending ? 1 : -1);
                }
                else
                {
                    return xText.CompareTo(yText) * (this.ascending ? 1 : -1);
                }

      
                


                
            }
        }
    }


    // The ColHeader class is a ColumnHeader object with an
    // added property for determining an ascending or descending sort.
    // True specifies an ascending order, false specifies a descending order.
    public class ColHeader : ColumnHeader
    {
        public bool ascending;
        public ColHeader(string text, int width, HorizontalAlignment align, bool asc)
        {
            this.Text = text;
            this.Width = width;
            this.TextAlign = align;
            this.ascending = asc;
        }
    }


}
