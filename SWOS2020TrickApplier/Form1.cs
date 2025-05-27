using System;
using System.IO;
using System.Windows.Forms;

namespace SWOS2020TrickApplier
{
    public partial class Form1 : Form
    {
        private string logFilePath;

        public Form1()
        {
            InitializeComponent();
            InitializeLogFile();
            SetupForm();
        }

        private void SetupForm()
        {
            // Set default selection
            cmbTrickType.SelectedIndex = 0;
        }

        private void InitializeLogFile()
        {
            string appDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            logFilePath = Path.Combine(appDirectory, "swos_trick_log.txt");

            // Check if log file exists and if it's from today
            if (File.Exists(logFilePath))
            {
                DateTime lastWrite = File.GetLastWriteTime(logFilePath);
                if (lastWrite.Date != DateTime.Now.Date)
                {
                    // Reset log file if it's not from today
                    File.WriteAllText(logFilePath, $"=== SWOS 2020 Trick Applier Log - {DateTime.Now.Date:yyyy-MM-dd} ==={Environment.NewLine}");
                }
            }
            else
            {
                // Create new log file
                File.WriteAllText(logFilePath, $"=== SWOS 2020 Trick Applier Log - {DateTime.Now.Date:yyyy-MM-dd} ==={Environment.NewLine}");
            }
        }

        private void LogMessage(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                // Silent fail for logging
                Console.WriteLine($"Error writing to log: {ex.Message}");
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\SWOS 2020";
                openFileDialog.Filter = "CAR files (*.car)|*.car|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void BtnUpdateData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Please select a .car file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMessage("ERROR: No file selected");
                return;
            }

            if (!File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Selected file does not exist or may be corrupted.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"ERROR: File not found or corrupted - {txtFilePath.Text}");
                return;
            }

            string trickName = cmbTrickType.SelectedItem.ToString();
            string fileName = Path.GetFileName(txtFilePath.Text);

            try
            {
                // Create backup first
                string backupPath = CreateBackup(txtFilePath.Text);
                LogMessage($"Backup created successfully: {Path.GetFileName(backupPath)} for file: {fileName}");

                // Apply selected trick
                ApplyTrick(txtFilePath.Text, cmbTrickType.SelectedIndex);

                MessageBox.Show($"Operation completed successfully!\n\nTrick applied: {trickName}\nFile: {fileName}\nBackup created: {Path.GetFileName(backupPath)}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogMessage($"SUCCESS: Applied trick '{trickName}' to file '{fileName}' - Operation completed successfully");
            }
            catch (Exception ex)
            {
                // Check if it's a backup creation error
                if (ex.Message.Contains("backup"))
                {
                    MessageBox.Show($"Cannot create backup file:\n{ex.Message}\n\nOperation stopped for safety.", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessage($"ERROR: Backup creation failed for file '{fileName}' - {ex.Message}");
                }
                else
                {
                    MessageBox.Show($"Error occurred while processing the file:\n{ex.Message}", "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMessage($"ERROR: Failed to apply trick '{trickName}' to file '{fileName}' - {ex.Message}");
                }
            }
        }

        private string CreateBackup(string originalFilePath)
        {
            string directory = Path.GetDirectoryName(originalFilePath);
            string fileName = Path.GetFileNameWithoutExtension(originalFilePath);
            string extension = Path.GetExtension(originalFilePath);
            string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

            string backupPath = Path.Combine(directory, $"{fileName}_backup_{timestamp}{extension}");

            try
            {
                File.Copy(originalFilePath, backupPath);
                return backupPath;
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception("Access denied. Cannot create backup file. Check permissions.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new Exception("Directory not found. Cannot create backup file.");
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("already exists"))
                {
                    throw new Exception($"Backup file already exists: {Path.GetFileName(backupPath)}");
                }
                else
                {
                    throw new Exception($"Cannot create backup file: {ex.Message}");
                }
            }
        }

        private void ApplyTrick(string filePath, int trickIndex)
        {
            byte[] fileBytes;

            try
            {
                fileBytes = File.ReadAllBytes(filePath);
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception("Access denied. The file may be in use by another process or you don't have sufficient permissions.");
            }
            catch (IOException)
            {
                throw new Exception("The file is currently in use by another process. Please close any applications using this file and try again.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot read file: {ex.Message}");
            }

            switch (trickIndex)
            {
                case 0: // Get 100M $ (Arrivano gli Sceicchi)
                    // Offset D5DC (54748 decimal) -> 00 E1 F5 05
                    int offset1 = 0xD5DC;
                    if (fileBytes.Length <= offset1 + 3)
                    {
                        throw new Exception("File appears to be corrupted or not a valid .car file.");
                    }
                    fileBytes[offset1] = 0x00;
                    fileBytes[offset1 + 1] = 0xE1;
                    fileBytes[offset1 + 2] = 0xF5;
                    fileBytes[offset1 + 3] = 0x05;
                    break;

                case 1: // Unlimited Transfer (Calciomercato pazzo)
                    // Offset D87B and D87D -> Both to FF
                    int offset2a = 0xD87B;
                    int offset2b = 0xD87D;
                    if (fileBytes.Length <= Math.Max(offset2a, offset2b))
                    {
                        throw new Exception("File appears to be corrupted or not a valid .car file.");
                    }
                    fileBytes[offset2a] = 0xFF;
                    fileBytes[offset2b] = 0xFF;
                    break;

                case 2: // Unlimited Career Years (Voglio giocare per sempre)
                    // Offset D5C3 -> 00
                    int offset3 = 0xD5C3;
                    if (fileBytes.Length <= offset3)
                    {
                        throw new Exception("File appears to be corrupted or not a valid .car file.");
                    }
                    fileBytes[offset3] = 0x00;
                    break;

                default:
                    throw new InvalidOperationException("Invalid trick selection.");
            }

            try
            {
                File.WriteAllBytes(filePath, fileBytes);
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception("Access denied. Cannot write to file. The file may be in use by another process or you don't have sufficient permissions.");
            }
            catch (IOException)
            {
                throw new Exception("The file is currently in use by another process. Please close any applications using this file and try again.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot write to file: {ex.Message}");
            }
        }
    }
}