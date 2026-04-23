using System;
using System.Threading; // Mutexのために必要
using System.Windows.Forms;

namespace ZMKSplit
{
    internal static class Program
    {
        // アプリケーション固有の「鍵(Mutex)」を作成
        // 同名のMutexがOS内に存在するかどうかで多重起動を判定します
        private static Mutex mutex = new Mutex(true, "ZMKSplitBattery_UniqueAppId_2026");

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // すでに「鍵」が他のインスタンスによって使われているか確認
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                // すでに起動している場合は、静かに終了してPCへの負荷を抑える
                return;
            }

            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            finally
            {
                // アプリ終了時に鍵を確実に解放する
                if (mutex != null)
                {
                    mutex.ReleaseMutex();
                    mutex.Dispose();
                }
            }
        }
    }
}
