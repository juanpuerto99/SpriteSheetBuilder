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

namespace SpriteSheetBuilder
{
    /// <summary>
    /// Lógica de interacción para FrameControl.xaml
    /// </summary>
    public partial class FrameControl : UserControl
    {
        private uint dataX;
        private uint dataY;
        private uint dataWidth;
        private uint dataHeight;
        private uint dataId;

        private int imageTotalWidth;
        private int imageTotalHeight;

        public delegate void SettedDataChangedEventHandler(object sender, DataChangedArgs e);
        public event SettedDataChangedEventHandler DataChangedEvent;

        public class DataChangedArgs
        {
            public enum ChangeDataType
            {
                X,
                Y,
                Width,
                Height
            }
            public ChangeDataType changeDataType;

            public DataChangedArgs(ChangeDataType cdt)
            {
                this.changeDataType = cdt;
            }
        }

        public uint SettedX
        {
            get => dataX;
            set
            {
                dataX = value;
                tbxPosX.Text = value.ToString();
            }
        }
        public uint SettedY
        {
            get => dataY;
            set
            {
                dataY = value;
                tbxPosY.Text = value.ToString();
            }
        }
        public uint SettedWidth
        {
            get => dataWidth;
            set
            {
                dataWidth = value;
                tbxWidth.Text = value.ToString();
            }
        }
        public uint SettedHeight
        {
            get => dataHeight;
            set
            {
                dataHeight = value;
                tbxHeight.Text = value.ToString();
            }
        }
        public uint SettedId
        {
            get => dataId;
            set
            {
                dataId = value;
                lblId.Content = "Id: " + dataId;
            }
        }

        public FrameControl(uint id, int imageTotalWidth, int imageTotalHeight)
        {
            InitializeComponent();
            lblId.Content = "Id: " + dataId;

            this.imageTotalWidth = imageTotalWidth;
            this.imageTotalHeight = imageTotalHeight;
        }
        public FrameControl(uint id, uint x, uint y, uint width, uint height, int imageTotalWidth, int imageTotalHeight)
        {
            this.dataId = id;
            this.dataX = x;
            this.dataY = y;
            this.dataWidth = width;
            this.dataHeight = height;

            InitializeComponent();

            lblId.Content = "Id: " + dataId;
            tbxPosX.Text = x.ToString();
            tbxPosY.Text = y.ToString();
            tbxWidth.Text = width.ToString();
            tbxHeight.Text = height.ToString();

            this.imageTotalWidth = imageTotalWidth;
            this.imageTotalHeight = imageTotalHeight;
        }

        private void TbxPosX_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!uint.TryParse(tbxPosX.Text, out uint value))
            {
                tbxPosX.Text = dataX.ToString();
                return;
            }

            if (value + dataWidth > imageTotalWidth)
            {
                tbxPosX.Text = dataX.ToString();
                return;
            }

            dataX = value;
            tbxPosX.Text = dataX.ToString();
            DataChangedEvent.Invoke(this, new DataChangedArgs(DataChangedArgs.ChangeDataType.X));
        }
        private void TbxPosY_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!uint.TryParse(tbxPosY.Text, out uint value))
            {
                tbxPosY.Text = dataY.ToString();
                return;
            }

            if (value + dataHeight > imageTotalHeight)
            {
                tbxPosY.Text = dataY.ToString();
                return;
            }

            dataY = value;
            tbxPosY.Text = dataY.ToString();
            DataChangedEvent.Invoke(this, new DataChangedArgs(DataChangedArgs.ChangeDataType.Y));
        }
        private void TbxWidth_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!uint.TryParse(tbxWidth.Text, out uint value))
            {
                tbxWidth.Text = dataWidth.ToString();
                return;
            }

            if (dataX + value > imageTotalWidth)
            {
                tbxWidth.Text = dataWidth.ToString();
                return;
            }

            dataWidth = value;
            tbxWidth.Text = dataWidth.ToString();
            DataChangedEvent.Invoke(this, new DataChangedArgs(DataChangedArgs.ChangeDataType.Width));
        }
        private void TbxHeight_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!uint.TryParse(tbxHeight.Text, out uint value))
            {
                tbxHeight.Text = dataHeight.ToString();
                return;
            }

            if (dataX + value > imageTotalHeight)
            {
                tbxHeight.Text = dataHeight.ToString();
                return;
            }

            dataHeight = value;
            tbxHeight.Text = dataHeight.ToString();
            DataChangedEvent.Invoke(this, new DataChangedArgs(DataChangedArgs.ChangeDataType.Height));
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
