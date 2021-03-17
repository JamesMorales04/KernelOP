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
         * 
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

            string[] msgClean = msg.Replace("<EOF>", "").Replace("{", "").Replace("}", "").Split(',');

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
                    break;
                case "GestorArc":
                    comunication.sendMessage(rawMsg, 8082);
                    break;
                case "kernel":
                    //comunication.sendMessage(rawMsg, 8082);
                    core.stopKernel();
                    break;
                case "APP":
                    comunication.sendMessage(rawMsg, 8083);
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
