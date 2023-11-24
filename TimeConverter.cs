using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheshkaWatchDog
{
    public class TimeConverter
    {

        public static IEnumerable<TimeObject> ConvertToTimeObjects(string timeString)
        {
            var timeObjects = timeString
                .Split(',')
                .Select(time => time.Split(':'))
                .Select(components => new TimeObject
                {
                    Hour = int.Parse(components[0]),
                    Minute = int.Parse(components[1])
                });

            return timeObjects;
        }

    }
}
