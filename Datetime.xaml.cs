using Microsoft.Maui.Layouts;
using System.Runtime.Intrinsics.Arm;

namespace MobiileApp;

public partial class Datetime : ContentPage
{
    Label lbl;
    DatePicker d;
    TimePicker tp;
    AbsoluteLayout abs;

    public Datetime(int k)
    {
        lbl = new Label()
        {
            BackgroundColor = Color.FromRgb(120, 44, 133),
            Text = "Vali mingi kuupäev või aeg"
        };
        d = new DatePicker()
        {
            MinimumDate = DateTime.Now.AddDays(-10),
            MaximumDate = DateTime.Now.AddDays(10),
            Format = "D"
        };
        d.DateSelected += D_DateSelected;
        tp = new TimePicker()
        {
            Time = new TimeSpan(12, 0, 0)
        };
        tp.PropertyChanged += Tp_PropertyChanged;
        abs = new AbsoluteLayout { Children = { lbl, d, tp } };
        AbsoluteLayout.SetLayoutBounds(lbl, new Rect(10, 10, 200, 50));
        AbsoluteLayout.SetLayoutBounds(d, new Rect(0.2, 0.2, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(d, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(tp, new Rect(0.2, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(tp, AbsoluteLayoutFlags.PositionProportional);
        Content = abs;
    }

    private void Tp_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        lbl.Text = "Oli valitud aeg: " + tp.Time.ToString();
    }

    private void D_DateSelected(object? sender, DateChangedEventArgs e)
    {
        lbl.Text = "Oli valitud aeg: " + e.NewDate.ToString("F");
    }
}