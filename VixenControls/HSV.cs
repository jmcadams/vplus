
namespace VixenPlusCommon {
    public class HSV {
        public HSV(float hue = 0f, float saturation = 0f, float value = 0f) {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }


        public float Hue { get; set; }

        public float Saturation { get; set; }

        public float Value { get; set; }


        public void SetToHSV(HSV hsv) {
            Hue = hsv.Hue;
            Saturation = hsv.Saturation;
            Value = hsv.Value;
        }

    }
}