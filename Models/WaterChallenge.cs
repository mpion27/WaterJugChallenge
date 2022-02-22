using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterJugChallenge.Models
{
    public class WaterChallenge
    {
        int A = 0; int B = 0; int Z = 0;

        int WaterInA = 0;
        int WaterInB = 0;
        int WaterInZ = 0;
        StringBuilder message = new StringBuilder();
        List<WaterAction> Actions = new List<WaterAction>();
        WaterAction action;

        string FILL_JUG_A = "FILL_JUG_A";
        string FILL_JUG_B = "FILL_JUG_B";
        string TRANSFER_AB= "TRANSFER_AB";
        string TRANSFER_BA= "TRANSFER_BA";
        string TRANSFER_AZ= "TRANSFER_AZ";
        string TRANSFER_BZ= "TRANSFER_BZ";
        string EMPTY_JUG_A= "EMPTY_JUG_A";
        string EMPTY_JUG_B= "EMPTY_JUG_B";



        public WaterChallenge(int ASize, int BSize, int ZSize)
        {
            A = ASize; B = BSize; Z = ZSize;
        }

        public List<WaterAction> GetActionsSolution()
        {

            bool fillWithA = false;
            bool fillWithB = false;
            bool fillTotalAB = false;
            bool fillWithDiff = false;

            //can fill with Jug A?
            fillWithA = IsDivisable(Z, A);
            //can fill with Jug B?
            fillWithB = IsDivisable(Z, B);
            //can fill with total of jug A and B
            fillTotalAB = IsDivisable(Z, (A + B));
            //can fill with difference of Jug A and B
            if(A-B!=0)
            fillWithDiff = IsDivisable(Z, Math.Abs(A - B));

            //Has no solution
            if(!fillWithA && !fillWithB && !fillTotalAB && !fillWithDiff)
            {
                return Actions;
            }


            while (WaterInZ < Z)
            {

                if (fillWithA)
                {
                    FillJurA();
                    transferAZ();
                }

                else if (fillWithB)
                {
                    FillJurB();
                    transferBZ();
                }

                else if (fillTotalAB)
                {

                    //Fill a
                    FillJurA();
                    //transfer a to z
                    transferAZ();
                    //Fill b
                    FillJurB();
                    //transfer b to z
                    transferBZ();
                }

                else if (fillWithDiff)
                {


                    if (A > B)
                    {
                        //fill a
                        FillJurA();
                        //transfer ab
                        transferAB();
                        //transfer az
                        transferAZ();
                        //emty b
                        EmptyJugB();
                    }
                    else
                    {
                        //fill b
                        FillJurB();
                        //transfer ba
                        transferBA();
                        //transfer bz
                        transferBZ();
                        //emty a
                        EmptyJugA();
                        
                    }
                }
                Console.WriteLine(message);
                message.Clear();


            }
            return Actions;
        }

        private bool IsDivisable(int dividend, int divisor)
        {
            bool result = dividend % divisor == 0;

            return result;
        }

        void FillJurA() { 
            WaterInA = A;
            action = new WaterAction()
            {
                Action = FILL_JUG_A,
                WaterInA = WaterInA,
                WaterInB = WaterInB,
                WaterInZ = WaterInZ
            };
            Actions.Add(action);
        }
        void FillJurB() { 
            WaterInB = B;
            action = new WaterAction()
            {
                Action = FILL_JUG_B,
                WaterInA = WaterInA,
                WaterInB = WaterInB,
                WaterInZ = WaterInZ
            };
            Actions.Add(action);
        }

        void transferAZ()
        {

            message.Append($"\nTransfer AZ : {WaterInA}, {WaterInZ}");
            WaterInZ += WaterInA;
            WaterInA = 0;
            message.Append($" -> : {WaterInA}, {WaterInZ}");
            action = new WaterAction()
            {
                Action = TRANSFER_AZ,
                WaterInA = WaterInA,
                WaterInB = WaterInB,
                WaterInZ = WaterInZ
            };
            Actions.Add(action);

        }
        void transferAB()
        {
            message.Append($"\nTransfer AB : {WaterInA}, {WaterInB}");
            int spaceAvailable = B - WaterInB;

            if (spaceAvailable > WaterInA)
            {
                WaterInB += WaterInA;
                WaterInA = 0;
            }

            else
            {
                WaterInB += spaceAvailable;
                WaterInA = WaterInA - spaceAvailable;
            }
            message.Append($" -> : {WaterInA}, {WaterInB}");

            action = new WaterAction()
            {
                Action = TRANSFER_AB,
                WaterInA = WaterInA,
                WaterInB = WaterInB,
                WaterInZ = WaterInZ
            };
            Actions.Add(action);
        }
        void transferBA()
        {
            message.Append($"\nTransfer BA : {WaterInB}, {WaterInA}");
            int spaceAvailable = A - WaterInA;

            if (spaceAvailable > WaterInB)
            {
                WaterInA += WaterInB;
                WaterInB = 0;
            }
            else
            {
                WaterInA += spaceAvailable;
                WaterInB = WaterInB - spaceAvailable;
            }
            message.Append($" -> : {WaterInA}, {WaterInB}");

            action = new WaterAction()
            {
                Action = TRANSFER_BA,
                WaterInA = WaterInA,
                WaterInB = WaterInB,
                WaterInZ = WaterInZ
            };
            Actions.Add(action);
        }
        void transferBZ()
        {
            message.Append($"\nTransfer BZ : {WaterInB}, {WaterInZ}");
            WaterInZ += WaterInB;
            WaterInB = 0;
            message.Append($" -> : {WaterInB}, {WaterInZ}");

            action = new WaterAction()
            {
                Action = TRANSFER_BZ,
                WaterInA = WaterInA,
                WaterInB = WaterInB,
                WaterInZ = WaterInZ
            };
            Actions.Add(action);
        }

        void EmptyJugA()
        {
            WaterInA = 0;

            action = new WaterAction()
            {
                Action = EMPTY_JUG_A,
                WaterInA = WaterInA,
                WaterInB = WaterInB,
                WaterInZ = WaterInZ
            };
            Actions.Add(action);
        }
        void EmptyJugB()
        {
            WaterInB = 0;
            action = new WaterAction()
            {
                Action = EMPTY_JUG_B,
                WaterInA = WaterInA,
                WaterInB = WaterInB,
                WaterInZ = WaterInZ
            };

            Actions.Add(action);
        }

        
    }


}