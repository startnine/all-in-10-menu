using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OriginMenu
{
    public class DoubleToThicknessConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType,
            Object parameter, CultureInfo culture)
        {
            double val = double.Parse(value.ToString());
            Thickness output = new Thickness(0);
            var param = parameter.ToString().ToLower();

            if (param.Contains("left"))
                output.Left = val;

            if (param.Contains("top"))
                output.Top = val;

            if (param.Contains("right"))
                output.Right = val;

            if (param.Contains("bottom"))
                output.Bottom = val;

            return output;
        }

        public Object ConvertBack(Object value, Type targetType,
            Object parameter, CultureInfo culture)
        {
            Thickness val = (Thickness)value;
            var param = parameter.ToString();
            if (param == "Top")
            {
                return val.Top;
            }
            else if (param == "Right")
            {
                return val.Right;
            }
            else if (param == "Bottom")
            {
                return val.Bottom;
            }
            else
            {
                return val.Left;
            }
        }
    }
}
