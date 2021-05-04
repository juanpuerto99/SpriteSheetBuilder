using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteSheetBuilder
{
    public static class General
    {
        public static uint FramesCount = 0;
        public static int ImageWidth = -1;
        public static int ImageHeight = -1;

        public static List<uint> SelectedFramesId = new List<uint>();
        public static List<FrameControl> SelectedFramesControls = new List<FrameControl>();
        public static List<FrameControl> FramesControls = new List<FrameControl>();

        public static List<Controles.AnimationControl> Animations = new List<Controles.AnimationControl>();
        public static List<uint> SelectedAnimationFramesId = new List<uint>();
        public static List<uint> SelectedAnimationSelectedFramesId = new List<uint>();
        public static int SelectedAnimation = -1;

        public static List<string> GetAnimationsNames()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < Animations.Count; i++)
                names.Add(Animations[i].Name);

            return names;
        }
        public static List<Controles.AnimationFrameControl> GetAnimationFrames(int index)
        {
            if (index < 0 || index > Animations.Count) return new List<Controles.AnimationFrameControl>();
            return Animations[index].lbxAnimations.Items.Cast<Controles.AnimationFrameControl>().ToList();
        }
    }
}
