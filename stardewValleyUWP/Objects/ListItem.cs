using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace stardewValleyUWP.Objects
{
    public class SubButton
    {
        public string Text { get; set; }
        public Action OnClick { get; set; }
        public Vector2 Size { get; set; }
    }

    public class ListItem
    {
        public string Text { get; set; }
        public Action OnClick { get; set; }
        public Vector2 Size { get; set; }
        public List<SubButton> SubButtons { get; set; } = new List<SubButton>();
    }
}