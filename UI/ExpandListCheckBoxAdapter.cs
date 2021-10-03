using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidBase.Attributes;
using AndroidBase.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBase.UI
{
    //TODO usuwa zaznaczenia przy przekręcaniu ekranu
    public abstract class ExpandListCheckBoxAdapter<EnumType, SelectControl> : BaseExpandableListAdapter where EnumType : struct, IConvertible, IComparable, IFormattable where SelectControl : CompoundButton
    {
        private readonly Context Context;
        protected readonly List<View[]> childViews;

        public abstract EnumType GetChildObject(int groupPosition, int childPosition);
        protected abstract int ElementItemResource { get; }
        protected abstract void SelectControlClick(SelectControl selectControl, EnumType key, int groupPosition, int childPosition);
        protected abstract bool GetChecked(EnumType key, int groupPosition, int childPosition);

        public ExpandListCheckBoxAdapter(Context context, Dictionary<string, List<EnumType>> expandableListDetail)
        {
            Context = context;

            childViews = expandableListDetail.Select(d => new View[d.Value.Count]).ToList();
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return GetChildObject(groupPosition, childPosition).GetEnumDescription(Context.Resources);
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override bool HasStableIds => false;

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            TextView textView;
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.list_extensionmodule_group, null);
                textView = convertView.FindViewById<TextView>(Resource.Id.listTitle);
                textView.SetTypeface(null, TypefaceStyle.Bold);
                convertView.Tag = textView;
            }
            else
            {
                textView = (TextView)convertView.Tag;
            }

            textView.Text = (string)GetGroup(groupPosition);

            return convertView;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            //TODO szerokość imageView z argumentów

            SelectControl selectControl;
            ImageView imageView;
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(ElementItemResource, null);
                selectControl = convertView.FindViewById<SelectControl>(Resource.Id.expandedListItem);
                imageView = convertView.FindViewById<ImageView>(Resource.Id.expandedListItemImage);

                selectControl.Click += new EventHandler((object sender, EventArgs e) =>
                {
                    ExpandListCheckBoxPosition groupAndChild = (ExpandListCheckBoxPosition)selectControl.Tag;
                    EnumType key = GetChildObject(groupAndChild.GroupPosition, groupAndChild.ChildPosition);

                    SelectControlClick(selectControl, key, groupAndChild.GroupPosition, groupAndChild.ChildPosition);
                });

                convertView.Tag = Android.Util.Pair.Create(selectControl, imageView);
            }
            else
            {
                Android.Util.Pair pair = (Android.Util.Pair)convertView.Tag;
                selectControl = (SelectControl)pair.First;
                imageView = (ImageView)pair.Second;
            }

            EnumType keyLocal = GetChildObject(groupPosition, childPosition);

            selectControl.Text = (string)GetChild(groupPosition, childPosition);
            selectControl.Tag = new ExpandListCheckBoxPosition(groupPosition, childPosition);
            // shit android
            selectControl.Checked = GetChecked(keyLocal, groupPosition, childPosition);

            ImageAttribute imageAttribute = keyLocal.GetEnumAttribute<EnumType, ImageAttribute>();
            if (imageAttribute != null)
                imageView.SetImageBitmap(imageAttribute.Image(Context.Resources));

            childViews[groupPosition][childPosition] = convertView;
            return convertView;
        }
    }
}