using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameUnit;

namespace Game_Character_Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Character _character = new Character();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void LoadCharaterInfo()
        {
            cbxStat.ItemsSource = _character.Stats;
            cbxSkill.ItemsSource = _character.Skills;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbxName.Text = _character.Name;
            LoadCharaterInfo();
            
            ItemUnit.Initialize();

            foreach (string unit in ItemUnit.Units)
            {
                string type = ItemUnit.GetUnitTypeFromName(unit);
                if (type.Equals("Armor"))
                    cbxArmor.Items.Add(unit);
                else if (type.Equals("Weapon"))
                    cbxWeapon.Items.Add(unit);
                else if (type.Equals("Item"))
                    cbxItem.Items.Add(unit);
            }
        }

        private void cbxStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxStat.SelectedIndex == -1)
                return;

            string stat = cbxStat.SelectedValue.ToString();
            lblStat.Content = "Value: +" + _character[stat];
        }

        private void btnUseSkill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditName_Click(object sender, RoutedEventArgs e)
        {
            if (tbxName.IsReadOnly != false)
            {
                btnEditName.Content = "OK";
                tbxName.IsReadOnly = false;
            }
            else
            {
                btnEditName.Content = "Edit";
                tbxName.IsReadOnly = true;
                _character.Name = tbxName.Text;
            }
        }

        private void btnEquipWeapon_Click(object sender, RoutedEventArgs e)
        {
            if (cbxWeapon.SelectedIndex == -1)
                return;

            string weapon = cbxWeapon.SelectedValue.ToString();
            bool flag = false;

            if (cbxWeapon.IsEnabled == true)
            {
                ItemUnit.Execute(weapon, "Equip", _character, out flag);

                if (flag == true)
                {
                    cbxWeapon.IsEnabled = false;
                    btnEquipWeapon.Content = "Remove";
                    LoadCharaterInfo();
                }
            }
            else
            {
                ItemUnit.Execute(weapon, "Remove", _character, out flag);

                if (flag == true)
                {
                    cbxWeapon.IsEnabled = true;
                    btnEquipWeapon.Content = "Equip";
                }
            }
        }

        private void btnEquipArmor_Click(object sender, RoutedEventArgs e)
        {
            if (cbxArmor.SelectedIndex == -1)
                return;

            string armor = cbxArmor.SelectedValue.ToString();
            bool flag = false;

            if (cbxArmor.IsEnabled == true)
            {
                ItemUnit.Execute(armor, "Equip", _character, out flag);

                if (flag == true)
                {
                    cbxArmor.IsEnabled = false;
                    btnEquipArmor.Content = "Remove";
                    LoadCharaterInfo();
                }
            }
            else
            {
                ItemUnit.Execute(armor, "Remove", _character, out flag);

                if (flag == true)
                {
                    cbxArmor.IsEnabled = true;
                    btnEquipArmor.Content = "Equip";
                }
            }
        }

        private void btnUseItem_Click(object sender, RoutedEventArgs e)
        {
            if (cbxItem.SelectedIndex == -1)
                return;

            string item = cbxItem.SelectedValue.ToString();

            bool flag = false;
            ItemUnit.Execute(item, "Use", _character, out flag);

            if (flag == true)
            {
                cbxUsedItem.Items.Add(item);
                LoadCharaterInfo();
            }
        }
    }
}
