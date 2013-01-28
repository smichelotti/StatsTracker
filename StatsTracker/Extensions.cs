using StatsTracker.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTracker
{
    static class Extensions
    {
        public static string GetImageFileName(this Roster roster)
        {
            var lastPart = roster.full_picture_url.Replace("http://cdn.teamsnap.com/roster_full_photos/", null).Replace("/", "-");
            var questionMark = lastPart.IndexOf('?');
            var cacheId = lastPart.Substring(questionMark + 1);
            var fileName = cacheId + "-" + lastPart.Substring(0, questionMark);
            return fileName;
        }

        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector)
        {
            var sortedList = source.OrderBy(keySelector).ToList();
            source.Clear();
            foreach (var sortedItem in sortedList)
            {
                source.Add(sortedItem);
            }
        }
    }
}
