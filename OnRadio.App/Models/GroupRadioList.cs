using System.Collections.Generic;
using System.Collections.ObjectModel;
using OnRadio.BL.Models;

namespace OnRadio.App.Models
{
    public class GroupRadioList : ObservableCollection<RadioModel>
    {
        public GroupRadioList(IEnumerable<RadioModel> collection) 
            : base(collection)
        {
        }

        public GroupRadioList()
        {
        }

        public GroupType Type { get; set; }

    }

    public enum GroupType
    {
        Others,
        Favorited,
        RecentlyPlayed
    }
}