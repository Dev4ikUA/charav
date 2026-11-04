using CharityAV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace CharityAV48
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string[] trojans = new string[]
{
    "Trojan.Win32.Agent",
    "Trojan.Win32.Injector",
    "Trojan.Win32.Stealer",
    "Trojan.Win32.Backdoor",
    "Trojan.Win32.Crypt",
    "Trojan.Win32.Ransom",
    "Trojan.Win32.Dropper",
    "Trojan.Win32.Keylogger",
    "Trojan.Win32.Loader",
    "Trojan.Win32.Ratink_Laff",
    "Trojan.Win32.AlexMotu_chudit"
};
        private int criticalCount = 0;
        private int suspiciousCount = 0;
        // Обробник кнопки Scan Now
        private async void StartScan_Click(object sender, RoutedEventArgs e)
        {
            criticalCount = 0;
            suspiciousCount = 0;

            CriticalText.Text = "Критические угрозы: 0";
            SuspiciousText.Text = "Обычные угрозы: 0";
            MainLogBox.Clear();
            MainScanProgress.Value = 0;

            int scanned = 0;
            Random rand = new Random();

            await Task.Run(() =>
            {
                foreach (var file in DirLikeScan(@"C:\Windows"))
                {
                    scanned++;

                    if (rand.Next(0, 50) == 1)
                    {
                        suspiciousCount++;

                        Dispatcher.Invoke(() =>
                        {
                            MainLogBox.AppendText($"⚠ Странное поведение у {file}\n");
                            SuspiciousText.Text = $"Обычные угрозы: {suspiciousCount}";
                        });
                    }

                    if (rand.Next(0, 200) == 1)
                    {
                        string trojan = trojans[rand.Next(trojans.Length)];
                        criticalCount++;

                        Dispatcher.Invoke(() =>
                        {
                            MainLogBox.AppendText($"КРИТИЧНО: {trojans} обнаружен в {file}\n");
                            CriticalText.Text = $"Критические угрозы: {criticalCount}";
                        });
                    }

                    // Фільтр як у антивіруса
                    if (!file.EndsWith(".exe") && !file.EndsWith(".dll") && !file.EndsWith(".sys"))
                        continue;

                    string threat = $"FakeThreat{rand.Next(100, 1000)}";

                    // Оновлення UI через Dispatcher
                    Dispatcher.Invoke(() =>
                    {
                        MainLogBox.AppendText($"Сканирование: {file} ...\n");
                        MainLogBox.ScrollToEnd();

                        MainScanProgress.Value = (scanned % 100); // циклічний прогрес як у старих AV
                    });

                    // Фейкові події
                    if (rand.Next(0, 50) == 1)
                    {
                        Dispatcher.Invoke(() =>
                            MainLogBox.AppendText($"⚠ Странное поведение у {file}\n"));
                    }

                    if (rand.Next(0, 200) == 1)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            MainLogBox.AppendText($"КРИТИЧНО: {trojans} обнаружен в {file}\n");
                        });
                    }

                    System.Threading.Thread.Sleep(1); // ефект "роботи"
                }
            });

            MainLogBox.AppendText("\nСканирование завершено!\n");

            Dispatcher.Invoke(() =>
            {
                int totalThreats = criticalCount + suspiciousCount;

                if (totalThreats > 0)
                {
                    var result = MessageBox.Show(
                        $"Обнаружено угроз: {totalThreats}\n" +
                        $"Критичных: {criticalCount}\n" +
                        $"Обычных: {suspiciousCount}\n\n" +
                        $"Нажите ДА для устранения угроз",
                        "CharityAV Результаты",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        RunCleanupExe();
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Угроз не обнаружено!",
                        "CharityAV",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            });
        }

        private void RunCleanupExe()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = @"C:\Path\To\Your\cleanup.exe", // ← тут свій exe
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось запустить Cleanup: " + ex.Message);
            }
        }

        private IEnumerable<string> DirLikeScan(string root)
        {
            Stack<string> dirs = new Stack<string>();
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string current = dirs.Pop();

                string[] subDirs = new string[0];
                string[] files = new string[0];

                try
                {
                    files = Directory.GetFiles(current);
                }
                catch { }

                foreach (var file in files)
                    yield return file;

                try
                {
                    subDirs = Directory.GetDirectories(current);
                }
                catch { }

                foreach (var dir in subDirs)
                    dirs.Push(dir);
            }
        }

        // Обробник кнопки Cleanup (відкриває WinForms форму)
        private void OpenCleanupWindow_Click(object sender, RoutedEventArgs e)
        {
            CleanupForm cleanup = new CleanupForm();
            cleanup.ShowDialog();
        }
    }
}