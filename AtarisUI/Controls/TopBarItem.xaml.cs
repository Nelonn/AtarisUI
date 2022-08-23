using System.Windows.Controls;
using System.Windows.Media;

namespace AtarisUI
{
    /// <summary>
    /// Interaction logic for TopBarItem.xaml
    /// </summary>
    public partial class TopBarItem : UserControl
    {
        private IWidget widget;

        public TopBarItem()
        {
            InitializeComponent();
        }

        public TopBarItem(IWidget widget)
        {
            InitializeComponent();
            this.widget = widget;
            AddChild(widget.GetComponent());
        }

        public override void OnApplyTemplate()
        {
            Border border = this.GetTemplateChild("border") as Border;
            border.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            border.MouseEnter += delegate
            {
                border.Background = new SolidColorBrush(Color.FromArgb(15, 255, 255, 255));
            };
            border.MouseLeave += delegate
            {
                border.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            };
            if (widget != null)
            {
                border.MouseLeftButtonDown += delegate
                {
                    widget.MouseLeftButtonDown();
                };
                border.MouseLeftButtonUp += delegate
                {
                    widget.MouseLeftButtonUp();
                };
                border.MouseRightButtonDown += delegate
                {
                    widget.MouseRightButtonDown();
                };
                border.MouseRightButtonUp += delegate
                {
                    widget.MouseRightButtonUp();
                };
            }
            base.OnApplyTemplate();
        }
    }
}
