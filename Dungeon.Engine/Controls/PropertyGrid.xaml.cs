using Dungeon.Engine.Editable;
using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Engine.Events;
using Dungeon.Utils;
using Dungeon.Utils.ReflectionExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dungeon.Engine.Controls
{
    /// <summary>
    /// Interaction logic for PropertyGrid.xaml
    /// </summary>
    public partial class PropertyGrid : UserControl
    {
        public PropertyGrid()
        {
            InitializeComponent();
        }

        public void Init(Action<string> report)
        {
            Report = report;
        }

        private Action<string> Report;
        private object PropGridObject;
        private IPropertyTable PropGridTable;
        private List<(string Key, Func<string> Value, int index)> PropGridBinding;

        private void ClearPropGrid()
        {
            PropGridSaveBtn.IsEnabled = false;
            PropGrid.Children.Clear();
            PropGrid.RowDefinitions.Clear();
            PropGridObject = default;
            PropGridTable = default;
        }

        public void FillPropGrid(PropGridFillEvent @event)
        {
            var Obj = @event.Target;

            ClearPropGrid();
            PropGridBinding = new List<(string Key, Func<string> Value, int index)>();
            PropGridObject = Obj;

            int i = 0;
            CreatePropGridCategory(Obj.GetType().Name, true, i++);

            var grayBrush = new SolidColorBrush(Color.FromRgb(128, 128, 128));

            foreach (var prop in Obj.GetType().GetProperties())
            {
                var hidden = Attribute.GetCustomAttributes(prop)
                    .FirstOrDefault(x => x.GetType() == typeof(HiddenAttribute)) != default;
                if (hidden)
                    continue;

                if (!prop.CanWrite)
                    continue;

                var displ = Attribute.GetCustomAttributes(prop)
                    .FirstOrDefault(x => x.GetType() == typeof(DisplayAttribute)).As<DisplayAttribute>();

                CreatePropGridCell(prop.Name, prop.GetValue(Obj), prop.PropertyType, i, default, displ?.Name, displ?.Description);

                i++;
            }

            PropGridSaveBtn.IsEnabled = true;
        }

        public void FillPropGrid(IPropertyTable propertyTable, string commonTitle = "Основные")
        {
            PropGridBinding = new List<(string Key, Func<string> Value, int index)>();

            ClearPropGrid();
            PropGridTable = propertyTable;

            int i = 0;

            CreatePropGridCategory(commonTitle, true, i++);

            foreach (var prop in propertyTable.Properties)
            {
                if (prop.Name.Contains("Constructor"))
                {
                    CreatePropGridCategory("Конструктор", true, i++);
                    CreatePropGridCell("Использовать", prop.Value, typeof(bool), i, prop.Name);
                }
                else
                {
                    CreatePropGridCell(prop.Name, prop.Value, prop.Type, i);
                }
                i++;
            }

            PropGridSaveBtn.IsEnabled = true;
        }

        private void CreatePropGridCategory(string name, bool first, int rowNum)
        {
            PropGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

            var label = new Label()
            {
                Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(0, (first ? 0 : 1), 1, 1),
                FontWeight = FontWeights.Bold
            };

            var textBlock = new TextBlock()
            {
                Text = name,
                Margin = new Thickness(10, 0, 0, 0)
            };

            label.Content = textBlock;

            Grid.SetRow(label, rowNum);
            Grid.SetColumn(label, 0);
            Grid.SetColumnSpan(label, 2);

            PropGrid.Children.Add(label);
        }

        private void CreatePropGridCell(string name, object value, Type type, int rowNum, string nameBinding = default, string display = default, string description = default)
        {
            PropGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

            var label = new Label()
            {
                Content = display ?? name,
                Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                BorderBrush = Brushes.Gray,
                Background = Brushes.DarkGray,
                BorderThickness = new Thickness(1, 0, 1, 1),
                Margin = new Thickness(10, 0, 0, 0)
            };
            Grid.SetRow(label, rowNum);
            Grid.SetColumn(label, 0);

            if (description != default)
            {
                label.ToolTip = description;
            }

            var editor = new Border()
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(0, 0, 0, 1)
            };
            Grid.SetRow(editor, rowNum);
            Grid.SetColumn(editor, 1);

            if (type == typeof(bool))
            {
                var comboBoxEditor = new ComboBox();
                comboBoxEditor.Items.Add("True");
                comboBoxEditor.Items.Add("False");
                comboBoxEditor.SelectedIndex = value.As<bool>() ? 0 : 1;
                editor.Child = comboBoxEditor;
                PropGridBinding.Add((nameBinding ?? name, () => comboBoxEditor.Text, rowNum - 1));

                if (description != default)
                {
                    comboBoxEditor.ToolTip = description;
                }
            }
            else
            {
                var textEditor = new TextBox() { Text = value?.ToString() ?? "" };
                editor.Child = textEditor;
                PropGridBinding.Add((nameBinding ?? name, () => textEditor.Text, rowNum - 1));

                if (description != default)
                {
                    textEditor.ToolTip = description;
                }
            }

            PropGrid.Children.Add(label);
            PropGrid.Children.Add(editor);
        }

        private void SavePropGrid(object sender, RoutedEventArgs e)
        {
            if (PropGridObject == default && PropGridTable == default)
                return;

            List<string> failedProps = new List<string>();

            foreach (var propBind in PropGridBinding)
            {
                if (PropGridObject != default)
                {
                    if (!PropGridObject.SetPropertyExprConverted(propBind.Key, propBind.Value()))
                    {
                        failedProps.Add(propBind.Key);
                    }
                }
                else
                {
                    var type = PropGridTable.Get(propBind.Key).Type;
                    if (type == default)
                        continue;
                    object data = default;
                    try
                    {
                        data = Convert.ChangeType(propBind.Value(), type);
                    }
                    catch (Exception)
                    {
                        data = type.GetDefault();
                        failedProps.Add(propBind.Key);
                    }

                    PropGridTable.Set(propBind.Key, data, type, PropGridBinding.IndexOf(propBind));
                }
            }

            var status = "Cохранено.";

            if (failedProps.Count != 0)
            {
                status += " Не удалось сохранить свойства: " + string.Join(" | ", failedProps);
            }

            if (PropGridObject is IEditable editable)
            {
                editable.Commit();
            }
            if (PropGridTable != default)
            {
                PropGridTable.Commit();
            }

            Report?.Invoke(status);
        }

        private void ClearPropGridManually(object sender, RoutedEventArgs e)
        {
            ClearPropGrid();
        }
    }
}