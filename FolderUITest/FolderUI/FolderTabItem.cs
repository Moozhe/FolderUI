using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace FolderUI
{
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    public class FolderTabItem : Component
    {
        private string _name;
        [Browsable(false)]
        public string Name
        {
            get
            {
                if (base.Site != null)
                {
                    _name = base.Site.Name;
                }

                return _name;
            }
            set { _name = value; }
        }

        private string _text;
        [DefaultValue("")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private object _tag;
        [DefaultValue(null), TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private GraphicsPath _path = new GraphicsPath();
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GraphicsPath Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public override string ToString()
        {
            if (!String.IsNullOrEmpty(Name))
                return Name;
            else
                return base.ToString();
        }
    }
}
