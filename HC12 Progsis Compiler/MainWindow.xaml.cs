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
        analizador ana;
        public MainWindow()
        {
            InitializeComponent();
            lineas = new List<Linea>();
            ana = new analizador();
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
            Linea aux;
            String[] lineas = source.Text.Split('\n');
            foreach (string linea in lineas) {
                if (linea.Length > 0) {
                    aux = ana.analizar(linea);
                    this.lineas.Add(aux);
                }
                ana = new analizador();
            }
            foreach (Linea i in this.lineas) {
                if (i.comentario != null) {
                    Console.WriteLine("Comentario");
                }
                if (i.etiqueta != null) {
                    Console.WriteLine("Etiqueta:"+ i.etiqueta);
                }
                else
                {
                    Console.WriteLine("Etiqueta: null");
                }
                if (i.codop != null)
                {
                    Console.WriteLine("CODOP: " + i.codop);
                }
                else {
                    Console.WriteLine("CODOP: null");
                }
                if (i.operando != null)
                {
                    Console.WriteLine("Operando: " + i.operando);
                }
                else {
                    Console.WriteLine("Operando: null");
                }
                Console.WriteLine();
            }
            

        }
    }
}
