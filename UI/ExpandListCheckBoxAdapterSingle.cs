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

        public ExpandListCheckBoxAdapterSingle(Context context, Dictionary<string, List<EnumType>> expandableListDetail) : this(context, expandableListDetail.ToDictionary(d => d.Key, d => (d.Value, default(EnumType?))))
        {
        }

        public ExpandListCheckBoxAdapterSingle(Context context, Dictionary<string, (List<EnumType>, EnumType?)> expandableListDetail) : base(context, expandableListDetail.ToDictionary(d => d.Key, d => d.Value.Item1))
        {
            if (expandableListDetail.Any(d => d.Value.Item1.Distinct().Count() != d.Value.Item1.Count()))
                throw new ArgumentException();

            ListSingleWithSelection = expandableListDetail.ToDictionary(d => d.Key, d => (d.Value.Item1, default(EnumType?)));
            for (int i = 0; i < ListSingleWithSelection.Count; i++)
            {
                EnumType? selectedValue = expandableListDetail.ElementAt(i).Value.Item2 ?? ListSingleWithSelection.ElementAt(i).Value.Item1.First();
                ListSingleWithSelection[(string)GetGroup(i)] = (ListSingleWithSelection.ElementAt(i).Value.Item1, selectedValue);
            }
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