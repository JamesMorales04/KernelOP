using System;
using System.Threading;

namespace Kernel
{
    class Kernel
    {

        /*
         * File sistem port: 8082 
         * GUI port: 8081
         * Kernel port: 8080
         * 
         */
        static void Main(string[] args)
        {
            Kernel core = new Kernel();
            Comunication comunicationSet = new Comunication();
            Messages messages = new Messages(core, comunicationSet);

            comunicationSet.setterMessages(messages);

            Thread listener = new Thread(() => comunicationSet.StartListening(8080));
            listener.Start();

            comunicationSet.sendMessage("{cmd:stop, src:APP1, dst:kernel, msg”Err->División por 0”}", 8080);

        }


        private void initilizationCore()
        {
            //TODO:
        }

        public void stopKernel()
        {
            //TODO: 
        }


    }
}
