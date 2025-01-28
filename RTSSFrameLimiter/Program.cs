using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTSSFrameLimiter
{
    public class MainForm : Form
    {
        private TextBox exeTextBox;
        private TextBox customFpsTextBox;
        private Button customFpsButton;
        private ComboBox processDropdown;
        private Label statusLabel;
        private string[] fpsOptions = { "30", "48", "60", "90", "120" };
        private Button[] fpsButtons;

        public MainForm()
        {
            Text = "RTSS Frame Limiter";
            Size = new Size(400, 450);
            Font = new Font("Arial", 14);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            //Game EXE textbox
            Label exeLabel = new Label { Text = "Game EXE:", Location = new Point(10, 10), AutoSize = true };
            exeTextBox = new TextBox { Location = new Point(140, 10), Width = 230 , PlaceholderText = "Enter exe name..." };
            exeTextBox.TextChanged += (s, e) => {
                processDropdown.Items.Clear();
                ToggleButtons(!string.IsNullOrWhiteSpace(exeTextBox.Text));
            };

            //Populate the process list when the dropdown is clicked
            processDropdown = new ComboBox { Location = new Point(140, 40), Width = 230, DropDownStyle = ComboBoxStyle.DropDownList};
            processDropdown.Click += (s, e) => PopulateProcessList();
            processDropdown.SelectedIndexChanged += (s, e) =>
            {
                var selectedItem = processDropdown.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(selectedItem))
                {
                    exeTextBox.Text = selectedItem;
                }
            };

            //FPS limit buttons
            Label fpsLabel = new Label { Text = "Custom FPS:", Location = new Point(10, 80), AutoSize = true };
            customFpsTextBox = new TextBox { Location = new Point(140, 80), Width = 100 };
            customFpsTextBox.TextChanged += (s, e) => {
                customFpsButton.Enabled = !string.IsNullOrWhiteSpace(customFpsTextBox.Text) && customFpsTextBox.Text.All(char.IsDigit);
            };
            customFpsButton = new Button { Text = "Set Custom", Location = new Point(260, 80), Width = 120, Height = 40, Enabled = false };
            customFpsButton.Click += (s, e) => SetFpsLimit(customFpsTextBox.Text);

            statusLabel = new Label { Text = "", Location = new Point(10, 385), Width = 360, Height = 20, TextAlign = ContentAlignment.MiddleCenter };

            //Add controls to the form
            Controls.Add(exeLabel);
            Controls.Add(exeTextBox);
            Controls.Add(processDropdown);
            Controls.Add(fpsLabel);
            Controls.Add(customFpsTextBox);
            Controls.Add(customFpsButton);
            Controls.Add(statusLabel);

            //Add FPS limit buttons
            int yOffset = 130;
            fpsButtons = new Button[fpsOptions.Length];
            int btnIndex = 0;
            foreach (string fps in fpsOptions)
            {
                Button fpsButton = new Button { Text = fps + " FPS", Location = new Point(10, yOffset), Width = 360, Height = 40 };
                fpsButton.Click += (s, e) => SetFpsLimit(fps);
                Controls.Add(fpsButton);
                yOffset += 50;

                fpsButtons[btnIndex] = fpsButton;
                btnIndex++;
            }

            ToggleButtons(false);
        }

        /// <summary>
        /// Enable/disable FPS limit buttons based on the EXE textbox
        /// </summary>
        /// <param name="enable"></param>
        private void ToggleButtons(bool enable)
        {
            customFpsButton.Enabled = enable && !string.IsNullOrWhiteSpace(customFpsTextBox.Text) && customFpsTextBox.Text.All(char.IsDigit);
            foreach (var button in fpsButtons)
            {
                button.Enabled = enable;
            }
        }

        /// <summary>
        /// Run command to change RTSS fps limit for selected EXE file 
        /// </summary>
        /// <param name="fps"></param>
        private async void SetFpsLimit(string fps)
        {
            if (string.IsNullOrWhiteSpace(exeTextBox.Text))
            {
                statusLabel.Text = "Please enter the game's EXE name.";
                return;
            }

            string exeName = exeTextBox.Text.Trim();
            if (!exeName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                exeName += ".exe";
            }

            string command = $"property:set {exeName} FramerateLimit {fps}";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "rtss-cli.exe",
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                }
                statusLabel.Text = $"FPS limit set to {fps} for {exeName}";
                await Task.Delay(7000);
                statusLabel.Text = "";
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Populate the process list in the dropdown
        /// </summary>
        private void PopulateProcessList()
        {
            var processes = Process.GetProcesses()
                .Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && !p.ProcessName.Contains("System", StringComparison.OrdinalIgnoreCase))
                .Select(p => p.ProcessName + ".exe")
                .Distinct()
                .ToList();

            processDropdown.Items.Clear();
            processDropdown.Items.AddRange(processes.ToArray());
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
