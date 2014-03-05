﻿namespace Quiche.Settings
{
    public class BuilderSettings
    {
        private static readonly BuilderSettings Defaults = new BuilderSettings
        {
            FieldCasing = FieldCasing.Default,
        };

        internal static BuilderSettings DefaultSettings
        {
            get { return Defaults; }
        }

        public FieldCasing FieldCasing { get; set; }
    }
}