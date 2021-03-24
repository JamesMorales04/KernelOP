using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    /*
         * File sistem port: 8082 
         * GUI port: 8081
         * Kernel port: 8080
         * APP port: 8083
         */
    class Messages
    {
        private Comunication comunication;
        private Kernel core;

        public Messages(Kernel core, Comunication comunication)
        {
            this.comunication = comunication;
            this.core = core;

        }


        public void Actions(string msg)
        {
            msg = msg.Replace("<EOF>", "");
            Console.WriteLine("Mensaje entrante" + msg);
            string[] msgClean = msg.Replace("{", "").Replace("}", "").Split(',');


            if (msgClean.Length > 2)
            {
                longMessage(msgClean, msg);
            }
            else
            {
                shortMessage(msgClean);
            }

        }

        private void longMessage(string[] msgClean, string rawMsg)
        {
            switch (msgClean[2].Split(':')[1])
            {
                case "GUI":
                    comunication.sendMessage(rawMsg, 8081);
                    comunication.sendMessage(rawMsg, 8082);
                    break;
                case "GestorArc":
                    comunication.sendMessage(rawMsg, 8082);
                    break;
                case "kernel":

                    if (msgClean[0].Split(':')[1] == "start" && msgClean[1].Split(':')[1] == "GUI")
                    {
                        comunication.sendMessage(rawMsg, 8082);
                        string msg = core.startApp();

                        comunication.sendMessage(msg, 8081);
                        comunication.sendMessage(msg, 8082);



                    }
                    else if (msgClean[0].Split(':')[1] == "stop" && msgClean[1].Split(':')[1] == "GUI")
                    {

                        comunication.sendMessage(rawMsg, 8082);
                        if (core.isAppRunnig())
                        {
                            Console.WriteLine("aaaaaaaaaaaaa2");
                            comunication.sendMessage("{cmd:stop,src:kernel,dst:APP,msg:\"Stop System\"}", 8083);


                            string msg = core.stoptApp();
                            comunication.sendMessage(msg, 8081);
                            comunication.sendMessage(msg, 8082);
                        }
                        else
                        {
                            comunication.sendMessage("{cmd:send,src:kernel,dst:GUI,msg:\"Error->App not running\"}", 8081);
                            comunication.sendMessage("{cmd:send,src:kernel,dst:GUI,msg:\"Error->App not running\"}", 8082);
                        }


                    }
                    else
                    {
                        comunication.sendMessage(rawMsg, 8082);
                        core.stopKernel(comunication);
                    }
                    break;
                case "APP":
                    comunication.sendMessage(rawMsg, 8083);
                    comunication.sendMessage(rawMsg, 8082);
                    break;
                default:
                    break;
            }
        }

        private void shortMessage(string[] msgClean)
        {
            switch (msgClean[1].Replace("\"", "").Split(':')[1])
            {
                case "OK":
                    break;
                case "0":
                    break;
                case "Err":
                    break;
                default:
                    break;
            }
        }




    }
}
