﻿using QLBVCB.ViewModel;
using System.Windows;
using System.Windows.Controls;
namespace QLBVCB.ViewModel
{
    public class SeatTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EconomySeatTemplate { get; set; }
        public DataTemplate EmptySeatTemplate { get; set; }
        public DataTemplate BookedSeatTemplate { get; set; }
        public DataTemplate LabelTemplate { get; set; }
        public DataTemplate PickingSeatTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var seat = item as Seat;
            if (seat == null)
                return base.SelectTemplate(item, container);

            if (seat.IsPicking)
                return PickingSeatTemplate;

            switch (seat.SeatType)
            {
                case "Economy":
                    return EconomySeatTemplate;
                case "Empty":
                    return EmptySeatTemplate;
                case "Booked":
                    return BookedSeatTemplate;
                case "Label":
                    return LabelTemplate;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}