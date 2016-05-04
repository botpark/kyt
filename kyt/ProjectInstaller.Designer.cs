namespace kyt
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Kyt = new System.ServiceProcess.ServiceProcessInstaller();
            this.KytServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // Kyt
            // 
            this.Kyt.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.Kyt.Password = null;
            this.Kyt.Username = null;
            this.Kyt.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // KytServiceInstaller
            // 
            this.KytServiceInstaller.Description = "Servicio de rfid";
            this.KytServiceInstaller.DisplayName = "KytService";
            this.KytServiceInstaller.ServiceName = "kyt";
            this.KytServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.KytServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.Kyt,
            this.KytServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller Kyt;
        private System.ServiceProcess.ServiceInstaller KytServiceInstaller;
    }
}