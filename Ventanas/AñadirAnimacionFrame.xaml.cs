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
    /// Lógica de interacción para AñadirAnimacionFrame.xaml
    /// </summary>
    public partial class AñadirAnimacionFrame : Window
    {
        public List<uint> Ids { get; private set; } = new List<uint>();
        public float Duration { get; private set; } = 0;

        private bool idCorrect = false;
        private bool durationCorrect = false;

        private SolidColorBrush incorrectColor = Brushes.Red;
        private SolidColorBrush correctColor = Brushes.Green;

        public AñadirAnimacionFrame()
        {
            InitializeComponent();
            tbxId.Focus();
        }
        public AñadirAnimacionFrame(List<uint> initialIds)
        {
            InitializeComponent();
            Ids = initialIds;

            string sIds = string.Empty;
            for (int i = 0; i < initialIds.Count; i++)
            {
                sIds += initialIds[i].ToString();
                if (i + 1 < initialIds.Count) sIds += ";";
            }
            tbxId.Text = sIds;

            tbxDuracion.Focus();
        }

        //Text boxs
        private void TbxId_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] sIds = tbxId.Text.Split(';');
            List<uint> ids = new List<uint>();
            idCorrect = false;

            if (sIds.Length > 0)
            {
                idCorrect = true;
                for (int i = 0; i < sIds.Length; i++)
                {
                    bool currentIdCorrect = false;
                    if (uint.TryParse(sIds[i], out uint id))
                    {
                        if (id < General.FramesCount) currentIdCorrect = true;
                    }

                    if (!currentIdCorrect)
                    {
                        idCorrect = false;
                        break;
                    }
                    else ids.Add(id);
                }
            }

            if (idCorrect)
            {
                tbxId.BorderBrush = correctColor;
                Ids = ids;
            }
            else tbxId.BorderBrush = incorrectColor;

            UpdateAceptarButton();
        }
        private void TbxDuracion_TextChanged(object sender, TextChangedEventArgs e)
        {
            durationCorrect = false;
            if (float.TryParse(tbxDuracion.Text, out float duration))
            {
                if (duration > 0) durationCorrect = true;
            }

            if (durationCorrect)
            {
                tbxDuracion.BorderBrush = correctColor;
                Duration = duration;
                lblDurationFrames.Content = Math.Ceiling(duration / 1000 * (1000f / 60)) + " frames";
            }
            else
            {
                tbxDuracion.BorderBrush = incorrectColor;
                lblDurationFrames.Content = "0 frames";
            }

            UpdateAceptarButton();
        }
        private void TextBoxSelectAll_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        //Cerrar
        public void UpdateAceptarButton()
        {
            if (idCorrect && durationCorrect) btnAceptar.IsEnabled = true;
            else btnAceptar.IsEnabled = false;
        }
        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (!idCorrect || !durationCorrect) return;
            DialogResult = true;
            Close();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            if (e.Key == Key.Enter) BtnAceptar_Click(sender, new RoutedEventArgs());
        }
    }
}
