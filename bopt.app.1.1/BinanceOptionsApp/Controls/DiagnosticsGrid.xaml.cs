using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BinanceOptionsApp.Controls
{
    public class DiagnosticsValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> projection)
        {
            var memberExpression = (MemberExpression)projection.Body;
            OnPropertyChangedExplicit(memberExpression.Member.Name);
        }
        void OnPropertyChangedExplicit(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int _Row;
        public int Row
        {
            get { return _Row; }
            set { if (_Row != value) { _Row = value; OnPropertyChanged(); } }
        }
        private int _Column;
        public int Column
        {
            get { return _Column; }
            set { if (_Column != value) { _Column = value; OnPropertyChanged(); } }
        }
        private string _Format;
        public string Format
        {
            get { return _Format; }
            set { if (_Format != value) { _Format = value; OnPropertyChanged(); } }
        }
        public DiagnosticsValue(int row, int column, string format)
        {
            Row = row;
            Column = column;
            Format = format;
        }
    }

    public class DiagnosticsValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] is string stringFormat && values[0] is IFormattable formattable)
            {
                if (!string.IsNullOrEmpty(stringFormat))
                {
                    return formattable.ToString(stringFormat, CultureInfo.InvariantCulture);
                }
            }
            if (values[0]!=null)
            {
                return values[0].ToString();
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DiagnosticsStringValue : DiagnosticsValue
    {
        public DiagnosticsStringValue(int row, int column)
            : base(row, column, null)
        {
        }
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { if (_Value != value) { _Value = value; OnPropertyChanged(); } }
        }

        public DiagnosticsStringValue SetValue(string value)
        {
            Value = value;
            return this;
        }
        public DiagnosticsStringValue Clone()
        {
            return new DiagnosticsStringValue(Row, Column)
            {
                Value = Value
            };
        }
    }

    public class DiagnosticsDoubleValue : DiagnosticsValue
    {
        public DiagnosticsDoubleValue(int row, int column, string format)
            : base(row, column, format)
        {
        }
        private double? _Value;
        public double? Value
        {
            get { return _Value; }
            set { if (_Value != value) { _Value = value; OnPropertyChanged(); } }
        }
        public DiagnosticsDoubleValue Clone()
        {
            return new DiagnosticsDoubleValue(Row, Column, Format)
            {
                Value = Value
            };
        }
    }

    public class DiagnosticsIntValue : DiagnosticsValue
    {
        public DiagnosticsIntValue(int row, int column, string format)
            : base(row, column, format)
        {
        }
        private int? _Value;
        public int? Value
        {
            get { return _Value; }
            set { if (_Value != value) { _Value = value; OnPropertyChanged(); } }
        }
        public DiagnosticsIntValue Clone()
        {
            return new DiagnosticsIntValue(Row, Column, Format)
            {
                Value = Value
            };
        }
    }

    public class DiagnosticsDateTimeValue : DiagnosticsValue
    {
        public DiagnosticsDateTimeValue(int row, int column, string format)
            : base(row, column, format)
        {
        }
        private DateTime? _Value;
        public DateTime? Value
        {
            get { return _Value; }
            set { if (_Value != value) { _Value = value; OnPropertyChanged(); } }
        }
        public DiagnosticsDateTimeValue Clone()
        {
            return new DiagnosticsDateTimeValue(Row, Column, Format)
            {
                Value = Value
            };
        }
    }

    public partial class DiagnosticsGrid : UserControl
    {
        public DiagnosticsGrid()
        {
            InitializeComponent();
        }

        public ObservableCollection<DiagnosticsValue> Values
        {
            get { return (ObservableCollection<DiagnosticsValue>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(ObservableCollection<DiagnosticsValue>), typeof(DiagnosticsGrid), new PropertyMetadata(new ObservableCollection<DiagnosticsValue>()));

        public int RowsCount
        {
            get { return (int)GetValue(RowsCountProperty); }
            set { SetValue(RowsCountProperty, value); }
        }
        public static readonly DependencyProperty RowsCountProperty =
            DependencyProperty.Register("RowsCount", typeof(int), typeof(DiagnosticsGrid), new PropertyMetadata(0));

        public int ColumnsCount
        {
            get { return (int)GetValue(ColumnsCountProperty); }
            set { SetValue(ColumnsCountProperty, value); }
        }
        public static readonly DependencyProperty ColumnsCountProperty =
            DependencyProperty.Register("ColumnsCount", typeof(int), typeof(DiagnosticsGrid), new PropertyMetadata(0));

    }
}
