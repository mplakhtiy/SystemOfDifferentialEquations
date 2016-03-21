using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemOfDifferentialEquations.Source;

namespace SystemOfDifferentialEquations
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();
            chart3.Series[2].Points.Clear();
            chart3.Series[3].Points.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            double startOfInterval = double.Parse(textBox1.Text);
            double endOfInterval = double.Parse(textBox2.Text);
            int coutOfNodes = int.Parse(textBox3.Text);
            double eps = double.Parse(textBox4.Text);
            Vector startVector = new Vector(new[] {0.027, 1.1});
            SystemCauchyProblems scp = new SystemCauchyProblems(startVector,startOfInterval,endOfInterval,coutOfNodes,eps);
            List<Vector> solutionsByEulerMethod = scp.GetSolutionsByEulerMethod();
            List<Vector> solutionsByCrankNicolsonMethod = scp.GetSolutionsByCrankNicolsonMethod();
            for (int i = 0; i < coutOfNodes; i++)
            {
                double node = scp.CalculateNode(i);
                dataGridView1.Rows.Add(i, solutionsByEulerMethod[i][0], solutionsByEulerMethod[i][1], node);
                chart1.Series[0].Points.AddXY(node, solutionsByEulerMethod[i][0]);
                chart1.Series[1].Points.AddXY(node, solutionsByEulerMethod[i][1]);
                dataGridView2.Rows.Add(i, solutionsByCrankNicolsonMethod[i][0], solutionsByCrankNicolsonMethod[i][1],
                    node);
                chart2.Series[0].Points.AddXY(node, solutionsByCrankNicolsonMethod[i][0]);
                chart2.Series[1].Points.AddXY(node, solutionsByCrankNicolsonMethod[i][1]);
                dataGridView3.Rows.Add(i, solutionsByEulerMethod[i][0] - solutionsByCrankNicolsonMethod[i][0], solutionsByEulerMethod[i][1] - solutionsByCrankNicolsonMethod[i][1],
                    node);
                chart3.Series[0].Points.AddXY(node, solutionsByEulerMethod[i][0]);
                chart3.Series[1].Points.AddXY(node, solutionsByEulerMethod[i][1]);
                chart3.Series[2].Points.AddXY(node, solutionsByCrankNicolsonMethod[i][0]);
                chart3.Series[3].Points.AddXY(node, solutionsByCrankNicolsonMethod[i][1]);

            }

        }
    }
}
