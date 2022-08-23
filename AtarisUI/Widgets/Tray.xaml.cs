using AtarisUI.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;

namespace AtarisUI.Widgets
{
    /// <summary>
    /// Interaction logic for Tray.xaml
    /// </summary>
    public partial class Tray : UserControl, IWidget
    {
        public Tray(dynamic settings)
        {
            InitializeComponent();

            //load();
        }

        private void load()
        {
            foreach (AutomationElement button in GetAllAutomationElements(AutomationElement.RootElement))
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = button.Current.Name;
                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                content.Children.Add(textBlock);
            }
        }

        /// <summary>
        /// Returns all children elements of an automation element.
        /// </summary>
        public virtual List<AutomationElement> GetAllAutomationElements(AutomationElement aeElement)
        {
            AutomationElement aeFirstChild = TreeWalker.RawViewWalker.GetFirstChild(aeElement);

            List<AutomationElement> aeList = new List<AutomationElement>();
            aeList.Add(aeFirstChild);
            AutomationElement aeSibling = null;

            int count = 0;
            while ((aeSibling = TreeWalker.RawViewWalker.GetNextSibling(aeList[count])) != null)
            {
                aeList.Add(aeSibling);
                /*Console.WriteLine(aeSibling.Current.HelpText);
                Console.WriteLine(aeSibling.Current.Name);
                Console.WriteLine(aeSibling.Current.ClassName);
                Console.WriteLine(aeSibling.Current.IsControlElement);*/
                /*try
                {
                    foreach (AutomationElement button in GetAllAutomationElements(aeSibling))
                    {
                        Console.WriteLine("++++++++");
                        Console.WriteLine(button.Current.Name);
                        Console.WriteLine("++++++++");
                    }
                } catch (Exception e)
                {

                }*/
                //Console.WriteLine("==================================");
                /*TextBlock textBlock = new TextBlock();
                textBlock.Text = "PIZDA: " + aeSibling.Current.HelpText;
                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                content.Children.Add(textBlock);*/
                count++;
            }

            return aeList;
        }

        public new void MouseLeftButtonDown()
        {

        }

        public new void MouseLeftButtonUp()
        {
            //Process.Start("ms-settings:bluetooth");
            menu.IsOpen = true;
        }

        public new void MouseRightButtonDown()
        {

        }

        public new void MouseRightButtonUp()
        {

        }

        public void Dispose()
        {

        }

        public FrameworkElement GetComponent()
        {
            return this;
        }
    }
}
