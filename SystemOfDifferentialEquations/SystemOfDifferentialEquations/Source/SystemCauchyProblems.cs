using System;
using System.Collections.Generic;

namespace SystemOfDifferentialEquations.Source
{
    class SystemCauchyProblems
    {
        public static Vector f(Vector x, double t)//System of differential equations.System Cauchy problems.
        {
            double[] functions = new double[2];

            functions[0] = x[1];
            functions[1] = -Math.Pow(x[0], 3) - 0.2 * x[1] + 0.3 * Math.Cos(t);
            return new Vector(functions);
        }

        public static Matrix F(Vector x)//Jacobian of system Cauchy problems.
        {
            double[,] functions = new double[2, 2];
            functions[0, 0] = 0;
            functions[0, 1] = 1;
            functions[1, 0] = -3 * Math.Pow(x[0], 2);
            functions[1, 1] = -0.2;
            return new Matrix(functions);
        }

        public Matrix StateTransitionMatrix(List<Vector> soluthions, double h) //this matrix is olready Inversed
        {
            Matrix stateTransitionMatrix = new Matrix(soluthions[0].Length);
            stateTransitionMatrix.IdentityMatrix();
            Matrix I = new Matrix(soluthions[0].Length);
            I.IdentityMatrix();

            for (int i = 1; i < soluthions.Count; i++)
            {
                stateTransitionMatrix *= (I - (h * (F(soluthions[i]))));
            }

            return stateTransitionMatrix;
        }

        public List<Vector> EulerMethodForSystem(Vector startVector, double startOfInterval, double endOfInterval, double numberOfNodes, double eps)
        {
            Matrix I = new Matrix(startVector.Length);
            I.IdentityMatrix();

            List<Vector> soluthions = new List<Vector> { startVector };

            int numberOfCurrentSolution = 0;
            double step = (endOfInterval - startOfInterval) / (numberOfNodes - 1); //(numberOfNodes-1) because we already know solution at first node.
            double currentNode = startOfInterval + step;

            while (currentNode < endOfInterval + step)
            {
                numberOfCurrentSolution++;
                Vector tempSoluthion = new Vector(soluthions[numberOfCurrentSolution - 1]);
                Vector checkSoluthion;
                soluthions.Add(new Vector(startVector.Length));

                do
                {

                    checkSoluthion = new Vector(tempSoluthion);
                    Matrix matrix = new Matrix((I - (step * F(tempSoluthion))));
                    Vector vector = new Vector((tempSoluthion - soluthions[numberOfCurrentSolution - 1] - step * f(tempSoluthion, currentNode)));
                    soluthions[numberOfCurrentSolution] = tempSoluthion - matrix.Inverse() * vector;
                    tempSoluthion = soluthions[numberOfCurrentSolution];

                } while (Math.Abs(soluthions[numberOfCurrentSolution][0] - checkSoluthion[0]) > eps &Math.Abs(soluthions[numberOfCurrentSolution][1] - checkSoluthion[1]) > eps);
               
                currentNode += step;
            }
            return soluthions;
        }

        public List<Vector> CrankNicolsonMethodForSystem(Vector startVector, double startOfInterval, double endOfInterval, double numberOfNodes, double eps)
        {
            Matrix I = new Matrix(startVector.Length);
            I.IdentityMatrix();

            List<Vector> soluthions = new List<Vector> {startVector};

            int numberOfCurrentSolution = 0;
            double step = (endOfInterval - startOfInterval)/(numberOfNodes - 1);//(numberOfNodes-1) because we already know solution at first node.
            double currentPoint = startOfInterval + step;

            while (currentPoint < endOfInterval + step)
            {
                numberOfCurrentSolution++;
                Vector tempSoluthion = new Vector(soluthions[numberOfCurrentSolution - 1]);
                Vector checkSoluthion;
                soluthions.Add(new Vector(startVector.Length));

                do
                {

                    checkSoluthion = new Vector(tempSoluthion);
                    Matrix matrix = new Matrix((I - ((step/2)*F(tempSoluthion))));
                    Vector vector = new Vector((tempSoluthion - soluthions[numberOfCurrentSolution - 1] -(step/2)*((f(soluthions[numberOfCurrentSolution - 1], currentPoint - step) + f(tempSoluthion, currentPoint)))));
                    soluthions[numberOfCurrentSolution] = tempSoluthion - matrix.Inverse()*vector;
                    tempSoluthion = soluthions[numberOfCurrentSolution];

                } while (Math.Abs(soluthions[numberOfCurrentSolution][0] - checkSoluthion[0]) > eps & Math.Abs(soluthions[numberOfCurrentSolution][1] - checkSoluthion[1]) > eps);
                
                currentPoint += step;
            }

            return soluthions;
        }

        public Vector NewApproximateOftTheInitialValue(Matrix stateTransitionMatrix, Vector startVector, List<Vector> systemSoluthions)//Accelerated Newton method.
        {
            Matrix I = new Matrix(stateTransitionMatrix.Length);
            I.IdentityMatrix();

            Matrix matrix = new Matrix((stateTransitionMatrix - I));
            Vector vector = new Vector(stateTransitionMatrix * systemSoluthions[systemSoluthions.Count - 1] - startVector);
            Vector newVector = matrix.Inverse() * vector;

            return newVector;
        }
    }
}
