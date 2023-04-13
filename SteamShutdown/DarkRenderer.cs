using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SteamShutdown
{
    internal class DarkColorTable : ProfessionalColorTable
    {
        private Color _backColor = Color.FromArgb(43, 43, 43),
                      _itemSelectedColor = Color.FromArgb(65, 65, 65),
                      _borderColor = Color.FromArgb(128, 128, 128),
                      _separatorColor = Color.FromArgb(128, 128, 128),
                      _btnPressedColor = Color.FromArgb(110, 160, 230);

        //Border color
        public override Color ToolStripBorder => _borderColor;
        public override Color MenuBorder => _borderColor;

        //Background color
        public override Color ToolStripDropDownBackground => _backColor;
        public override Color MenuStripGradientBegin => _backColor;
        public override Color MenuStripGradientEnd => _backColor;
        public override Color CheckBackground => _backColor;
        public override Color CheckSelectedBackground => _backColor;
        public override Color CheckPressedBackground => _backColor;
        public override Color ImageMarginGradientBegin => _backColor;
        public override Color ImageMarginGradientMiddle => _backColor;
        public override Color ImageMarginGradientEnd => _backColor;

        //Seperator color
        public override Color SeparatorDark => _separatorColor;
        public override Color SeparatorLight => _separatorColor;

        //MenuItem selected color
        public override Color MenuItemSelected => _itemSelectedColor;
        public override Color MenuItemBorder => _itemSelectedColor;

        //Item pressed color
        public override Color ButtonPressedBorder => _btnPressedColor;
        public override Color ButtonPressedGradientBegin => _btnPressedColor;
        public override Color ButtonPressedGradientMiddle => _btnPressedColor;
        public override Color ButtonPressedGradientEnd => _btnPressedColor;

        //StatusStrip color
        public override Color StatusStripGradientBegin => _backColor;
        public override Color StatusStripGradientEnd => _backColor;
    }

    public class DarkRenderer : ToolStripProfessionalRenderer
    {
        private Color _accentColor;

        public DarkRenderer(Color accentColor) : base(new DarkColorTable())
        {
            _accentColor = accentColor;
        }

        //Make sure the text is rendered with white text color
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White;
            base.OnRenderItemText(e);
        }

        //Rendering the arrow for the actions submenu
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //Rectangle for the arrows bounding box, a smaller box for a cleaner menu look
            var r = new Rectangle(e.ArrowRectangle.Location, e.ArrowRectangle.Size);
            r.Inflate(-3, -6);

            //The arrow consists of two lines, defined by 3 points
            e.Graphics.DrawLines(Pens.White, new Point[3] {
                new Point(r.Left, r.Top),
                new Point(r.Right, r.Top + r.Height /2),
                new Point(r.Left, r.Top+ r.Height)
            });
        }

        //Rendering the checkmark and its surrounding box
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            if (e != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var rectImage = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
                rectImage.X += 4;
                //rectImage.Inflate(-1, -1);

                using (var p = new Pen(_accentColor, 1))
                    e.Graphics.DrawRectangle(p, rectImage);

                rectImage.Width -= 6;
                rectImage.Height -= 8;

                rectImage.X += 3;
                rectImage.Y += 4;

                using (var p = new Pen(Color.White, 1))
                {
                    e.Graphics.DrawLines(p, new Point[] { new Point(rectImage.Left, rectImage.Bottom - (int)(rectImage.Height / 2)), new Point(rectImage.Left + (int)(rectImage.Width / 3), rectImage.Bottom), new Point(rectImage.Right, rectImage.Top) });
                }
            }
            else
                base.OnRenderItemCheck(e);
        }
    }
}
