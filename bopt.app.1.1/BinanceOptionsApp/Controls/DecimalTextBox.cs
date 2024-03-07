using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BinanceOptionsApp.Controls
{
    internal class DecimalTextBox : TextBox
    {
        public DecimalTextBox()
        {
            CommandBinding CutCommandBinding = new CommandBinding(ApplicationCommands.Cut);
            CutCommandBinding.CanExecute += new CanExecuteRoutedEventHandler(CutCommandBinding_CanExecute);
            CutCommandBinding.Executed += new ExecutedRoutedEventHandler(CutCommandBinding_Executed);

            CommandBinding CopyCommandBinding = new CommandBinding(ApplicationCommands.Copy);
            CopyCommandBinding.Executed += new ExecutedRoutedEventHandler(CopyCommandBinding_Executed);
            CopyCommandBinding.CanExecute += new CanExecuteRoutedEventHandler(CopyCommandBinding_CanExecute);

            CommandBinding PasteCommandBinding = new CommandBinding(ApplicationCommands.Paste);
            PasteCommandBinding.Executed += new ExecutedRoutedEventHandler(PasteCommandBinding_Executed);
            PasteCommandBinding.CanExecute += new CanExecuteRoutedEventHandler(PasteCommandBinding_CanExecute);

            CommandBindings.Add(CutCommandBinding);
            CommandBindings.Add(CopyCommandBinding);
            CommandBindings.Add(PasteCommandBinding);

            TextChanged += new TextChangedEventHandler(DecimalTextBox_TextChanged);
            PreviewKeyDown += new KeyEventHandler(DecimalTextBox_PreviewKeyDown);
            PreviewTextInput += new TextCompositionEventHandler(DecimalTextBox_PreviewTextInput);
            MouseDoubleClick += new MouseButtonEventHandler(DecimalTextBox_MouseDoubleClick);

            LostKeyboardFocus += new KeyboardFocusChangedEventHandler(DecimalTextBox_LostKeyboardFocus);
            OnNewDependencyPropertyChanged();
        }

        void DecimalTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SelectAll();
            }
        }

        #region Commands
        void PasteCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        void PasteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }
        void CopyCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(SelectedText);
        }
        void CopyCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                string text = SelectedText.Replace(" ", "");
                Clipboard.SetText(text);
            }
            catch
            {

            }
        }
        void CutCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        void CutCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }
        #endregion

        string freeze_text;
        void OnNewDependencyPropertyChanged()
        {
            if (freeze_text == null)
            {
                string text = ValueToString(Value);
                text = text.RemoveNumericStartAndEnd();
                if (string.IsNullOrEmpty(text)) text = "0";
                if (Text != text) SetNewText(text);
            }
            else
            {
                SetNewText(freeze_text);
            }
        }

        #region ValueProperty
        /// <summary>
        /// Значение
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        "Value",
        typeof(decimal),
        typeof(DecimalTextBox),
        new FrameworkPropertyMetadata(0.0M,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
        ValueChanged, ValueCoerce),
        ValueValidate
        );

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DecimalTextBox control = d as DecimalTextBox;
            control.OnNewDependencyPropertyChanged();
        }

        private static object ValueCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static bool ValueValidate(object value)
        {
            return true;
        }
        /// <summary>
        /// Значение
        /// </summary>
        public decimal Value
        {
            get
            {
                return (decimal)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        #endregion

        #region PowerProperty
        /// <summary>
        /// Степень
        /// </summary>
        public static readonly DependencyProperty PowerProperty = DependencyProperty.Register(
        "Power",
        typeof(int),
        typeof(DecimalTextBox),
        new PropertyMetadata(0, PowerChanged, PowerCoerce),
        PowerValidate
        );

        private static void PowerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DecimalTextBox control = d as DecimalTextBox;
            control.OnNewDependencyPropertyChanged();
        }

        private static object PowerCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static bool PowerValidate(object value)
        {
            return true;
        }
        /// <summary>
        /// Степень
        /// </summary>
        public int Power
        {
            get
            {
                return (int)GetValue(PowerProperty);
            }
            set
            {
                SetValue(PowerProperty, value);
            }
        }

        #endregion

        #region FractionalPartProperty
        /// <summary>
        /// Длина дробной части
        /// </summary>
        public static readonly DependencyProperty FractionalPartProperty = DependencyProperty.Register(
        "FractionalPart",
        typeof(int),
        typeof(DecimalTextBox),
        new PropertyMetadata(1, FractionalPartChanged, FractionalPartCoerce),
        FractionalPartValidate
        );

        private static void FractionalPartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DecimalTextBox control = d as DecimalTextBox;
            control.OnNewDependencyPropertyChanged();
        }

        private static object FractionalPartCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static bool FractionalPartValidate(object value)
        {
            return true;
        }
        /// <summary>
        /// Длина дробной части
        /// </summary>
        public int FractionalPart
        {
            get
            {
                return (int)GetValue(FractionalPartProperty);
            }
            set
            {
                SetValue(FractionalPartProperty, value);
            }
        }

        #endregion

        Key lastKeyDown;

        void DecimalTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
            lastKeyDown = e.Key;
            if (!string.IsNullOrEmpty(Text) && CaretIndex >= 0 && CaretIndex <= Text.Length)
            {
                if (lastKeyDown == Key.Delete)
                {
                    if (CaretIndex < Text.Length)
                    {
                        if (Text[CaretIndex] != ' ') lastKeyDown = Key.None;
                    }
                }
                if (lastKeyDown == Key.Back)
                {
                    if (CaretIndex > 0)
                    {
                        if (Text[CaretIndex - 1] != ' ') lastKeyDown = Key.None;
                    }
                }

            }
        }

        void DecimalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Получение нового текста после ввода символа
            string newTextPreview = Text.Remove(SelectionStart, SelectionLength).Insert(SelectionStart, e.Text);

            // Если не удается преобразовать в число, то не обрабатывать нажатие клавиши
            decimal value = 0;
            if (!StringToValue(newTextPreview, out value))
            {
                e.Handled = true;
                return;
            }

            // Получение текста нового значения с удалением незначащих знаков
            string valueText = ValueToString(value);
            valueText = valueText.RemoveNumericStartAndEnd();

            // Получение текста из нового предполагаемого значения поля ввода без значащих символов
            newTextPreview = MakeVisibleString(newTextPreview);
            newTextPreview = newTextPreview.RemoveNumericStartAndEnd();

            // Если они не равны - то не обрабатывать
            if (valueText != newTextPreview) e.Handled = true;
        }
        void DecimalTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            string txt = Text.Trim();
            txt = txt.RemoveNumericStartAndEnd();

            if (string.IsNullOrEmpty(txt)) txt = "0";
            string newText = MakeVisibleString(txt);
            decimal value;
            if (StringToValue(newText, out value))
            {
                Value = value;
                if (newText != Text) SetNewText(newText);
            }
        }

        void DecimalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = Text;

            if (e.Changes.Count == 1)
            {
                foreach (TextChange tc in e.Changes)
                {
                    if (tc.AddedLength == 0 && tc.RemovedLength == 1)
                    {
                        if (lastKeyDown == Key.Delete && tc.Offset < txt.Length)
                        {
                            txt = txt.Remove(tc.Offset, 1);
                        }
                        if (lastKeyDown == Key.Back && tc.Offset > 0)
                        {
                            txt = txt.Remove(tc.Offset - 1, 1);
                        }
                    }
                }
            }


            string newText = MakeVisibleString(txt); // получение нового текста с учетом форматирования
            decimal value;
            if (StringToValue(newText, out value)) // если удается преобразовать в decimal - установить значение и текст
            {
                freeze_text = newText;
                if (Value != value)
                {
                    Value = value;
                }
                else
                {
                    OnNewDependencyPropertyChanged();
                }
                freeze_text = null;
            }
            else
            {
                // установить текущее значение
                string text = ValueToString(Value);
                if (Text != text) SetNewText(text);
            }
        }
        /// <summary>
        /// Установка нового значение текста с учетом каретки
        /// </summary>
        /// <param name="newText"></param>
        void SetNewText(string newText)
        {
            int caretPos = CaretIndex;
            int oldLength = Text.Length;
            Text = newText;
            int curLength = Text.Length;
            int m_caret_index = caretPos + (curLength - oldLength);
            CaretIndex = m_caret_index > 0 ? m_caret_index : 0;

        }
        private static char[] m_allowed_chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '-' };
        /// <summary>
        /// Из Строки в значение
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="value">Значение</param>
        /// <returns>Успех</returns>
        bool StringToValue(string str, out decimal value)
        {
            str = MakeVisibleString(str);
            str = str.Replace(" ", "");
            if (string.IsNullOrEmpty(str))
            {
                value = 0;
                return true;
            }
            foreach (char c in str)
            {
                if (!m_allowed_chars.Contains(c))
                {
                    value = 0;
                    return false;
                }
            }
            if (str == "-")
            {
                value = 0;
                return true;
            }
            decimal v = 0;
            bool result = decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out v);

            value = v * (decimal)Math.Pow(10.0, Power);
            return result;
        }
        string ValueToString(decimal value)
        {
            decimal mul = (decimal)Math.Pow(10, Power);
            value /= mul;

            int actualFractionalLen = FractionalPart + Power;
            if (actualFractionalLen < 0) actualFractionalLen = 0;

            string format = "F" + actualFractionalLen.ToString();
            string result = value.ToString(format, CultureInfo.InvariantCulture);
            return MakeVisibleString(result);
        }
        /// <summary>
        /// Получение строки с форматированием
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns>Новая строка</returns>
        string MakeVisibleString(string str)
        {
            string result = "";
            str = str.Replace(" ", "");
            str = str.Replace(",", ".");

            bool minus = str.Length > 0 && str[0] == '-';
            if (minus) str = str.Substring(1, str.Length - 1);
            int index = str.LastIndexOf('.');
            if (index < 0) index = str.Length;
            int counter = 0;
            for (int i = index - 1; i >= 0; i--)
            {
                result = result.Insert(0, str[i].ToString());
                counter++;
                if (counter == 3 && i != 0)
                {
                    //result = result.Insert(0, " ");
                    counter = 0;
                }
            }
            if (minus) result = result.Insert(0, "-");
            counter = FractionalPart + Power;
            for (int i = index; i < str.Length; i++)
            {
                if (counter >= 0)
                {
                    result += str[i];
                    counter--;
                }
            }
            return result;
        }
    }
}
