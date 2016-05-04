namespace kyt
{
    partial class Installer
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
            this.KytProcess = new System.ServiceProcess.ServiceProcessInstaller();
            this.KytService = new System.ServiceProcess.ServiceInstaller();
            // 
            // KytProcess
            // 
            this.KytProcess.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.KytProcess.Password = null;
            this.KytProcess.Username = null;
            // 
            // KytService
            // 
            this.KytService.Description = "Servicio Windows del RFID PT-3L01Z";
            this.KytService.DisplayName = "Kyt";
            this.KytService.ServiceName = "kyt";
            this.KytService.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // Installer
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.KytProcess,
            this.KytService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller KytProcess;
        private System.ServiceProcess.ServiceInstaller KytService;
    }
}