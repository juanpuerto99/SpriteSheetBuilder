using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpriteSheetBuilder.Controles
{
    /// <summary>
    /// Lógica de interacción para AnimationFrameControl.xaml
    /// </summary>
    public partial class AnimationFrameControl : UserControl
    {
        private uint settedFrameId;
        private float settedDuration;
        public uint SettedFrameId
        {
            get => settedFrameId;
            set
            {
                settedFrameId = value;
                tbxId.Text = settedFrameId.ToString();
            }
        }
        public float SettedDuration
        {
            get => settedDuration;
            set
            {
                settedDuration = value;
                tbxVelocidad.Text = settedDuration.ToString();
            }
        }

        public event EventHandler ChangedFrameId;

        //public delegate void SettedDataChangedEventHandler(object sender, DataChangedArgs e);
        //public event SettedDataChangedEventHandler DataChangedEvent;

        //public class IDChangedArgs
        //{
        //    int 
        //}

        public AnimationFrameControl(uint frameId, float duration)
        {
            InitializeComponent();

            settedFrameId = frameId;
            settedDuration = duration;

            tbxId.Text = frameId.ToString(); 
            tbxVelocidad.Text = duration.ToString(); 
        }

        //TextBox Focus
        private void Tbx_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        //ID Preview Texto
        private void TbxId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Regex regex = new Regex("[^0-9]+");
            string originalText = textBox.Text;
            string cuttedText = originalText.Remove(textBox.SelectionStart, textBox.SelectionLength);
            string finalText = cuttedText.Insert(textBox.SelectionStart, e.Text);
            e.Handled = regex.IsMatch(finalText);
        }
        //ID Lost Focus
        private void TbxId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!UInt32.TryParse(tbxId.Text, out uint newId))
            {
                tbxId.Text = settedFrameId.ToString();
                return;
            }
            if (newId > General.FramesCount - 1)
            {
                tbxId.Text = settedFrameId.ToString();
                return;
            }
            if (settedFrameId != newId)
            {
                settedFrameId = newId;
                ChangedFrameId?.Invoke(this, new EventArgs());
            }
        }

        //Velocidad Preview Texto
        private void TbxVelocidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            string originalText = textBox.Text;
            string cuttedText = originalText.Remove(textBox.SelectionStart, textBox.SelectionLength);
            string finalText = cuttedText.Insert(textBox.SelectionStart, e.Text);
            if (finalText[0] == '.' || !regex.IsMatch(finalText))
            {
                e.Handled = true;
                return;
            }
            e.Handled = false;
        }
        //Velocidad Lost Focus
        private void TbxVelocidad_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!UInt32.TryParse(tbxVelocidad.Text, out uint newDuracion))
            {
                tbxVelocidad.Text = settedDuration.ToString();
                return;
            }
            if (newDuracion < 0)
            {
                tbxVelocidad.Text = settedDuration.ToString();
                return;
            }
            settedDuration = newDuracion;
        }
    }
}
