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
using System.Text.RegularExpressions;

namespace SpriteSheetBuilder.Ventanas
{
    /// <summary>
    /// Lógica de interacción para AñadirFrame.xaml
    /// </summary>
    public partial class AñadirFrame : Window
    {
        private int imageWidth;
        private int imageHeight;

        public uint SettedWidth;
        public uint SettedHeight;
        public uint SettedX;
        public uint SettedY;
        public uint SettedRepeticiones = 1;

        public AñadirFrame(int imageWidth, int imageHeight)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SettedX > 0) tbxY.Focus();
            else tbxX.Focus();
        }
        public void SetPositionX(uint posX)
        {
            this.SettedX = posX;
            tbxX.Text = posX.ToString();
            //tbxX.IsEnabled = false;
        }
        public void SetPositionY(uint posY)
        {
            this.SettedY = posY;
            tbxY.Text = posY.ToString();
            //tbxX.IsEnabled = false;
        }

        private void TbxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TbxTextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizarBotonAceptar();
        }

        private bool ActualizarBotonAceptar()
        {
            bool enabled = true;
            if (!uint.TryParse(tbxRepticiones.Text, out uint repeticiones)) enabled = false;
            if (!uint.TryParse(tbxX.Text, out uint posX)) enabled = false;
            if (!uint.TryParse(tbxY.Text, out uint posY)) enabled = false;
            if (!uint.TryParse(tbxWidth.Text, out uint width)) enabled = false;
            if (!uint.TryParse(tbxHeight.Text, out uint height)) enabled = false;

            if (enabled)
            {
                if (repeticiones == 0) enabled = false;
                if (posX + width * repeticiones > imageWidth) enabled = false;
                if (posY + height > imageHeight) enabled = false;
                if (width == 0) enabled = false;
                if (height == 0) enabled = false;

                if (enabled)
                {
                    SettedRepeticiones = repeticiones;
                    SettedX = posX;
                    SettedY = posY;
                    SettedWidth = width;
                    SettedHeight = height;
                }
            }

            btnAceptar.IsEnabled = enabled;
            return enabled;
        }
        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            if (e.Key == Key.Enter)
            {
                if (ActualizarBotonAceptar())
                    BtnAceptar_Click(sender, new RoutedEventArgs());
            }
        }

        private void TextBoxSelectAll_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }
    }
}
