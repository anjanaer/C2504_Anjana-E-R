using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _30_Brushes_AndPanels
{
    static class FormConfig
    {
        public static Window FrmSolid { get; set; } = new SolidWindow();
        public static Window FrmLinear{ get;set; } = new LinearGradientWindow();
        public static Window FrmRadient { get; set; } = new RadialGradientWindow();
        public static Window FrmImage { get; set; } = new ImageWindow();
        public static Window FrmVisual { get; set; } = new VisualWindow();
        public static Window FrmStack { get; set; } = new StackWindow();
        public static Window FrmGrid { get; set; } = new GridWindow();
        public static Window FrmWrap { get; set; } = new WrapWindow();
        public static Window FrmCanvas { get; set; } = new CanvasWindow();
        public static Window FrmDock { get; set; } = new DockWindow();
    }

}
