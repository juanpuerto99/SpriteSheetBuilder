using Microsoft.Win32;
using SpriteSheetBuilder.Controles;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpriteSheetBuilder
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        float zoom = 1f;
        BitmapDecoder bitmapDecoder;
        private int BitmapWidth { get => bitmapDecoder.Frames[0].PixelWidth; }
        private int BitmapHeight { get => bitmapDecoder.Frames[0].PixelHeight; }

        private Dictionary<int, FrameControl> frameControls = new Dictionary<int, FrameControl>();
        private uint FramesCount
        {
            get => General.FramesCount;
            set
            {
                General.FramesCount = value;
                for (int i = 0; i < lbxAnimations.Items.Count; i++)
                    (lbxAnimations.Items[i] as AnimationControl).Update();

                if (!lbxAnimations.IsEnabled && value > 0) lbxAnimations.IsEnabled = true;
                if (!btnAñadirAnimacion.IsEnabled && value > 0) btnAñadirAnimacion.IsEnabled = true;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(imgTextura, BitmapScalingMode.NearestNeighbor);
        }

        private void BtnCargarTextura_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (*.png;*.jpg)|*.png;*.jpg";
            if (!(bool)ofd.ShowDialog()) return;

            bitmapDecoder = BitmapDecoder.Create(new Uri(ofd.FileName, UriKind.Absolute), BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
            General.ImageWidth = BitmapWidth;
            General.ImageHeight = BitmapHeight;

            SetImageSize();

            //btnAñadirAbajo.IsEnabled = true;
            //btnAñadirDerecha.IsEnabled = true;
            //btnAñadirSecDerecha.IsEnabled = true;
            //btnAñadirSecAbajo.IsEnabled = true;

            btnAñadirFrame.IsEnabled = true;
            btnCargar.IsEnabled = true;
            btnGuardar.IsEnabled = true;

            imgTextura.Source = bitmapDecoder.Frames[0];// BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));
        }
        private void BtnCargarXML_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetImageSize()
        {
            double w = BitmapWidth * zoom;
            double h = BitmapHeight * zoom;

            grdTextura.Width = w;
            grdTextura.Height = h;
            imgTextura.Width = w;
            imgTextura.Height = h;

            imgTextura.Margin = new Thickness(grdTextura.Margin.Left, grdTextura.Margin.Top, 0, 0);

            //Labels
            for (int i = 0; i < FramesCount; i++) UpdateLabelZoom(i);
        }

        private void CmbZoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int n = ((ComboBox)sender).SelectedIndex;
            string s = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString();

            if (s == "50%") zoom = 0.5f;
            else if (s == "100%") zoom = 1f;
            else if (s == "200%") zoom = 2f;

            if (bitmapDecoder != null) SetImageSize();
        }

        #region FRAMES
        //Boton Añadir Frame
        private void BtnAñadirFrame_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.AñadirFrame af = new Ventanas.AñadirFrame(BitmapWidth, BitmapHeight);
            if (af.ShowDialog() == true)
            {
                uint x = af.SettedX;
                for (int i = 0; i < af.SettedRepeticiones; i++)
                {
                    AddFrame(x, af.SettedY, af.SettedWidth, af.SettedHeight);
                    x += af.SettedWidth;
                    FramesCount++;
                }
            }
        }
        //Boton Añadir a la Derecha
        private void BtnAñadirDerecha_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.AñadirFrame af = new Ventanas.AñadirFrame(BitmapWidth, BitmapHeight);
            af.SetPositionX(((FrameControl)lbxFrames.SelectedItems[0]).SettedX +
                ((FrameControl)lbxFrames.SelectedItems[0]).SettedWidth);

            if (af.ShowDialog() == true)
            {
                uint x = af.SettedX;
                for (int i = 0; i < af.SettedRepeticiones; i++)
                {
                    AddFrame(x, af.SettedY, af.SettedWidth, af.SettedHeight);
                    x += af.SettedWidth;
                    FramesCount++;
                }
            }
        }
        //Boton añadir Abajo
        private void BtnAñadirAbajo_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.AñadirFrame af = new Ventanas.AñadirFrame(BitmapWidth, BitmapHeight);
            af.SetPositionY(((FrameControl)lbxFrames.SelectedItems[0]).SettedY +
                ((FrameControl)lbxFrames.SelectedItems[0]).SettedHeight);

            if (af.ShowDialog() == true)
            {
                uint x = af.SettedX;
                for (int i = 0; i < af.SettedRepeticiones; i++)
                {
                    AddFrame(x, af.SettedY, af.SettedWidth, af.SettedHeight);
                    x += af.SettedWidth;
                    FramesCount++;
                }
            }
        }

        //Añadir Frame
        private void AddFrame(uint logicX, uint logicY, uint logicWidth, uint logicHeight)
        {
            FrameControl fc = new FrameControl(FramesCount, logicX, logicY, logicWidth, logicHeight, BitmapWidth, BitmapHeight);
            fc.DataChangedEvent += Fc_DataChangedEvent;
            frameControls.Add((int)FramesCount, fc);
            lbxFrames.Items.Add(fc);

            CreateLabel();
        }
        //Quitar Frames
        private void DeleteFrames()
        {

        }

        private void lblFrame_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            int id = Convert.ToInt32(label.Tag.ToString());

            bool controlPressed = Keyboard.IsKeyDown(Key.LeftCtrl);
            if (!controlPressed)
            {
                //selectedFramesIds.Clear();
                selectedFrames.Clear();
            }

            if (controlPressed && General.SelectedFramesId.Contains((uint)id))
            {
                //selectedFramesIds.Remove((uint)id);
                selectedFrames.Remove(lbxFrames.Items[id]);
            }
            else
            {
                //selectedFramesIds.Add((uint)id);
                selectedFrames.Add(lbxFrames.Items[id]);
            }
            //UpdateLabel((int)id);

            lbxFrames.Focus();
        }

        System.Collections.IList selectedFrames = new List<object>();

        //Frame Seleccionado
        private void LbxFrames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for(int i = 0; i < e.RemovedItems.Count; i++)
            {
                uint id = ((FrameControl)e.RemovedItems[i]).SettedId;
                frameLabels[(int)id].Background = labelColorNoSel;
                General.SelectedFramesId.Remove(id);
            }
            for (int i = 0; i < e.AddedItems.Count; i++)
            {
                uint id = ((FrameControl)e.AddedItems[i]).SettedId;
                frameLabels[(int)id].Background = labelColorSel;
                General.SelectedFramesId.Add(id);
            }

            selectedFrames = lbxFrames.SelectedItems;
            General.SelectedFramesControls = selectedFrames.Cast<FrameControl>().ToList();

            //Update animations
            for (int i = 0; i < lbxAnimations.Items.Count; i++)
                (lbxAnimations.Items[i] as AnimationControl).Update();

            if (selectedFrames.Count == 0)
            {
                btnAñadirDerecha.IsEnabled = false;
                btnAñadirAbajo.IsEnabled = false;
            }
            else if (selectedFrames.Count == 1)
            {
                btnAñadirDerecha.IsEnabled = true;
                btnAñadirAbajo.IsEnabled = true;
                btnIncrementar.IsEnabled = true;

                btnSubirFrame.IsEnabled = (selectedFrames[0] as FrameControl).SettedId != 0;
                btnBajarframe.IsEnabled = (selectedFrames[0] as FrameControl).SettedId != FramesCount - 1;
            }
            else
            {
                btnAñadirDerecha.IsEnabled = false;
                btnAñadirAbajo.IsEnabled = false;
                btnIncrementar.IsEnabled = true;

                btnSubirFrame.IsEnabled = false;
                btnBajarframe.IsEnabled = false;
            }
        }

        //Incrementar
        private void BtnIncrementar_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.IncrementarFrame incF = new Ventanas.IncrementarFrame();
            if (incF.ShowDialog() == true)
            {
                for (int i = 0; i < General.SelectedFramesId.Count; i++)
                {
                    General.SelectedFramesControls[i].SettedX = Convert.ToUInt32(General.SelectedFramesControls[i].SettedX + incF.XIncrement);
                    General.SelectedFramesControls[i].SettedY = Convert.ToUInt32(General.SelectedFramesControls[i].SettedY + incF.YIncrement);
                    General.SelectedFramesControls[i].SettedWidth = Convert.ToUInt32(General.SelectedFramesControls[i].SettedWidth + incF.WidthIncrement);
                    General.SelectedFramesControls[i].SettedHeight = Convert.ToUInt32(General.SelectedFramesControls[i].SettedHeight + incF.HeightIncrement);

                    UpdateLabel((int)General.SelectedFramesControls[i].SettedId);
                }
            }
        }

        //Subir Frame
        private void BtnSubirFrame_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)General.SelectedFramesId[0];
            int idArriba = id - 1;

            uint x = frameControls[id].SettedX;
            uint y = frameControls[id].SettedY;
            uint w = frameControls[id].SettedWidth;
            uint h = frameControls[id].SettedHeight;

            //frameControls[id].SettedId = (uint)idArriba;
            frameControls[id].SettedX = frameControls[idArriba].SettedX;
            frameControls[id].SettedY = frameControls[idArriba].SettedY;
            frameControls[id].SettedWidth = frameControls[idArriba].SettedWidth;
            frameControls[id].SettedHeight = frameControls[idArriba].SettedHeight;

            //frameControls[idArriba].SettedId = (uint)id;
            frameControls[idArriba].SettedX = x;
            frameControls[idArriba].SettedY = y;
            frameControls[idArriba].SettedWidth = w;
            frameControls[idArriba].SettedHeight = h;

            UpdateLabel(id);
            UpdateLabel(idArriba);

            lbxFrames.SelectedIndex = lbxFrames.SelectedIndex - 1;
            lbxFrames.Focus();
        }
        //Bajar Frame
        private void BtnBajarframe_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)General.SelectedFramesId[0];
            int idAbajo = id + 1;

            uint x = frameControls[id].SettedX;
            uint y = frameControls[id].SettedY;
            uint w = frameControls[id].SettedWidth;
            uint h = frameControls[id].SettedHeight;

            //frameControls[id].SettedId = (uint)idArriba;
            frameControls[id].SettedX = frameControls[idAbajo].SettedX;
            frameControls[id].SettedY = frameControls[idAbajo].SettedY;
            frameControls[id].SettedWidth = frameControls[idAbajo].SettedWidth;
            frameControls[id].SettedHeight = frameControls[idAbajo].SettedHeight;

            //frameControls[idArriba].SettedId = (uint)id;
            frameControls[idAbajo].SettedX = x;
            frameControls[idAbajo].SettedY = y;
            frameControls[idAbajo].SettedWidth = w;
            frameControls[idAbajo].SettedHeight = h;

            UpdateLabel(id);
            UpdateLabel(idAbajo);

            lbxFrames.SelectedIndex = lbxFrames.SelectedIndex + 1;
            lbxFrames.Focus();
        }

        private void Fc_DataChangedEvent(object sender, FrameControl.DataChangedArgs e)
        {
            FrameControl fc = (FrameControl)sender;
            int id = (int)fc.SettedId;
            UpdateLabel(id);
        }
        #endregion

        #region LABELS
        private Dictionary<int, Label> frameLabels = new Dictionary<int, Label>();
        private SolidColorBrush labelColorNoSel = new SolidColorBrush(new Color() { R = 255, G = 0, B = 0, A = 50 });
        private SolidColorBrush labelColorSel = new SolidColorBrush(new Color() { R = 255, G = 0, B = 0, A = 150 });
        private SolidColorBrush labelColorAnimSel = new SolidColorBrush(new Color() { R = 0, G = 0, B = 255, A = 150 });
        private SolidColorBrush labelBorderSelAnim = new SolidColorBrush(new Color() { R = 0, G = 0, B = 255, A = 255 });
        private SolidColorBrush labelBorderNoSel = new SolidColorBrush(new Color() { R = 255, G = 0, B = 0, A = 255 });
        private SolidColorBrush labelTextColor = new SolidColorBrush(new Color() { R = 255, G = 255, B = 255, A = 255 });

        private float borderSize = 4f;
        private double labelFrameDefaultFontScale = 48;

        private void CreateLabel()
        {
            int id = (int)General.FramesCount;

            Label label = new Label();
            label.Tag = General.FramesCount;
            label.MouseLeftButtonUp += lblFrame_MouseUp;

            frameLabels.Add((int)FramesCount, label);
            grdTextura.Children.Add(label);

            UpdateLabel(id);
        }
        private void UpdateLabel(int id)
        {
            frameLabels[id].Content = id.ToString();
            UpdateLabelZoom(id);
            UpdateLabelAlignaments(id);
            UpdateLabelColor(id);
        }
        private void UpdateLabelZoom(int id)
        {
            frameLabels[id].Margin = new Thickness(
                (grdTextura.Margin.Left + frameControls[id].SettedX) * zoom,
                (grdTextura.Margin.Top + frameControls[id].SettedY) * zoom,
                0, 0);

            frameLabels[id].Width = frameControls[id].SettedWidth * zoom;
            frameLabels[id].Height = frameControls[id].SettedHeight * zoom;

            frameLabels[id].FontSize = labelFrameDefaultFontScale * zoom;
            frameLabels[id].BorderThickness = new Thickness(borderSize * zoom);
        }
        private void UpdateLabelAlignaments(int id)
        {
            frameLabels[id].HorizontalAlignment = HorizontalAlignment.Left;
            frameLabels[id].VerticalAlignment = VerticalAlignment.Top;
            frameLabels[id].HorizontalContentAlignment = HorizontalAlignment.Center;
            frameLabels[id].VerticalContentAlignment = VerticalAlignment.Center;
        }
        private void UpdateLabelColor(int id)
        {
            bool frameSelected = General.SelectedFramesId.Contains((uint)id);
            bool animationSelected = General.SelectedAnimationFramesId.Contains((uint)id);
            bool animationFrameSelected = General.SelectedAnimationSelectedFramesId.Contains((uint)id);

            //Background
            if (animationFrameSelected) frameLabels[id].Background = labelColorAnimSel;
            else if (frameSelected) frameLabels[id].Background = labelColorSel;
            else frameLabels[id].Background = labelColorNoSel;

            //Border
            if (animationSelected) frameLabels[id].BorderBrush = labelBorderSelAnim;
            else frameLabels[id].BorderBrush = labelBorderNoSel;

            //Text color
            frameLabels[id].Foreground = labelTextColor;
        }

        private void ClearLabels()
        {
            for (int i = 0; i < FramesCount; i++)
            {
                grdTextura.Children.Remove(frameLabels[i]);
                frameLabels.Remove(i);
            }
        }
        #endregion
        #region ANIMACIONES
        //Crear Animacion
        private void CreateAnimation(string name, List<AnimationFrameControl> afc)
        {
            AnimationControl ac;
            if (name == null && afc == null)
                ac = new AnimationControl();
            else
                ac = new AnimationControl(name, afc);

            ac.scrollEvent += AnimationPanel_MouseScroll;
            ac.changedFrames += AnimationPanel_ChangedFrames;
            lbxAnimations.Items.Add(ac);
            General.Animations = lbxAnimations.Items.Cast<AnimationControl>().ToList();
        }
        //Añadir Animación
        private void BtnAñadirAnimacion_Click(object sender, RoutedEventArgs e)
        {
            CreateAnimation(null, null);
            lbxAnimations.Focus();
            lbxAnimations.SelectedIndex = lbxAnimations.Items.Count - 1;

            for (int i = 0; i < General.Animations.Count; i++)
                UpdateLabelColor(i);
        }
        //Eliminar Animación
        private void BtnEliminarAnimacion_Click(object sender, RoutedEventArgs e)
        {
            lbxAnimations.Items.RemoveAt(General.SelectedAnimation);
            General.Animations = lbxAnimations.Items.Cast<AnimationControl>().ToList();
            for (int i = 0; i < General.FramesCount; i++) UpdateLabelColor(i);
        }
        //Animación seleccionada
        private void LbxAnimations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            General.SelectedAnimation = lbxAnimations.SelectedIndex;
            AnimationControl animationControl = (AnimationControl)lbxAnimations.SelectedItem;

            General.SelectedAnimationFramesId.Clear();

            if (animationControl != null)
            {
                for (int i = 0; i < animationControl.lbxAnimations.Items.Count; i++)
                    General.SelectedAnimationFramesId.Add((animationControl.lbxAnimations.Items[i] as AnimationFrameControl).SettedFrameId);

                if (General.SelectedAnimation != -1) btnEliminarAnimacion.IsEnabled = true;
                else btnEliminarAnimacion.IsEnabled = false;
            }

            General.SelectedAnimationSelectedFramesId = animationControl.SelectedFrameId;
            for (int i = 0; i < General.FramesCount; i++) UpdateLabelColor(i);
        }
        //Frames de Animacion cambiados
        private void AnimationPanel_ChangedFrames(object sender, EventArgs e)
        {
            AnimationControl ac = (AnimationControl)sender;
            if (General.SelectedAnimation >= 0 && ac == General.Animations[General.SelectedAnimation])
            {
                General.SelectedAnimationFramesId.Clear();
                for (int i = 0; i < ac.lbxAnimations.Items.Count; i++)
                    General.SelectedAnimationFramesId.Add((ac.lbxAnimations.Items[i] as AnimationFrameControl).SettedFrameId);

                General.SelectedAnimationSelectedFramesId = ac.SelectedFrameId;
                for (int i = 0; i < General.FramesCount; i++) UpdateLabelColor(i);
            }
        }
        //Scroll
        private void AnimationPanel_MouseScroll(object sender, MouseWheelEventArgs e)
        {
            DependencyObject o = VisualTreeHelper.GetChild(lbxAnimations, 0);
            DependencyObject o2 = VisualTreeHelper.GetChild(o, 0);
            if (o2 is ScrollViewer)
            {
                ScrollViewer sw = (ScrollViewer)o2;
                sw.ScrollToVerticalOffset(sw.VerticalOffset - e.Delta / 6);
            }
        }
        #endregion

        #region GUARDAR Y CARGAR
        public Engine.AnimationSerialize GenerateXml()
        {
            //Frames
            Engine.SpriteSheetSerialize spriteSheetData = new Engine.SpriteSheetSerialize();
            List<Microsoft.Xna.Framework.Rectangle> rectangles = new List<Microsoft.Xna.Framework.Rectangle>();

            for (int i = 0; i < lbxFrames.Items.Count; i++)
            {
                FrameControl fc = (FrameControl)lbxFrames.Items[i];
                rectangles.Add(new Microsoft.Xna.Framework.Rectangle((int)fc.SettedX, (int)fc.SettedY, (int)fc.SettedWidth, (int)fc.SettedHeight));
            }

            Engine.SpriteSheetData1 ssd1 = new Engine.SpriteSheetData1 { Sprites = rectangles.ToArray() };
            spriteSheetData.SpriteSheetData = ssd1;

            //Anims
            List<Engine.AnimationDataGRL> animData = new List<Engine.AnimationDataGRL>();

            for (int i = 0; i < lbxAnimations.Items.Count; i++)
            {
                AnimationControl ac = (AnimationControl)lbxAnimations.Items[i];
                string name = ac.tbxAnimacion.Text;
                List<Engine.FrameData> frames = new List<Engine.FrameData>();
                for (int j = 0; j < ac.lbxAnimations.Items.Count; j++)
                {
                    AnimationFrameControl afc = (AnimationFrameControl)ac.lbxAnimations.Items[j];
                    Engine.FrameData fd = new Engine.FrameData { FrameId = (int)afc.SettedFrameId, Speed = afc.SettedDuration };
                    frames.Add(fd);
                }

                Engine.AnimationData1 ad = new Engine.AnimationData1 { Name = name, Frames = frames };
                animData.Add(ad);
            }

            //Create serial
            Engine.AnimationSerialize anim = new Engine.AnimationSerialize { SpriteSheetData = spriteSheetData, AnimationData = animData };
            return anim;
        }

        //Guardar
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Animations (*.xml)|*.xml";
            if (!(bool)sfd.ShowDialog()) return;

            Engine.AnimationSerialize anim = GenerateXml();
            Engine.AnimationSerialize.Save(sfd.FileName, anim);
        }
        //Cargar
        private void BtnCargar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Animations (*.xml)|*.xml";
            if (!(bool)ofd.ShowDialog()) return;

            Engine.AnimationSerialize anim = Engine.AnimationSerialize.LoadExtern(ofd.FileName);

            //Frames
            Microsoft.Xna.Framework.Rectangle[] frames = anim.SpriteSheetData.SpriteSheetData.GetRectangleArray();

            ClearLabels();
            lbxFrames.Items.Clear();

            General.FramesCount = 0;
            for (int i = 0; i < frames.Length; i++)
            {
                FrameControl fc = new FrameControl(General.FramesCount, BitmapWidth, BitmapHeight);
                AddFrame((uint)frames[i].X, (uint)frames[i].Y, (uint)frames[i].Width, (uint)frames[i].Height);
                General.FramesCount++;
            }

            //Animations
            lbxAnimations.Items.Clear();
            for (int i = 0; i < anim.AnimationData.Count; i++)
            {
                List<Engine.FrameData> fd = anim.AnimationData[i].GetFrameData();
                List<AnimationFrameControl> frameList = new List<AnimationFrameControl>();
                for (int j = 0; j < fd.Count; j++)
                {
                    AnimationFrameControl afc = new AnimationFrameControl((uint)fd[j].FrameId, fd[j].Speed);
                    frameList.Add(afc);
                }

                CreateAnimation(anim.AnimationData[i].Name, frameList);
            }

            lbxAnimations.IsEnabled = true;
            if (General.FramesCount > 0) btnAñadirAnimacion.IsEnabled = true;
        }
        #endregion
    }
}
