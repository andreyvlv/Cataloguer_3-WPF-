using System;
using System.Text;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Cataloguer_2_WPF_
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			save.IsEnabled = false;
			
		}

		string[] files;
		string path;			
		int substringIndex;

		private void button_Click(object sender, RoutedEventArgs e)
		{					
			fileListView.Items.Clear();
			path = pathFile.Text;
			string fileExt = fileExtension.Text;
			// выбор опции поиска
			SearchOption srchOpt = new SearchOption();
			if (allDirCheckBox.IsChecked == true)
				srchOpt = SearchOption.AllDirectories;
			else
				srchOpt = SearchOption.TopDirectoryOnly;

            // В зависимости от выбора чекбокса substringDir выбирается вырезать или оставить путь в названии файла
            string replacer;
            if (substringDir.IsChecked == true)
                replacer = "";
            else
                replacer = path + "\\";

			// Если указанный каталог существует произвести поиск по расширению файлов			 		
			if (Directory.Exists(path))
			{
				try
				{					
					files = Directory.GetFiles(@path, "*." + fileExt, srchOpt);                                    
                    for (int i = 0; i < files.Length; i++)
                    {                       
                        files[i] = files[i].Replace(path + "\\", replacer);                       
                        fileListView.Items.Add(files[i]);
                    }                  
                    found.Text = "Найдено файлов: " + files.Length;
					save.IsEnabled = true;
				}
				catch(IOException)
				{
					System.Windows.MessageBox.Show("Произошла ошибка!");
				}
			}
			else
			{
				System.Windows.MessageBox.Show("Указанного каталога не существует!");
			}
		}

		private void save_Click(object sender, RoutedEventArgs e)
		{
			if (fileListView.Items.Count > 0)
			{
				Microsoft.Win32.SaveFileDialog saveFile1 = new Microsoft.Win32.SaveFileDialog();
				saveFile1.DefaultExt = "*.txt";
				saveFile1.Filter = "Text files|*.txt";
				if (saveFile1.ShowDialog() == true && saveFile1.FileName.Length > 0)
				{
					using (StreamWriter sw = new StreamWriter(saveFile1.FileName, true))
					{
						// Если отмечен substringDir
						if (substringDir.IsChecked == true)
						{
							sw.WriteLine("Имя каталога: " + path);
							sw.WriteLine();
						}	
						// Вывод массива найденных файлов в файл				
						for (int i = 0; i < files.Length; i++)
						{							
							sw.WriteLine(files[i]);
						}						
						sw.WriteLine();
						sw.WriteLine("Найдено:" + files.Length);						
					}
				}
			}
		}

		private void openFolder_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string storage = dialog.SelectedPath;
				pathFile.Text = storage;
			}
		}
	}
}
