using System;
using System.Windows.Forms;
using Notifier.Controller;
using Notifier.Model;
using Notifier.View;

namespace Notifier
{
    public class NotifierApplicationContext : ApplicationContext
    {
        const int MINUTE = 60 * 1000;
        const int HOUR = 60 * MINUTE;

        public double Interval
        {
            get { return mController.Interval; }
            set { mController.Interval = value; }
        }

        public INotificationModel Model
        {
            get { return mModel; }
            set
            {
                mModel = value;
                mController.Model = value;
                View.Model = value;
            }
        }

        public INotificationView View
        {
            get
            {
                if (mView == null)
                    View = new NotifyIconBalloon();

                return mView;
            }
            set
            {
                if (mView != null)
                    mView.NotifyIcon = null;
                if (value != null)
                    value.NotifyIcon = mNotifyIcon;
                mView = value;
            }
        }

        private System.ComponentModel.IContainer mComponents;
        private ContextMenuStrip mContextMenu;
        private NotificationController mController;
        private ToolStripMenuItem mExitApplication;
        private ToolStripMenuItem mStartMonitoring;
        private ToolStripMenuItem mStopMonitoring;
        private INotificationModel mModel;
        private NotifyIcon mNotifyIcon;
        private INotificationView mView;

        public NotifierApplicationContext()
        {
            //Instantiate the component Module to hold everything
            mComponents = new System.ComponentModel.Container();

            //Instantiate the NotifyIcon attaching it to the components container and 
            //provide it an icon, note, you can imbed this resource 
            mNotifyIcon = new NotifyIcon(this.mComponents);
            mNotifyIcon.Icon = new System.Drawing.Icon("Icon.ico");
            mNotifyIcon.Visible = true;

            //Instantiate the context menu and items
            mContextMenu = new ContextMenuStrip();

            //Attach the menu to the notify icon
            mNotifyIcon.ContextMenuStrip = mContextMenu;

            mExitApplication = new ToolStripMenuItem();
            mExitApplication.Text = "Exit";
            mExitApplication.Click += mExitApplication_Click;
            mContextMenu.Items.Add(mExitApplication);

            mStartMonitoring = new ToolStripMenuItem();
            mStartMonitoring.Text = "Start";
            mStartMonitoring.Click += mStartMonitoring_Click;

            mStopMonitoring = new ToolStripMenuItem();
            mStopMonitoring.Text = "Stop";
            mStopMonitoring.Click += mStopMonitoring_Click;

            mController = new NotificationController();

            StateChange();
        }

        protected override void ExitThreadCore()
        {
            mController.Stop();

            Model.Clear();
            Model = null;

            base.ExitThreadCore();
        }

        private void mExitApplication_Click(object sender, EventArgs e)
        {
            //Call our overridden exit thread core method!
            ExitThreadCore();
        }

        private void mStartMonitoring_Click(object sender, EventArgs e)
        {
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                mNotifyIcon.ShowBalloonTip(MINUTE, "Error", ex.Message, ToolTipIcon.Error);
            }
        }

        private void mStopMonitoring_Click(object sender, EventArgs e)
        {
            try
            {
                mController.Stop();
                StateChange();
            }
            catch (Exception ex)
            {
                mNotifyIcon.ShowBalloonTip(MINUTE, "Error", ex.Message, ToolTipIcon.Error);
            }
        }

        public void Start()
        {
            mController.Start();
            StateChange();
        }

        private void StateChange()
        {
            mStartMonitoring.Enabled = !mController.IsMonitoring;
            mStopMonitoring.Enabled = mController.IsMonitoring;
        }
    }
}
