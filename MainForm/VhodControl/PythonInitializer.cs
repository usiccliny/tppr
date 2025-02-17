using System.Diagnostics;

internal class PythonInitializer
{
    private Process _pythonProcess;

    public static string FindPythonPath()
    {
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "where";
            process.StartInfo.Arguments = "python";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string[] paths = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length > 0)
            {
                return paths[0].Trim();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске Python: {ex.Message}");
        }

        return null;
    }

    public async Task RunPythonScriptAsync(string scriptPath)
    {
        try
        {
            string exeDirectory = Application.StartupPath;
            scriptPath = Path.Combine(exeDirectory, scriptPath);

            if (!File.Exists(scriptPath))
            {
                MessageBox.Show($"Файл {scriptPath} не найден.");
                return;
            }

            string pythonExecutable = FindPythonPath();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonExecutable,
                Arguments = $"\"{scriptPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _pythonProcess = new Process();
            _pythonProcess.StartInfo = startInfo;

            _pythonProcess.Start();

            await _pythonProcess.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при запуске Python-скрипта: {ex.Message}");
        }
    }

    public void StopPythonScript()
    {
        if (_pythonProcess != null && !_pythonProcess.HasExited)
        {
            _pythonProcess.Kill();
            _pythonProcess.Dispose();
            _pythonProcess = null;
        }
    }
}