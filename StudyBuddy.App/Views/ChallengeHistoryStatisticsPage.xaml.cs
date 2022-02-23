using System.Collections.Generic;
using Entry = Microcharts.ChartEntry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChallengeHistoryStatisticsPage : ContentPage
    {
        List<Entry> entries = new List<Entry>
        {
            new Entry(200)
            {
                Color = SkiaSharp.SKColor.Parse("#FF1493"),
                Label = "January",
                ValueLabel = "200",
            },
            new Entry(300)
            {
                Color = SkiaSharp.SKColor.Parse("#00BFFF"),
                Label = "February",
                ValueLabel = "300",
            },
            new Entry(400)
            {
                Color = SkiaSharp.SKColor.Parse("#00CED1"),
                Label = "March",
                ValueLabel = "400",
            }
        };
        private static SKColor GeneralColor => SKColor.Parse("#407082");

        List <Entry> overallChallengeEntries = new List<Entry>
        {
            new Entry(3)
            {
                Color = GeneralColor,
                Label = "Week1",
                ValueLabel = "3"
            },
            new Entry(5)
            {
                Color = GeneralColor,
                Label = "Week2",
                ValueLabel = "3"
            },
            new Entry(7)
            {
                Color = GeneralColor,
                Label = "Week3",
                ValueLabel = "3"
            },
            new Entry(2)
            {
                Color = GeneralColor,
                Label = "Week4",
                ValueLabel = "3"
            },
            new Entry(3)
            {
                Color = GeneralColor,
                Label = "Week5",
                ValueLabel = "3"
            },
            new Entry(0)
            {
                Color = GeneralColor,
                Label = "Week6",
                ValueLabel = "3"
            },
            new Entry(6)
            {
                Color = SkiaSharp.SKColor.Parse("#407082"),
                Label = "Week7",
                ValueLabel = "3"
            },
            new Entry(8)
            {
                Color = SkiaSharp.SKColor.Parse("#407082"),
                Label = "Week8",
                ValueLabel = "3"
            },

        };



        public ChallengeHistoryStatisticsPage()
        {
            InitializeComponent();

            GeneralChart1.Chart = new Microcharts.LineChart { Entries = overallChallengeEntries };

            exampleChart1.Chart = new Microcharts.RadialGaugeChart { Entries = entries };
            exampleChart2.Chart = new Microcharts.BarChart { Entries = entries };
            exampleChart3.Chart = new Microcharts.LineChart { Entries = entries };
            exampleChart4.Chart = new Microcharts.PieChart { Entries = entries };
            exampleChart5.Chart = new Microcharts.DonutChart { Entries = entries };
            exampleChart6.Chart = new Microcharts.PointChart { Entries = entries };
            exampleChart7.Chart = new Microcharts.RadarChart { Entries = entries };


            
        }

    }
}