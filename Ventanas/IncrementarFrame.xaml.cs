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

        private bool horizontalCorrect = true;
        private bool verticalCorrect = true;

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
            CheckHorizontal();
        }
        private void TbxY_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!loaded) return;
            CheckVertical();
        }
        private void TbxWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!loaded) return;
            CheckHorizontal();
        }
        private void TbxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!loaded) return;
            CheckVertical();
        }

        private void CheckHorizontal()
        {
            bool correct = true;
            int newX = 0;
            int newW = 0;

            bool incXCorrect = int.TryParse(tbxX.Text, out int incX);
            bool incWCorrect = int.TryParse(tbxWidth.Text, out int incW);

            for (int i = 0; i < General.SelectedFramesControls.Count; i++)
            {
                FrameControl fc = General.SelectedFramesControls[i];

                if (incXCorrect) newX = (int)fc.SettedX + incX;
                if (incWCorrect) newW = (int)fc.SettedWidth + incW;
                if (!incXCorrect || !incWCorrect) break;
                if (newX < 0 || newW <= 0 || newX + newW > General.ImageWidth)
                {
                    correct = false;
                    break;
                }
            }

            if (correct)
            {
                if (incXCorrect) tbxX.Background = correctColor;
                else tbxX.Background = incorrectColor;

                if (incWCorrect) tbxWidth.Background = correctColor;
                else tbxWidth.Background = incorrectColor;

                if (enableFinalValues)
                {
                    if (incXCorrect) lblPosXFinal.Content = newX;
                    else lblPosXFinal.Content = "-";

                    if (incWCorrect) lblWidthFinal.Content = newW;
                    else lblWidthFinal.Content = "-";
                }
            }
            else
            {
                tbxX.Background = incorrectColor;
                tbxWidth.Background = incorrectColor;

                if (enableFinalValues)
                {
                    lblPosXFinal.Content = "-";
                    lblWidthFinal.Content = "-";
                }
            }

            if (correct && incXCorrect && incWCorrect) horizontalCorrect = true;

            if (horizontalCorrect && verticalCorrect) btnAceptar.IsEnabled = true;
            else btnAceptar.IsEnabled = false;
        }
        private void CheckVertical()
        {
            bool correct = true;
            int newY = 0;
            int newH = 0;

            bool incYCorrect = int.TryParse(tbxY.Text, out int incY);
            bool incHCorrect = int.TryParse(tbxHeight.Text, out int incH);

            if (incYCorrect && incHCorrect)
            {
                for (int i = 0; i < General.SelectedFramesControls.Count; i++)
                {
                    FrameControl fc = General.SelectedFramesControls[i];

                    if (incYCorrect) newY = (int)fc.SettedY + incY;
                    if (incHCorrect) newH = (int)fc.SettedHeight + incH;
                    if (!incYCorrect || !incHCorrect) break;
                    if (newY < 0 || newH <= 0 || newY + newH > General.ImageHeight)
                    {
                        correct = false;
                        break;
                    }
                }
            }

            if (correct)
            {
                if (incYCorrect) tbxY.Background = correctColor;
                else tbxWidth.Background = incorrectColor;

                if (incHCorrect) tbxHeight.Background = correctColor;
                else tbxHeight.Background = incorrectColor;

                if (enableFinalValues)
                {
                    if (incYCorrect) lblPosYFinal.Content = newY;
                    else lblPosYFinal.Content = "-";

                    if (incHCorrect) lblHeightFinal.Content = newH;
                    else lblHeightFinal.Content = "-";
                }
            }
            else
            {
                tbxY.Background = incorrectColor;
                tbxHeight.Background = incorrectColor;

                if (enableFinalValues)
                {
                    lblPosYFinal.Content = "-";
                    lblHeightFinal.Content = "-";
                }
            }

            if (correct && incYCorrect && incHCorrect) horizontalCorrect = true;

            if (horizontalCorrect && verticalCorrect) btnAceptar.IsEnabled = true;
            else btnAceptar.IsEnabled = false;
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
        private void TextBoxSelectAll_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }
    }
}
