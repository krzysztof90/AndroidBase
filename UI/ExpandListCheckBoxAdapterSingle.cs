using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBase.UI
{
    public class ExpandListCheckBoxAdapterSingle<EnumType> : ExpandListCheckBoxAdapter<EnumType, RadioButton> where EnumType : struct, IConvertible, IComparable, IFormattable
    {
        protected readonly Dictionary<string, (List<EnumType>, EnumType?)> ListSingleWithSelection;

        public Dictionary<string, EnumType> SelectedList => ListSingleWithSelection.ToDictionary(d => d.Key, d => (EnumType)d.Value.Item2);

        public ExpandListCheckBoxAdapterSingle(Context context, Dictionary<string, List<EnumType>> expandableListDetail) : base(context, expandableListDetail)
        {
            if (expandableListDetail.Any(d => d.Value.Distinct().Count() != d.Value.Count()))
                throw new ArgumentException();

            ListSingleWithSelection = expandableListDetail.ToDictionary(d => d.Key, d => (d.Value, default(EnumType?)));
            for (int i = 0; i < ListSingleWithSelection.Count; i++)
                ListSingleWithSelection[(string)GetGroup(i)] = (ListSingleWithSelection.ElementAt(0).Value.Item1, ListSingleWithSelection.ElementAt(0).Value.Item1.First());
        }

        public override EnumType GetChildObject(int groupPosition, int childPosition)
        {
            return ListSingleWithSelection.ElementAt(groupPosition).Value.Item1[childPosition];
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return ListSingleWithSelection.ElementAt(groupPosition).Value.Item1.Count;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return ListSingleWithSelection.ElementAt(groupPosition).Key;
        }

        public override int GroupCount => ListSingleWithSelection.Count;

        protected override int ElementItemResource => Resource.Layout.list_extensionmodule_item_radiobuttons;

        protected override void SelectControlClick(RadioButton selectControl, EnumType key, int groupPosition, int childPosition)
        {
            ListSingleWithSelection[(string)GetGroup(groupPosition)] = (ListSingleWithSelection.ElementAt(groupPosition).Value.Item1, key);

            for (int j = 0; j < this.GetChildrenCount(groupPosition); j++)
            {
                if (j != childPosition)
                {
                    View listItem = childViews[groupPosition][j];
                    Android.Util.Pair pair2 = (Android.Util.Pair)listItem.Tag;
                    RadioButton radioButton3 = (RadioButton)pair2.First;

                    radioButton3.Checked = false;
                }
            }
        }

        protected override bool GetChecked(EnumType key, int groupPosition, int childPosition)
        {
            return key.CompareTo(ListSingleWithSelection.ElementAt(groupPosition).Value.Item2) == 0;
        }
    }
}