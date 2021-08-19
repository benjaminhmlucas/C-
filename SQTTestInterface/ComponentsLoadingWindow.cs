using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SQTTestInterface {
    public partial class ComponentsLoadingWindow : Form {
        public ComponentsLoadingWindow(string loadingMessage,Bitmap loadingAnimationName) {
            InitializeComponent(loadingMessage, loadingAnimationName);
        }
        //Delegate for cross thread call to close
        public delegate void CloseDelegate();
        //The type of form to be displayed as the splash screen.
        private static ComponentsLoadingWindow componentsLoadingWindow;
        static public ComponentsLoadingWindow ShowSplashScreen(string loadingMessage, Bitmap loadingAnimationName) {
            // Make sure it is only launched once.    
            componentsLoadingWindow = new ComponentsLoadingWindow(loadingMessage,loadingAnimationName);
            Thread thread = new Thread(new ThreadStart(ShowForm));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return componentsLoadingWindow;
        }
        static private void ShowForm() {
            componentsLoadingWindow.CenterToScreen();
            if (componentsLoadingWindow != null) Application.Run(componentsLoadingWindow);
        }
        static public void CloseForm() {
            componentsLoadingWindow?.Invoke(new CloseDelegate(CloseFormInternal));
        }
        static private void CloseFormInternal() {
            if (componentsLoadingWindow != null) {
                componentsLoadingWindow.Close();
                componentsLoadingWindow = null;
            };
        }
    }
}
