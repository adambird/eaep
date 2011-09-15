namespace eaep.servicehost
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EAEPServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.EAEPServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // EAEPServiceProcessInstaller
            // 
            this.EAEPServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.EAEPServiceProcessInstaller.Password = null;
            this.EAEPServiceProcessInstaller.Username = null;
            // 
            // EAEPServiceInstaller
            // 
            this.EAEPServiceInstaller.DisplayName = "EAEP Monitor";
            this.EAEPServiceInstaller.ServiceName = "EAEPMonitor";
            this.EAEPServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.EAEPServiceProcessInstaller,
            this.EAEPServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller EAEPServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller EAEPServiceInstaller;
    }
}