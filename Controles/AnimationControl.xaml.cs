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

namespace SpriteSheetBuilder.Controles
{
    /// <summary>
    /// Lógica de interacción para AnimationControl.xaml
    /// </summary>
    public partial class AnimationControl : UserControl
    {
        public string Name = "";
        public bool FocusedOnListBox = false;
        public event EventHandler FramesCountChanged;

        System.Collections.IList selectedFrames = new List<object>();
        List<uint> selectedFramesIds = new List<uint>();
        List<AnimationFrameControl> animations = new List<AnimationFrameControl>();

        public List<uint> SelectedFrameId
        {
            get
            {
                List<uint> selecteds = new List<uint>();
                for(int i = 0; i < lbxAnimations.SelectedItems.Count; i++)
                    selecteds.Add((lbxAnimations.SelectedItems[i] as AnimationFrameControl).SettedFrameId);
                return selecteds;
            }
        }

        private int AnimationFrameHeight = 36;
        private int AnimationExtraeHeight = 40;
        private int baseHeight = 150;
        private int expanderBaseHeight = 30;

        public MouseWheelEventHandler scrollEvent;
        public EventHandler changedFrames;

        public AnimationControl()
        {
            InitializeComponent();

            string name = "Animacion 1";
            List<string> animationsNames = General.GetAnimationsNames();
            int i = 1;
            while (animationsNames.Contains(name))
            {
                i++;
                name = "Animacion " + i;
            }

            tbxAnimacion.Text = name;
            Name = name;

            Update();
            UpdateControlSize();
        }

        public AnimationControl(string name, List<AnimationFrameControl> animations)
        {
            InitializeComponent();

            tbxAnimacion.Text = name;
            Name = name;

            this.animations = animations;
            for (int i = 0; i < animations.Count; i++)
                lbxAnimations.Items.Add(animations[i]);

            General.Animations.Add(this);

            Update();
            UpdateControlSize();
        }

        //Extender
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            UpdateControlSize();
            btnSubirFrame.IsEnabled = true;
            btnBajarFrame.IsEnabled = true;
        }
        //Collapsar
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            UpdateControlSize();
            btnSubirFrame.IsEnabled = false;
            btnBajarFrame.IsEnabled = false;
        }

        //Actualizar Tamaños
        private void UpdateControlSize()
        {
            if (expAnimaciones.IsExpanded)
            {
                int itemHeight = AnimationFrameHeight * lbxAnimations.Items.Count;
                this.Height = baseHeight + AnimationExtraeHeight + itemHeight;
                this.lbxAnimations.Height = itemHeight + 5 * lbxAnimations.Items.Count;
                expAnimaciones.Height = expanderBaseHeight + lbxAnimations.Height;
            }
            else
            {
                this.Height = baseHeight;
            }
        }

        //Añadir Frame
        private void TbxAñadirFrame_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.AñadirAnimacionFrame aaf = new Ventanas.AñadirAnimacionFrame();
            if (aaf.ShowDialog() == true)
            {
                for (int i = 0; i < aaf.Ids.Count; i++)
                {
                    AnimationFrameControl afc = new AnimationFrameControl(aaf.Ids[i], aaf.Duration);
                    afc.ChangedFrameId += AFC_ChangedFrameId;
                    lbxAnimations.Items.Add(afc);
                    animations.Add(afc);
                }

                changedFrames?.Invoke(this, new EventArgs());
                UpdateControlSize();
            }
        }

        //Añadir Frame Actual
        private void BtnAñadirFrameActual_Click(object sender, RoutedEventArgs e)
        {
            Ventanas.AñadirAnimacionFrame aaf = new Ventanas.AñadirAnimacionFrame(General.SelectedFramesId);
            if (aaf.ShowDialog() == true)
            {
                for (int i = 0; i < aaf.Ids.Count; i++)
                {
                    AnimationFrameControl afc = new AnimationFrameControl(aaf.Ids[i], aaf.Duration);
                    afc.ChangedFrameId += AFC_ChangedFrameId;
                    lbxAnimations.Items.Add(afc);
                    animations.Add(afc);
                }

                changedFrames?.Invoke(this, new EventArgs());
                UpdateControlSize();
            }
        }
        //Eliminar Frames
        private void BtnEliminarFrames_Click(object sender, RoutedEventArgs e)
        {
            while (selectedFrames.Count > 0)
                lbxAnimations.Items.Remove(selectedFrames[0]);

            changedFrames?.Invoke(this, new EventArgs());
            UpdateControlSize();
        }

        //Id Cambiado
        private void AFC_ChangedFrameId(object sender, EventArgs e)
        {
            changedFrames?.Invoke(this, new EventArgs());
        }

        //Cambia seleccionado
        private void LbxAnimations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < e.RemovedItems.Count; i++)
            {
                int id = animations.FindIndex(x => x == e.RemovedItems[i] as AnimationFrameControl);
                selectedFramesIds.Remove((uint)id);
            }
            for (int i = 0; i < e.AddedItems.Count; i++)
            {
                int id = animations.FindIndex(x => x == e.AddedItems[i] as AnimationFrameControl);
                selectedFramesIds.Add((uint)id);
            }

            selectedFrames = lbxAnimations.SelectedItems;
            if (selectedFrames.Count > 0) btnEliminarFrames.IsEnabled = true;
            else btnEliminarFrames.IsEnabled = false;
        }

        //Subir Frame
        private void BtnSubirFrame_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)selectedFramesIds[0];
            int idArriba = id - 1;

            uint sId = animations[id].SettedFrameId;
            float sSpeed = animations[id].SettedDuration;

            //frameControls[id].SettedId = (uint)idArriba;
            animations[id].SettedFrameId = animations[idArriba].SettedFrameId;
            animations[id].SettedDuration = animations[idArriba].SettedDuration;

            //frameControls[idArriba].SettedId = (uint)id;
            animations[idArriba].SettedFrameId = sId;
            animations[idArriba].SettedDuration = sSpeed;

            //UpdateLabel(id);
            //UpdateLabel(idArriba);

            lbxAnimations.SelectedIndex = lbxAnimations.SelectedIndex - 1;
            lbxAnimations.Focus();
        }
        //Bajar Frame
        private void BtnBajarFrame_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)selectedFramesIds[0];
            int idArriba = id + 1;

            uint sId = animations[id].SettedFrameId;
            float sSpeed = animations[id].SettedDuration;

            //frameControls[id].SettedId = (uint)idArriba;
            animations[id].SettedFrameId = animations[idArriba].SettedFrameId;
            animations[id].SettedDuration = animations[idArriba].SettedDuration;

            //frameControls[idArriba].SettedId = (uint)id;
            animations[idArriba].SettedFrameId = sId;
            animations[idArriba].SettedDuration = sSpeed;

            //UpdateLabel(id);
            //UpdateLabel(idArriba);

            lbxAnimations.SelectedIndex = lbxAnimations.SelectedIndex + 1;
            lbxAnimations.Focus();
        }

        public void Update()
        {
            if (General.SelectedFramesId.Count > 0)
            {
                if (btnAñadirFrameActual.IsEnabled == false)
                    btnAñadirFrameActual.IsEnabled = true;
            }
            else
            {
                if (btnAñadirFrameActual.IsEnabled == true)
                    btnAñadirFrameActual.IsEnabled = false;
            }

            //UpdateFrameCount
            for (int i = 0; i < lbxAnimations.Items.Count; i++)
            {
                AnimationFrameControl afc = (lbxAnimations.Items[i] as AnimationFrameControl);
                if (afc.SettedFrameId > General.FramesCount)
                    lbxAnimations.Items.Remove(afc);
            }
        }

        private void LbxAnimations_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
        private void LbxAnimations_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollEvent?.Invoke(sender, e);
        }

        //Nombre Cambiado
        private void TbxAnimacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!tbxAnimacion.IsFocused) return;
            if (ValidName(tbxAnimacion.Text))
                tbxAnimacion.Background = Brushes.Green;
            else
                tbxAnimacion.Background = Brushes.Red;
        }
        //Nombre Lost Focus
        private void TbxAnimacion_LostFocus(object sender, RoutedEventArgs e)
        {
            List<string> animationsNames = General.GetAnimationsNames();
            if (ValidName(tbxAnimacion.Text))
                Name = tbxAnimacion.Text;
            else
                tbxAnimacion.Text = Name;

            tbxAnimacion.Background = Brushes.White;
        }

        private bool ValidName(string name)
        {
            List<string> animationsNames = General.GetAnimationsNames();
            if (tbxAnimacion.Text == string.Empty) return false;
            if (Name != tbxAnimacion.Text && animationsNames.Contains(tbxAnimacion.Text)) return false;
            return true;
        }
    }
}
