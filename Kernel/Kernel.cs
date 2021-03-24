using System;
using System.Threading;
using System.Diagnostics;


namespace Kernel
{
    class Kernel
    {

        /*
         * File sistem port: 8082 
         * GUI port: 8081
         * Kernel port: 8080
         * APP port: 8083
         * 
         */

        private Process[] modules = new Process[3];
        private string[] modulesNames = new string[] { "GuiOP.exe", "OAPP.exe", "GestorArchivos.exe" };
        static void Main(string[] args)
        {

            Kernel core = new Kernel();
            core.initilizationCore();
            Comunication comunicationSet = new Comunication();
            Messages messages = new Messages(core, comunicationSet);

            comunicationSet.setterMessages(messages);

            Thread listener = new Thread(() => comunicationSet.StartListening(8080));
            listener.Start();


        }


        private void initilizationCore()
        {


            for (int index = 0; index < modules.Length; index++)
            {
                modules[index] = new Process();


                modules[index].StartInfo.FileName = modulesNames[index];


                modules[index].Start();

            }
        }

        public void stopKernel(Comunication comunicationSet)
        {
            comunicationSet.sendMessage("{cmd:stop,src:kernel,dst:GUI,msg:\"Stop System\"}", 8081);
            comunicationSet.sendMessage("{cmd:stop,src:kernel,dst:GestorArc,msg:\"Stop System\"}", 8082);
            comunicationSet.sendMessage("{cmd:stop,src:kernel,dst:APP,msg:\"Stop System\"}", 8083);

            System.Environment.Exit(1);
        }

        public string startApp()
        {
            if (modules[1] == null)
            {
                modules[1] = new Process();
                modules[1].StartInfo.FileName = modulesNames[1];
                modules[1].Start();
                return "{cmd:send,src:kernel,dst:GUI,msg:\"App->started\"}";
            }

            return "{cmd:send,src:kernel,dst:GUI,msg:\"Error->App already running\"}";



        }

        public string stoptApp()
        {
            if (modules[1] != null)
            {

                modules[1].CloseMainWindow();
                modules[1] = null;
                return "{cmd:send,src:kernel,dst:GUI,msg:\"App->stopped\"}";

            }

            return "{cmd:send,src:kernel,dst:GUI,msg:\"Error->App not running\"}";
        }

        public bool isAppRunnig()
        {
            if (modules[1] == null)
            {
                return false;
            }
            return true;
        }
    }
}
