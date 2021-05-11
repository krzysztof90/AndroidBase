using System;

namespace AndroidBase.Attributes
{
    public class DescriptionResourceAttribute : Attribute
    {
        public int Resource { get; set; }

        public DescriptionResourceAttribute(int resource)
        {
            Resource = resource;
        }
    }
}