using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBase.UI
{
    public class ExpandListCheckBoxAdapterMultiple<EnumType> : ExpandListCheckBoxAdapter<EnumType, CheckBox> where EnumType : struct, IConvertible, IComparable, IFormattable
    {
        protected readonly Dictionary<string, Dictionary<EnumType, bool>> ListMultipleWithSelections;

        public IEnumerable<EnumType> SelectedList => ListMultipleWithSelections.SelectMany(d => d.Value).Where(d => d.Value).Select(d => d.Key);

        public ExpandListCheckBoxAdapterMultiple(Context context, Dictionary<string, List<EnumType>> expandableListDetail) : this(context, expandableListDetail.ToDictionary(d => d.Key, d => d.Value.Select(l => (l, false)).ToList()))
        {
        }

        public ExpandListCheckBoxAdapterMultiple(Context context, Dictionary<string, List<(EnumType, bool)>> expandableListDetail) : base(context, expandableListDetail.ToDictionary(d => d.Key, d => d.Value.Select(l => l.Item1).ToList()))
        {
            if (expandableListDetail.SelectMany(d => d.Value).Distinct().Count() != expandableListDetail.SelectMany(d => d.Value).Count())
                throw new ArgumentException();

            ListMultipleWithSelections = expandableListDetail.ToDictionary(d => d.Key, d => d.Value.ToDictionary(l => l.Item1, l => l.Item2));
        }

        public override EnumType GetChildObject(int groupPosition, int childPosition)
        {
            return ListMultipleWithSelections.ElementAt(groupPosition).Value.ElementAt(childPosition).Key;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return ListMultipleWithSelections.ElementAt(groupPosition).Value.Count;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return ListMultipleWithSelections.ElementAt(groupPosition).Key;
        }

        public override int GroupCount => ListMultipleWithSelections.Count;

        protected override int ElementItemResource => Resource.Layout.list_extensionmodule_item;

        protected override void SelectControlClick(CheckBox selectControl, EnumType key, int groupPosition, int childPosition)
        {
            ListMultipleWithSelections.ElementAt(groupPosition).Value[key] = !ListMultipleWithSelections.ElementAt(groupPosition).Value[key];
        }

        protected override bool GetChecked(EnumType key, int groupPosition, int childPosition)
        {
            return ListMultipleWithSelections.ElementAt(groupPosition).Value.ElementAt(childPosition).Value;
        }
    }
}