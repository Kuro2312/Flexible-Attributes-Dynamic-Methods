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
using System.Reflection;
using System.IO;
using FlexibleUnit;
using System.Collections.ObjectModel;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Game_Character_Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Character _character = new Character();
        private Dictionary<string, DynamicMethod> _skills = new Dictionary<string, DynamicMethod>();
        int _counter = 0;

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

            LoadSkills();
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
            if (cbxSkill.SelectedIndex == -1)
                return;

            string skill = cbxSkill.SelectedValue.ToString();
            bool flag;
            string result = _character.ExecuteMethod(skill, null, out flag).ToString();
            MessageBox.Show(result);
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

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (cbxNewSkill.SelectedIndex == -1)
                return;

            string skill = cbxNewSkill.SelectedValue.ToString();
            cbxNewSkill.Items.RemoveAt(cbxNewSkill.SelectedIndex);
            cbxNewSkill.SelectedIndex = -1;

            _character.AddNewSkill(_skills[skill]);
            cbxSkill.ItemsSource = _character.GetAllSkills();
        }

        private void LoadSkills()
        {
            // Get list of DLL files in main executable folder
            string folder = Directory.GetCurrentDirectory();
            FileInfo[] fis = new DirectoryInfo(folder).GetFiles("*.dll");

            // Load all assemblies from current working directory
            foreach (FileInfo fileInfo in fis)
            {
                var domain = AppDomain.CurrentDomain;
                Assembly assembly = domain.Load(AssemblyName.GetAssemblyName(fileInfo.FullName));

                // Get all of the types in the dll
                Type[] types = assembly.GetTypes();

                // Only create instance of concrete class that inherits from IGUI, IBus or IDao
                foreach (var type in types)
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        if (typeof(DynamicMethod).IsAssignableFrom(type))
                        {
                            DynamicMethod skill = Activator.CreateInstance(type) as DynamicMethod;

                            if (skill.Name() != "Equip Game Item" && skill.Name() != "Remove Game Item")
                            {
                                _skills.Add(skill.Name(), skill);
                                cbxNewSkill.Items.Add(skill.Name());
                            }
                        }
                    }
                }
            }
        }

        private DynamicMethod GenPluginFromScript(string name, string text)
        {
            string strCSharpSourceCode = GenCSharpSourceCode(name, text);
            Assembly asm = CompileCode((_counter - 1).ToString("0000"), strCSharpSourceCode);
            return CreatePluginFromAssembly(asm);
        }

        private string GenCSharpSourceCode(string name, string text)
        {
            string strTemplate = "using FlexibleUnit; namespace DynamicPlugin { public class ClassName###1 : DynamicMethod { public string Name() { return \"###2\"; } public object Execute(object param, out bool isSuccessful) {     ###3 } } } ";
            
            string part1 = _counter.ToString("0000");
            _counter++;

            string strCode = strTemplate.Replace("###1", part1);

            string part2 = name;
            strCode = strCode.Replace("###2", part2);
            
            string part3 = text;
            strCode = strCode.Replace("###3", part3);

            return strCode;
        }

        private Assembly CompileCode(string name, string strCSharpSourceCode)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            // Reference to System.Drawing library
            parameters.ReferencedAssemblies.Add("FlexibleUnit.dll");

            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = false;

            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = false;

            parameters.OutputAssembly = name + ".dll";

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, strCSharpSourceCode);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                MessageBox.Show(sb.ToString());
                return null;
            }

            return results.CompiledAssembly;
        }

        private DynamicMethod CreatePluginFromAssembly(Assembly asm)
        {
            foreach (Type type in asm.GetTypes())
            {
                DynamicMethod result = Activator.CreateInstance(type) as DynamicMethod;
                
                if (result != null)
                    return result;
            }

            return null;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            DynamicMethod skill = GenPluginFromScript(tbxSkillName.Text, tbxSkillEffect.Text);
            
            if (skill == null)
                return;

            _character.AddNewSkill(skill);
            cbxSkill.ItemsSource = _character.GetAllSkills();
        }
    }
}
