using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Planomatic
{
    /// <summary>
    /// Interaction logic for TeamViewPage.xaml
    /// </summary>
    public partial class TimeForBreakPage : Page
    {
        private string _videoFilePath = string.Empty;
        private Action _timeForBreakPageUnloadedCallback;
        DispatcherTimer _timer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();

        public TimeForBreakPage(Action timeForBreakPageUnloadedCallback)
        {
            InitializeComponent();

            _timer.Tick += new EventHandler(Timer_Click);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();

            _timeForBreakPageUnloadedCallback = timeForBreakPageUnloadedCallback;

            string videoFileName = "Workout.mp4";

            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            this._videoFilePath = Path.Combine(tempDirectory, videoFileName);
            CopyResourceFileIfApplicable(videoFileName, _videoFilePath);

            VideoPlayer.Open(new Uri(_videoFilePath, UriKind.Absolute));

            VideoPlayer.MediaEnded += (sender, e) =>
            {
                VideoPlayer.Position = TimeSpan.FromSeconds(0);
                VideoPlayer.Play();
            };

            VideoPlayer.IsMuted = false;
        }

        private void TimeForBreakPage_Loaded(object sender, RoutedEventArgs e)
        {
            stopWatch.Restart();
            VideoPlayer.Play();
        }

        private void TimeForBreakPage_Unloaded(object sender, RoutedEventArgs e)
        {
            stopWatch.Reset();
            stopWatch.Stop();
            VideoPlayer.Stop();
            _timeForBreakPageUnloadedCallback();
        }
        private void Timer_Click(object sender, EventArgs e)
        {
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

            TimerTextBlock.Text = elapsedTime;
        }

        private static void CopyResourceFileIfApplicable(
            string resourceFileName,
            string destFilePath)
        {
            if (!File.Exists(destFilePath))
            {
                using (Stream stream = GetResrouceAsStream(resourceFileName))
                {
                    byte[] bytes = new byte[(int)stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    File.WriteAllBytes(destFilePath, bytes);
                }
            }
        }

        private static Stream GetResrouceAsStream(string resourceName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string[] embeddedResouceNames = assembly.GetManifestResourceNames();
            string resourceFullName = Array.Find(
                embeddedResouceNames,
                s => s.ToUpperInvariant().Contains(resourceName.ToUpperInvariant()));

            return assembly.GetManifestResourceStream(resourceFullName);
        }
    }
}
