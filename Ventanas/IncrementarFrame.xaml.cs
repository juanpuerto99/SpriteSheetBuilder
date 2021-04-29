using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpriteSheetBuilder.Ventanas
{
    /// <summary>
    /// Lógica de interacción para IncrementarFrame.xaml
    /// </summary>
    public partial class IncrementarFrame : Window
    {
        private int xIncrement = 0;
        private int yIncrement = 0;
        private int widthIncrement = 0;
        private int heightIncrement = 0;

        private bool enableFinalValues;
        private bool loaded;

        private SolidColorBrush correctColor = Brushes.Green;
        private SolidColorBrush incorrectColor = Brushes.Red;

        public int XIncrement { get => xIncrement; }
        public int YIncrement { get => yIncrement; }
        public int WidthIncrement { get => widthIncrement; }
        public int HeightIncrement { get => heightIncrement; }

        public IncrementarFrame()
        {
            InitializeComponent();
            loaded = true;
            if (General.SelectedFramesId.Count == 1)
            {
                enableFinalValues = true;

                lblPosXFinal.Visibility = Visibility.Visible;
                lblPosYFinal.Visibility = Visibility.Visible;
                lblWidthFinal.Visibility = Visibility.Visible;
                lblHeightFinal.Visibility = Visibility.Visible;

                lblPosXFinal.Content = General.SelectedFramesControls[0].SettedX;
                lblPosYFinal.Content = General.SelectedFramesControls[0].SettedY;
                lblWidthFinal.Content = General.SelectedFramesControls[0].SettedWidth;
                lblHeightFinal.Content = General.SelectedFramesControls[0].SettedHeight;
            }
            else
            {
                enableFinalValues = false;

                lblPosXFinal.Visibility = Visibility.Hidden;
                lblPosYFinal.Visibility = Visibility.Hidden;
                lblWidthFinal.Visibility = Visibility.Hidden;
                lblHeightFinal.Visibility = Visibility.Hidden;
            }
        }

        private void TbxX_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!loaded) return;

            int newX = 0;
            int newW = 0;
            bool correct = int.TryParse(tbxX.Text, out int x);

            if (correct)
            {
                for (int i = 0; i < General.SelectedFramesControls.Count; i++)
                {
                    FrameControl fc = General.SelectedFramesControls[i];
                    newX = (int)fc.SettedX + x;
                    newW = (int)fc.SettedWidth + widthIncrement;
                    if (newX + newW > General.ImageWidth || newX < 0)
                    {
                        correct = false;
                        break;
                    }
                }
            }

            if (enableFinalValues)
                lblPosXFinal.Content = newX;

            if (correct)
            {
                xIncrement = x;
                tbxX.Background = correctColor;

                if (enableFinalValues)
                    lblPosXFinal.Foreground = correctColor;

                btnAceptar.IsEnabled = true;
            }
            else
            {
                tbxX.Background = incorrectColor;

                if (enableFinalValues)
                    lblPosXFinal.Foreground = incorrectColor;

                btnAceptar.IsEnabled = false;
            }
        }
        private void TbxY_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!loaded) return;

            int newY = 0;
            int newH = 0;
            bool correct = int.TryParse(tbxY.Text, out int y);

            if (correct)
            {
                for (int i = 0; i < General.SelectedFramesControls.Count; i++)
                {
                    FrameControl fc = General.SelectedFramesControls[i];
                    newY = (int)fc.SettedY + y;
                    newH = (int)fc.SettedHeight + heightIncrement;
                    if (newY + newH > General.ImageHeight || newY < 0)
                    {
                        correct = false;
                        break;
                    }
                }
            }

            if (enableFinalValues)
                lblPosYFinal.Content = newY;

            if (correct)
            {
                yIncrement = y;
                tbxY.Background = correctColor;

                if (enableFinalValues)
                    lblPosYFinal.Foreground = correctColor;

                btnAceptar.IsEnabled = true;
            }
            else
            {
                tbxY.Background = incorrectColor;

                if (enableFinalValues)
                    lblPosYFinal.Foreground = correctColor;

                btnAceptar.IsEnabled = false;
            }
        }
        private void TbxWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!loaded) return;

            int newX = 0;
            int newW = 0;
            bool correct = int.TryParse(tbxWidth.Text, out int w);

            if (correct)
            {
                for (int i = 0; i < General.SelectedFramesControls.Count; i++)
                {
                    FrameControl fc = General.SelectedFramesControls[i];
                    newX = (int)fc.SettedX + xIncrement;
                    newW = (int)fc.SettedWidth + w;
                    if (newX + newW > General.ImageWidth || newW <= 0)
                    {
                        correct = false;
                        break;
                    }
                }
            }

            if (enableFinalValues)
                lblWidthFinal.Content = newW;

            if (correct)
            {
                widthIncrement = w;
                tbxWidth.Background = correctColor;

                if (enableFinalValues)
                    lblWidthFinal.Foreground = correctColor;

                btnAceptar.IsEnabled = true;
            }
            else
            {
                tbxWidth.Background = incorrectColor;

                if (enableFinalValues)
                    lblWidthFinal.Foreground = incorrectColor;

                btnAceptar.IsEnabled = false;
            }
        }
        private void TbxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!loaded) return;

            int newY = 0;
            int newH = 0;
            bool correct = int.TryParse(tbxY.Text, out int h);

            if (correct)
            {
                for (int i = 0; i < General.SelectedFramesControls.Count; i++)
                {
                    FrameControl fc = General.SelectedFramesControls[i];
                    newY = (int)fc.SettedY + yIncrement;
                    newH = (int)fc.SettedHeight + h;
                    if (newY + newH > General.ImageHeight || newH <= 0)
                    {
                        correct = false;
                        break;
                    }
                }
            }

            if (enableFinalValues)
                lblHeightFinal.Content = newH;

            if (correct)
            {
                heightIncrement = h;
                tbxHeight.Background = correctColor;

                if (enableFinalValues)
                    lblHeightFinal.Foreground = correctColor;

                btnAceptar.IsEnabled = true;
            }
            else
            {
                tbxHeight.Background = incorrectColor;

                if (enableFinalValues)
                    lblHeightFinal.Foreground = correctColor;

                btnAceptar.IsEnabled = false;
            }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            if (e.Key == Key.Enter && btnAceptar.IsEnabled) BtnAceptar_Click(sender, new RoutedEventArgs());
        }
    }
}
