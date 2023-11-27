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

        public IEnumerable<EnumType> SelectedList => ListMultipleWithSelections.SelectMany(d => d.Value).Where(d => d.Value).Select(d => d.Key).Distinct();

        public ExpandListCheckBoxAdapterMultiple(Context context, Dictionary<string, List<EnumType>> expandableListDetail) : this(context, expandableListDetail.ToDictionary(d => d.Key, d => d.Value.Select(l => (l, false)).ToList()))
        {
        }

        public ExpandListCheckBoxAdapterMultiple(Context context, Dictionary<string, List<(EnumType, bool)>> expandableListDetail) : base(context, expandableListDetail.ToDictionary(d => d.Key, d => d.Value.Select(l => l.Item1).ToList()))
        {
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
            KeyValuePair<string, Dictionary<EnumType, bool>> listElement = ListMultipleWithSelections.ElementAt(groupPosition);
            listElement.Value[key] = !listElement.Value[key];

            foreach (KeyValuePair<string, Dictionary<EnumType, bool>> anotherListElement in ListMultipleWithSelections.Where(l => l.Key != listElement.Key && l.Value.ContainsKey(key)))
            {
                anotherListElement.Value[key] = !anotherListElement.Value[key];
                int groupPositionLocal = ListMultipleWithSelections.Keys.ToList().IndexOf(anotherListElement.Key);
                int childPositionLocal = anotherListElement.Value.Keys.ToList().IndexOf(key);
                GetSelectControl(groupPositionLocal, childPositionLocal).Checked = GetChecked(key, groupPositionLocal, childPositionLocal);
            }
        }

        protected override bool GetChecked(EnumType key, int groupPosition, int childPosition)
        {
            return ListMultipleWithSelections.ElementAt(groupPosition).Value.ElementAt(childPosition).Value;
        }
    }
}