﻿using System;
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

namespace HC12_Progsis_Compiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Linea> lineas;
        public MainWindow()
        {
            InitializeComponent();
            lineas = new List<Linea>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) {
                source.Text = "";
                string filename = dlg.FileName;
                name.Text = filename;
                {
                    string temp;
                    System.IO.StreamReader archivo = new System.IO.StreamReader(filename);
                    while ((temp = archivo.ReadLine()) != null) {
                        source.Text += temp + "\n";
                    }
                    //source.Text = archivo.ReadToEnd();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String[] lineas = source.Text.Split('\n');
            Console.WriteLine(lineas.Length);
            foreach (string line in lineas) {
                String[] terminos = line.Split('\t',' ');
                for (int i = 0; i < terminos.Length; i++) {
                    mostrarErrores.Text+= terminos[i] + i + '*';
                }
               mostrarErrores.Text += '\n';
            }
        }
    }
}
