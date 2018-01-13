using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace NumberGenerator
{
    class GenerateNumber
    {
        public int TargetNumber { get; set; }
        public string Solution { get; set; }
        public long TotalExp { get; private set; }
        private string[] OpList = { "+", "-", "*", "/" };

        public bool Generate()
        {
            // set the total expression into
            this.TotalExp = 0;
            // call the CreateMathString function that will generate the string that will express the Math
            return this.CreateMathString(0, "", 0);
        }

        private bool CreateMathString(int CurrNum, string CurrExp, int Function)
        {
            string TempCurrExp;
            // we should try all operator and then try add the next number
            // if eval is correct then we just need to stop here, if not we need to continue, until
            // we finished all the CurrNum

            // log the current expression we check
            // Console.WriteLine(CurrNum + " " + Function + " -> " + CurrExp);

            // switch the function that we will do, whether we will add operator, or add number
            switch(Function)
            {
                case 0:
                    // for the first string, we can only add the + and - operator (0, and 1) only from the operator list
                    // so check if this is the first digit or not?
                    if (CurrExp.Length == 0)
                    {
                        // add operator +
                        TempCurrExp = CurrExp + OpList[0];
                        if(this.CreateMathString(CurrNum, TempCurrExp, 1))
                        {
                            return true;
                        }
                        else
                        {
                            // add operator - now
                            TempCurrExp = CurrExp + OpList[1];
                            if(this.CreateMathString(CurrNum, TempCurrExp, 1))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // try for all operator
                        // add operator +
                        TempCurrExp = CurrExp + OpList[0];
                        if (this.CreateMathString(CurrNum, TempCurrExp, 1))
                        {
                            return true;
                        }
                        else
                        {
                            // add operator -
                            TempCurrExp = CurrExp + OpList[1];
                            if (this.CreateMathString(CurrNum, TempCurrExp, 1))
                            {
                                return true;
                            }
                            else
                            {
                                TempCurrExp = CurrExp + OpList[2];
                                if (this.CreateMathString(CurrNum, TempCurrExp, 1))
                                {
                                    return true;
                                }
                                else
                                {
                                    TempCurrExp = CurrExp + OpList[3];
                                    if (this.CreateMathString(CurrNum, TempCurrExp, 1))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    // add new CurrNum if we add number to the string
                    CurrNum = CurrNum + 1;

                    // if Current Number is > 9 already (means we already used all the number), then try to evaluate
                    // and check the number.
                    if (CurrNum == 9)
                    {
                        // evaluate the current math expression
                        string ExpResult;
                        int iExpResult;

                        // add the latest digit to current expression before we compute it
                        CurrExp = CurrExp + CurrNum.ToString();
                        this.TotalExp = this.TotalExp + 1;

                        try
                        {
                            ExpResult = new DataTable().Compute(CurrExp, null).ToString();

                            int.TryParse(ExpResult, out iExpResult);
                            if (iExpResult == this.TargetNumber)
                            {
                                this.Solution = CurrExp;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception err)
                        {
                            // skip this, since this function cannot be computed
                            Console.WriteLine("Cannot Compute: " + CurrExp);
                            return false;
                        }
                    }
                    else
                    {
                        TempCurrExp = CurrExp + CurrNum.ToString();
                        // after number we can either add new operator or new number, so called the expression to add new operator and number again
                        if (this.CreateMathString(CurrNum, TempCurrExp, 0))
                        {
                            return true;
                        }
                        else
                        {
                            if (this.CreateMathString(CurrNum, TempCurrExp, 1))
                            {
                                return true;
                            }
                        }
                    }
                    break;
            }

            // default to return false
            return false;
        }
    }
}
