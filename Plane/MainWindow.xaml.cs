using Microsoft.Win32;
using Plane.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Plane
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Information information = new Information();
        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = information;
        }
        
        string path=null;

        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "File (*.xml)|*.xml";
                if (openFile.ShowDialog() == true)
                {
                    path = openFile.FileName;
                    using(var XMLRead = new XmlTextReader(path))
                    {
                        XMLRead.WhitespaceHandling = WhitespaceHandling.None; // убираем в нем пробелы
                        while (XMLRead.Read())
                        {

                            switch (XMLRead.Name)
                            {
                                case "land_records": // поиск до атрибута land_records
                                    TreeViewItem TVItem = new TreeViewItem()
                                    {
                                        Header = "Parcels",
                                        IsExpanded = false,
                                    };
                                    ItemsTV.Items.Add(TVItem);

                                    var _read = XMLRead.ReadSubtree(); // передаем в атрибуте land_records XML
                                    while (_read.Read())
                                    {
                                        switch (_read.Name)
                                        {
                                            case "cad_number":
                                                BuildTreeView(TVItem, _read.ReadElementContentAsString());
                                                break;
                                        }
                                    }
                                    break;

                                case "build_records":
                                case "construction_record":
                                    TVItem = new TreeViewItem()
                                    {
                                        Header = "ObjectRealty",
                                        IsExpanded = false,
                                    };
                                    ItemsTV.Items.Add(TVItem);

                                    _read = XMLRead.ReadSubtree();

                                    while (_read.Read())
                                    {
                                        switch (_read.Name)
                                        {
                                            case "cad_number":
                                                BuildTreeView(TVItem, _read.ReadElementContentAsString());
                                                break;
                                        }
                                    }
                                    break;

                                case "spatial_data":
                                    TVItem = new TreeViewItem()
                                    {
                                        Header = "SpatialData",
                                        IsExpanded = false,
                                    };
                                    ItemsTV.Items.Add(TVItem);

                                    _read = XMLRead.ReadSubtree();

                                    while (_read.Read())
                                    {
                                        switch (_read.Name)
                                        {
                                            case "sk_id":
                                                BuildTreeView(TVItem, _read.ReadElementContentAsString());
                                                break;
                                        }
                                    }
                                    break;

                                case "municipal_boundaries":
                                    TVItem = new TreeViewItem()
                                    {
                                        Header = "Bounds",
                                        IsExpanded = false,
                                    };
                                    ItemsTV.Items.Add(TVItem);

                                    _read = XMLRead.ReadSubtree();

                                    while (_read.Read())
                                    {
                                        switch (_read.Name)
                                        {
                                            case "reg_numb_border":
                                                BuildTreeView(TVItem, _read.ReadElementContentAsString());
                                                break;
                                        }
                                    }
                                    break;

                                case "zones_and_territories_boundaries":
                                    TVItem = new TreeViewItem()
                                    {
                                        Header = "Zones",
                                        IsExpanded = false,
                                    };
                                    ItemsTV.Items.Add(TVItem);

                                    _read = XMLRead.ReadSubtree();

                                    while (_read.Read())
                                    {
                                        switch (_read.Name)
                                        {
                                            case "reg_numb_border":
                                                BuildTreeView(TVItem, _read.ReadElementContentAsString());
                                                break;
                                        }
                                    }
                                    break;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            //Функция для создания дочерних элементов
            void BuildTreeView(TreeViewItem treeViewItem, string name)
            {
                TreeViewItem item = new TreeViewItem()
                {
                    Header = name,
                    IsExpanded = false,
                };
                treeViewItem.Items.Add(item);
            }
            
        }

        
        private void ItemsTV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {   
            var item = ItemsTV.SelectedItem as TreeViewItem;
            
            var xml = new XmlDocument();
            xml.Load(path);

            
            var load_item = xml.SelectSingleNode($"//common_data[cad_number='{item.Header}']");
            if(load_item!=null) information.Info = load_item.InnerText;

        }


        private void helpBtn_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }
    }
}
