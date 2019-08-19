using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rca.Sht85Lib.Helpers
{
    public static class PeriodicMeasureModeExtensions
    {
        public static int GetPeriodTime(this PeriodicMeasureModes mode)
        {
            Attribute[] attributes = mode.GetAttributes();

            RefreshRateAttribute attr = null;

            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].GetType() == typeof(RefreshRateAttribute))
                {
                    attr = (RefreshRateAttribute)attributes[i];
                    break;
                }
            }

            if (attr == null)
                return 0;
            else
                return attr.PeriodTime;
        }

        private static Attribute[] GetAttributes(this PeriodicMeasureModes restartReason)
        {
            var fi = restartReason.GetType().GetField(restartReason.ToString());
            Attribute[] attributes = (Attribute[])fi.GetCustomAttributes(typeof(Attribute), false);

            return attributes;
        }
    }
}
