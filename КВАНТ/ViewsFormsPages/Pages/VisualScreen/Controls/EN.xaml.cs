﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using КВАНТ.Logical.ProgrammEngine.array_l_element;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    /// <summary>
    /// Interaction logic for EN.xaml
    /// </summary>
    public partial class EN : UserControl, IControlVisual
    {
        private Logical.ProgrammEngine.array_l_element.EN En;
        public event MouseButtonEventHandler ModuleOnDown;
        public event MouseButtonEventHandler ModuleOnUp;
        public event EventHandler DeleteElement;

        public bool SetEdit
        {
            set
            {
                LabelImage.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                MenuItem1.IsChecked = value;
            }
        }
        public EN(object element, Point posPoint, bool b)
        {
            InitializeComponent();
            En = (Logical.ProgrammEngine.array_l_element.EN)element;
            DataContext =((IContentData)element).Content;
            VElement = new VisualElement { X = En.X, Y = En.Y };
            Position = posPoint;
           
            if ((Uslovie)En.Content.MiscObject[0] == Uslovie.Bolshe)
                Button1.Content = "<";
            else
                Button1.Content = ">";

            Helper1.SetBinding(Helper.CaptionTextProperty, new Binding("Name") { Source = ((IContentData)element).Content });
            Helper1.SetBinding(Helper.EnableBlinkProperty, new Binding("BlockExec1Bool") { Source = ((IContentData)element).Content });

            if (b)
            {
                LabelImage.Visibility = Visibility;
                MenuItem1.IsChecked = true;
            }
        }

        public Point Position
        {
            get
            {
                return new Point(this.Margin.Left, this.Margin.Top);
            }
            set
            {
                var m = this.Margin;
                m.Top = value.Y;
                m.Left = value.X;
                this.Margin = m;
                VElement.Position = new Point((int)value.X, (int)value.Y);
            }
        }

        public VisualElement VElement { get; set; }

        #region Перемещение и контекстное меню
        private void MenuItem_Click_Move(object sender, RoutedEventArgs e)
        {
            var a = (MenuItem)sender;
            LabelImage.Visibility = a.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            En.Dialog();
        }

        private void MenuItem_Click_Remove(object sender, RoutedEventArgs e)
        {
            if (DeleteElement != null)
                DeleteElement(this, null);
        }

        private void LabelImage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ModuleOnDown != null)
                ModuleOnDown(this, e);
        }

        private void LabelImage_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ModuleOnUp != null)
                ModuleOnUp(this, e);
        }
        #endregion

        // Пользователь захотел поменять знак
        private void ClickButton(object sender, RoutedEventArgs e)
        {
            var usl = (Uslovie)En.Content.MiscObject[0];
            if (usl == Uslovie.Bolshe)
            {
                En.Content.MiscObject[0] = Uslovie.Menshe;
                Button1.Content = ">";
            }
            else
            {
                En.Content.MiscObject[0] = Uslovie.Bolshe;
                Button1.Content = "<";
            }
        }
    }
}
