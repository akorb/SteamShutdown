using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SteamShutdown
{
    internal class DarkColorTable : ProfessionalColorTable
    {
        private readonly Color backColor = Color.FromArgb(43, 43, 43);
        private readonly Color itemSelectedColor = Color.FromArgb(65, 65, 65);
        private readonly Color borderColor = Color.FromArgb(128, 128, 128);
        private readonly Color separatorColor = Color.FromArgb(128, 128, 128);
        private readonly Color btnPressedColor = Color.FromArgb(110, 160, 230);

        //Border color
        public override Color ToolStripBorder => borderColor;
        public override Color MenuBorder => borderColor;

        //Background color
        public override Color ToolStripDropDownBackground => backColor;
        public override Color MenuStripGradientBegin => backColor;
        public override Color MenuStripGradientEnd => backColor;
        public override Color CheckBackground => backColor;
        public override Color CheckSelectedBackground => backColor;
        public override Color CheckPressedBackground => backColor;
        public override Color ImageMarginGradientBegin => backColor;
        public override Color ImageMarginGradientMiddle => backColor;
        public override Color ImageMarginGradientEnd => backColor;

        //Separator color
        public override Color SeparatorDark => separatorColor;
        public override Color SeparatorLight => separatorColor;

        //MenuItem selected color
        public override Color MenuItemSelected => itemSelectedColor;
        public override Color MenuItemBorder => itemSelectedColor;

        //Item pressed color
        public override Color ButtonPressedBorder => btnPressedColor;
        public override Color ButtonPressedGradientBegin => btnPressedColor;
        public override Color ButtonPressedGradientMiddle => btnPressedColor;
        public override Color ButtonPressedGradientEnd => btnPressedColor;

        //StatusStrip color
        public override Color StatusStripGradientBegin => backColor;
        public override Color StatusStripGradientEnd => backColor;
    }

    /// <summary>
    /// The DarkRenderer is used to render the context menu strip with darkmode friendly styling.
    /// Applying the renderer by using the ToolStrip.Renderer property results in a dark context menu.
    /// </summary>
    public class DarkRenderer : ToolStripProfessionalRenderer
    {
        private readonly Color accentColor;

        public DarkRenderer(Color accentColor) : base(new DarkColorTable())
        {
            this.accentColor = accentColor;
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
            if (e == null)
            {
                base.OnRenderItemCheck(e);
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rectImage = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
            rectImage.X += 4;

            using (var p = new Pen(accentColor, 1))
                e.Graphics.DrawRectangle(p, rectImage);

            rectImage.Width -= 6;
            rectImage.Height -= 8;

            rectImage.X += 3;
            rectImage.Y += 4;

            using (var p = new Pen(Color.White, 1))
            {
                e.Graphics.DrawLines(p, new Point[] {
                    new Point(rectImage.Left, rectImage.Bottom - rectImage.Height / 2),
                    new Point(rectImage.Left + rectImage.Width / 3, rectImage.Bottom),
                    new Point(rectImage.Right, rectImage.Top) });
            }
        }
    }
}
