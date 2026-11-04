using System;
using System.Windows.Forms;

namespace CharityAV
{
    public partial class CleanupForm : Form
    {
        private void InitializeUI()
        {
            this.Text = "CharityAV Cleanup";
            this.Width = 400;
            this.Height = 300;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Список фейкових файлів
            ListBox fileList = new ListBox();
            fileList.Top = 20;
            fileList.Left = 20;
            fileList.Width = 340;
            fileList.Height = 180;
            fileList.Items.AddRange(new string[]
            {
                "Burmalda.exe",
                "rundll32.dll",
                "Laff_RAT_Driver.sys"
            });
            this.Controls.Add(fileList);

            // Кнопка Cleanup
            Button cleanButton = new Button();
            cleanButton.Text = "Очистить";
            cleanButton.Width = 120;
            cleanButton.Height = 40;
            cleanButton.Top = 210;
            cleanButton.Left = 120;
            cleanButton.Click += (s, e) =>
            {
                MessageBox.Show("Вибраные файлы очищено.", "CharityAV", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            this.Controls.Add(cleanButton);
        }
    }
}