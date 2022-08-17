


using System;

namespace ms
{
    public class ItemDrop : Drop
    {
        public ItemDrop(int oid, int owner, Point_short start, Point_short dest, sbyte type, sbyte mode, int iid, bool pd, Texture icn) : base(oid, owner, start, dest, type, mode, pd)
        {
            this.icon = new Texture(icn);
            this.itemid = iid;
        }

        public override void draw(double viewx, double viewy, float alpha)
        {
            if (!active)
            {
                return;
            }

            Point_short absp = phobj.get_absolute(viewx, viewy, alpha);
            if (icon == null)
                AppDebug.Log($"ItemDrop icon == null");
            //icon?.draw(new DrawArgument(angle.get(alpha), absp, opacity.get(alpha), Constants.get().sortingLayer_ItemDrop, 0));
            icon?.draw(new DrawArgument(angle.get(alpha), absp, opacity.get(alpha)));
        }

        public override void Dispose()
        {
            icon?.Dispose();
            base.Dispose();
        }

        private readonly Texture icon;
        private int itemid;
    }
}