using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumberGenerator
{
    public partial class frmInputNumber : Form
    {
        // delegate function to update the label on the form from another thread
        delegate void SetNumberOfExp(string ExpNum);
        delegate void SetSolution(string Solution);
        delegate void EnabledFormButton();
        delegate void EnabledFormText();

        private GenerateNumber clsGen;

        public frmInputNumber()
        {
            InitializeComponent();
        }

        private void frmInputNumber_Load(object sender, EventArgs e)
        {
            this.clsGen = new GenerateNumber();

            // for testing purpose only
            // Console.WriteLine(new DataTable().Compute("2^3", null).ToString());
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            // first check if target number is inputted?
            if (this.txtTargetNumber.Text.Length > 0)
            {
                // try to convert the text number and ensure that this is still below 11000
                int TargetNumber = -1;
                try
                {
                    TargetNumber = int.Parse(this.txtTargetNumber.Text);
                    // now check if the target number is valid or not?

                    if (TargetNumber >= 11000)
                    {
                        // give warning that the maximum target number is 11000
                        MessageBox.Show("Maximum target number value is 11000.", "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // set the target number on the GenerateNumber class
                        this.clsGen.TargetNumber = TargetNumber;
                        this.lblNumOfExp.Text = "Working...";
                        this.txtTargetNumber.Enabled = false;
                        this.btnCheck.Enabled = false;

                        // create new task for the generate class
                        new Task(CheckSolution).Start();
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Invalid number inputted.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CheckSolution()
        {
            if (this.clsGen.Generate())
            {
                SetSolutionText("Solution: " + this.clsGen.Solution);
            }
            else
            {
                SetSolutionText("No solution available.");
            }
            SetNumOfExpLabel("Checking " + this.clsGen.TotalExp.ToString() + " of Math Expression.");
            EnabledFormButtonClick();
            EnabledFormTextTarget();
        }

        public void SetNumOfExpLabel(string ExpNum)
        {
            // check whether the label is invoked or not from the thread
            if (this.lblNumOfExp.InvokeRequired)
            {
                SetNumberOfExp d = new SetNumberOfExp(SetNumOfExpLabel);
                this.lblNumOfExp.Invoke(d, new object[] { ExpNum });
            }
            else
            {
                this.lblNumOfExp.Text = ExpNum;
            }
        }

        public void SetSolutionText(string Solution)
        {
            if (this.lblSolution.InvokeRequired)
            {
                SetSolution d = new SetSolution(SetSolutionText);
                this.lblSolution.Invoke(d, new object[] { Solution });
            }
            else
            {
                this.lblSolution.Text = Solution;
            }
        }

        public void EnabledFormButtonClick()
        {
            if (this.btnCheck.InvokeRequired)
            {
                EnabledFormButton d = new EnabledFormButton(EnabledFormButtonClick);
                this.btnCheck.Invoke(d, new object[] { });
            }
            else
            {
                this.btnCheck.Enabled = true;
            }
        }

        public void EnabledFormTextTarget()
        {
            if (this.txtTargetNumber.InvokeRequired)
            {
                EnabledFormText d = new EnabledFormText(EnabledFormTextTarget);
                this.txtTargetNumber.Invoke(d, new object[] { });
            }
            else
            {
                this.txtTargetNumber.Enabled = true;
            }
        }

    }
}
