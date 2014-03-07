using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace FolderUI
{
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    public class ButtonSwitchItem : Component
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

        private string _toolTipText;
        public string ToolTipText
        {
            get
            {
                if (String.IsNullOrEmpty(_toolTipText))
                    return Text;

                return _toolTipText;
            }
            set { _toolTipText = value; }
        }

        private object _tag;
        [DefaultValue(null), TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private Image _icon;
        public Image Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        private Image _selectedIcon;
        public Image SelectedIcon
        {
            get { return _selectedIcon; }
            set { _selectedIcon = value; }
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
