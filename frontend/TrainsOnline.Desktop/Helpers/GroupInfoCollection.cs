namespace TrainsOnline.Desktop.ViewModels.Route
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class GroupInfoCollection<T> : ObservableCollection<T>
    {
        public object Key { get; set; }

        public new IEnumerator<T> GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }

    //// Filtering implementation using LINQ
    //public enum FilterOptions
    //{
    //    All = -1,
    //    Rank_Low = 0,
    //    Rank_High = 1,
    //    Height_Low = 2,
    //    Height_High = 3
    //}

    //public ObservableCollection<DataGridDataItem> FilterData(FilterOptions filterBy)
    //{
    //    switch (filterBy)
    //    {
    //        case FilterOptions.All:
    //            return new ObservableCollection<DataGridDataItem>(_items);

    //        case FilterOptions.Rank_Low:
    //            return new ObservableCollection<DataGridDataItem>(from item in _items
    //                                                              where item.Rank < 50
    //                                                              select item);

    //        case FilterOptions.Rank_High:
    //            return new ObservableCollection<DataGridDataItem>(from item in _items
    //                                                              where item.Rank > 50
    //                                                              select item);

    //        case FilterOptions.Height_High:
    //            return new ObservableCollection<DataGridDataItem>(from item in _items
    //                                                              where item.Height_m > 8000
    //                                                              select item);

    //        case FilterOptions.Height_Low:
    //            return new ObservableCollection<DataGridDataItem>(from item in _items
    //                                                              where item.Height_m < 8000
    //                                                              select item);
    //    }

    //    return _items;
    //}

}
