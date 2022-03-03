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
        string TRANSFER_AB = "TRANSFER_AB";
        string TRANSFER_BA = "TRANSFER_BA";
        string EMPTY_JUG_A = "EMPTY_JUG_A";
        string EMPTY_JUG_B = "EMPTY_JUG_B";

 
        bool MeasureWithTotalAB;
        bool MeasureWithA;
        bool MeasureWithB;
        bool MeasureWithMultipleSums;
        bool MeasureWithDiff;


        public WaterChallenge(int ASize, int BSize, int ZSize)
        {
            A = ASize; B = BSize; Z = ZSize;
        }

       
        public List<WaterAction> GetActionsSolution()
        {
            //Has no solution
            if (Z > (A + B))
            {
                return Actions;
            }


            AnalizeDifferentsSolutions();

          
            if (!MeasureWithA && !MeasureWithB && !MeasureWithTotalAB && !MeasureWithMultipleSums && !MeasureWithMultipleSums && !MeasureWithDiff)
            {
                //No solutions
                return Actions;
            }

            //Solve with the best solution
            if (MeasureWithA) { FillJugA(); } 
            else if (MeasureWithB) { FillJugB(); }
            else if (MeasureWithTotalAB) { SolveWithTotalAB(); }
            else if (MeasureWithMultipleSums) { SolveWithMultipleSums(); }
            else if (MeasureWithDiff) { SolveWithDiff();  }

            return Actions;
        }

        private bool IsDivisable(int dividend, int divisor)
        {
            if (divisor == 0) return false;

            bool result = dividend % divisor == 0;

            return result;
        }

        void FillJugA()
        {
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
        void FillJugB()
        {
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

        
        void TransferAB()
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
        void TransferBA()
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

        bool IsTheExactMeasure()
        {
            if (WaterInA == Z) return true;
            if (WaterInB == Z) return true;
            if (WaterInA + WaterInB == Z) return true;
            return false;
        }

        void SolveMeasureWithDiffModuleAB()
        {

            while (!IsTheExactMeasure())
            {
                FillJugB();
                TransferBA();

                if (WaterInB != Z)
                {
                    EmptyJugA();
                    TransferBA();
                }
                else
                {
                    EmptyJugA();
                }
            }
            
        }

        void SolveMeasureWithDiffModuleOneAB()
        {

            while (!IsTheExactMeasure())
            {
                FillJugA();
                TransferAB();

                if(WaterInA == Z)
                {
                    EmptyJugB();
                }else if (WaterInB == B)
                {
                    EmptyJugB();
                    TransferAB();
                }
            }

        }
        void SolveMeasureWithDiffModuleOneBA()
        {
            while (!IsTheExactMeasure())
            {
                FillJugB();
                TransferBA();

                if (WaterInB == Z)
                {
                    EmptyJugA();
                }
                else if (WaterInA == A)
                {
                    EmptyJugA();
                    TransferBA();
                }
            }
        }


        void SolveMeasureWithDiffModuleBA()
        {
            while (!IsTheExactMeasure())
            {
                FillJugA();
                TransferAB();

                if (WaterInA != Z)
                {
                    EmptyJugB();
                    TransferAB();
                }
                else
                {
                    EmptyJugB();
                }
            }

        }

        void AnalizeDifferentsSolutions()
        {
            //can fill with total of jug A and B
            MeasureWithTotalAB = IsDivisable(Z, A + B);
            //can fill with Jug A?
            MeasureWithA = Z == A;
            //can fill with Jug B?
            MeasureWithB = Z == B;
            //Multiple add measureWithMultipleAddAB
            MeasureWithMultipleSums = (Z % A == 0 && Z <= B ) || (Z % B == 0 && Z <= A);

            MeasureWithDiff = IsDivisable(Z, Math.Abs(A - B));
        }

        void SolveWithTotalAB()
        {
            FillJugA();
            FillJugB();
        }
        void SolveWithMultipleSums()
        {
            if (A < B)
            {
                while (WaterInB < Z)
                {
                    FillJugA();
                    TransferAB();

                }
            }
            else
            {
                while (WaterInA < Z)
                {
                    FillJugB();
                    TransferBA();

                }
            }
        }


        void SolveWithDiff()
        {
            bool diffBetwenJugsIsOne = (Math.Abs(A - B) == 1);

            if (diffBetwenJugsIsOne)
            {
                //Start filling the smaller jug
                if (A < B)
                {
                    SolveMeasureWithDiffModuleOneAB();
                }
                else
                {
                    SolveMeasureWithDiffModuleOneBA();
                }
            }
            else
            {
                //Start filling the greater jug
                if (A < B)
                {
                    SolveMeasureWithDiffModuleAB();
                }
                else
                {
                    SolveMeasureWithDiffModuleBA();
                }
            }
        }
    }
}