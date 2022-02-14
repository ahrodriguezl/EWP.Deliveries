using System;

namespace EWP
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FormTypeAttribute : Attribute
    {
        public string Name { get; set; }

        public FormTypeAttribute(string Name)
        {
            this.Name = Name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SimpleMenuAttribute : Attribute
    {
        public string UniqueID { get; private set; }
        public string Caption { get; private set; }
        public string Image { get; private set; }

        public SimpleMenuAttribute(string UniqueID, string Caption, string Image = null)
        {
            this.UniqueID = UniqueID;
            this.Caption = Caption;
            this.Image = Image;
        }
    }
}
