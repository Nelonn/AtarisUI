using System.Windows;

namespace AtarisUI
{
    public interface IWidget
    {
        void MouseLeftButtonDown();

        void MouseLeftButtonUp();

        void MouseRightButtonDown();

        void MouseRightButtonUp();

        void Dispose();

        FrameworkElement GetComponent();
    }
}
