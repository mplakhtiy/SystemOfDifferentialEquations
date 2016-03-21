using System;
using System.Collections.Generic;

namespace SystemOfDifferentialEquations.Source
{
    public class Matrix
    {
        private double[,] matrix { get; set; }
        public int Length { get; private set; }

        public double this[int i, int j]
        {
            get
            {
                if (Length <= 0) throw new ArgumentException("Matrix not Find!");
                if ((i > -1 & j > -1) & (i < Length & j < Length))
                {
                    return matrix[i, j];
                }
                throw new ArgumentException("Wrong indexes!");
            }
            set
            {
                if (Length <= 0) throw new ArgumentException("Matrix not Find!");
                if ((i > -1 & j > -1) & (i < Length & j < Length))
                {
                    matrix[i, j] = value;
                }
                else throw new ArgumentException("Wrong indexes!");
            }
        }

        #region Constructors

        public Matrix(int size)
        {
            Length = size;
            ZeroMatrix();
        }

        public Matrix(double[,] matrixInput)
        {
            Length = (int)Math.Sqrt(matrixInput.Length);
            matrix = matrixInput;
        }

        public Matrix(Matrix inputMatrix)
        {
            Length = inputMatrix.Length;
            matrix = copyArray(inputMatrix.matrix);
        }

        #endregion

        #region Public Methods

        public Matrix IdentityMatrix()//This method change current object
        {

            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    matrix[i, j] = (i == j) ? 1 : 0;
                }
            }

            return this;
        }

        public Matrix ZeroMatrix()//This method change current object
        {
            matrix = new double[Length, Length];

            return this;
        }

        public Matrix Transpose()//This method return new object
        {
            Matrix returnMatrix = new Matrix(Length);

            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    returnMatrix[i, j] = this[j, i];
                }
            }

            return returnMatrix;
        }

        public Matrix Inverse()//This method return new object
        {
            double eps = Math.Pow(10, -5);
            double determinant = Determinant();
            Matrix adjugateMatrix = AdjugateMatrix();
            Matrix returnMatrix = new Matrix((1 / determinant) * adjugateMatrix);
            double firstValueOfMatrix = (this * (returnMatrix))[0, 0];
            bool ifIndentity = Math.Abs(firstValueOfMatrix - 1) < eps;

            if (!ifIndentity)
            {
                return returnMatrix / firstValueOfMatrix;
            }

            return returnMatrix;
        }

        public double Determinant()
        {
            //As default we choose first column as minor
            return Determinant(true, 0);
        }

        public double Determinant(bool isColumnMinor, int minor)
        {
            if (Length < 2)
            {
                return this[0, 0];
            }

            if (Length == 2)
            {
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }

            Matrix tempMatrix = new Matrix(Length - 1);
            Matrix resultMatrix = new Matrix(this);
            double determinant = new double();

            for (int i = 0; i < tempMatrix.Length; i++)
            {
                for (int k = 0; k < resultMatrix.Length; k++)
                {
                    if (isColumnMinor)
                    {
                        if ((k + minor) % 2 == 0)
                            determinant += resultMatrix[k, minor] * cutMatrixCross(resultMatrix, k, minor).Determinant();
                        else
                        {
                            determinant -= resultMatrix[k, minor] * cutMatrixCross(resultMatrix, k, minor).Determinant();
                        }
                    }
                    else
                    {
                        if ((k + minor) % 2 == 0)
                            determinant += resultMatrix[minor, k] * cutMatrixCross(resultMatrix, minor, k).Determinant();
                        else
                        {
                            determinant -= resultMatrix[minor, k] * cutMatrixCross(resultMatrix, minor, k).Determinant();
                        }
                    }
                }
                resultMatrix = tempMatrix;
                tempMatrix = new Matrix(resultMatrix.Length);
            }

            return determinant;

        }

        public Matrix AdjugateMatrix()//This method return new object
        {
            Matrix returnMatrix = new Matrix(Length);

            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if ((i + j) % 2 == 0)
                        returnMatrix[i, j] = cutMatrixCross(this, i, j).Determinant();
                    else
                    {
                        returnMatrix[i, j] = -cutMatrixCross(this, i, j).Determinant();
                    }

                }
            }

            return returnMatrix.Transpose();
        }
        #endregion

        #region Operators

        //This operators returns new object without changes to input parameters

        public static Matrix operator *(Matrix inputMatrixA, Matrix inputMatrixB)
        {
            if (inputMatrixA.Length != inputMatrixB.Length)
                throw new ArgumentException("Diferent Length");

            Matrix returnMatrix = new Matrix(inputMatrixA.Length);

            for (int i = 0; i < inputMatrixA.Length; i++)
            {
                for (int j = 0; j < inputMatrixB.Length; j++)
                {
                    for (int k = 0; k < returnMatrix.Length; k++)
                    {
                        returnMatrix[i, j] += inputMatrixA[i, k] * inputMatrixB[k, j];
                    }
                }
            }

            return returnMatrix;
        }

        public static Vector operator *(Matrix inputMatrix, Vector inputVector)
        {
            if (inputMatrix.Length != inputVector.Length)
                throw new ArgumentException("Diferent Length");

            Vector returnVector = new Vector(inputMatrix.Length);

            for (int i = 0; i < inputMatrix.Length; i++)
            {
                for (int j = 0; j < inputVector.Length; j++)
                {
                    returnVector[i] += inputMatrix[i, j] * inputVector[j];
                }
            }

            return returnVector;
        }

        public static Matrix operator *(Vector inputVector, Matrix inputMatrix)
        {
            if (inputVector.Length != inputMatrix.Length)
                throw new ArgumentException("Diferent Matrix Length");

            Matrix returnMatrix = new Matrix(inputVector.Length);

            for (int i = 0; i < inputVector.Length; i++)
            {
                for (int j = 0; j < inputMatrix.Length; j++)
                {
                    for (int k = 0; k < returnMatrix.Length; k++)
                    {
                        returnMatrix[i, j] += inputVector[i] * inputMatrix[k, j];
                    }
                }
            }

            return returnMatrix;
        }

        public static Matrix operator *(double inputValue, Matrix inputMatrix)
        {
            Matrix returnMatrix = new Matrix(inputMatrix.Length);

            for (int i = 0; i < inputMatrix.Length; i++)
            {
                for (int j = 0; j < inputMatrix.Length; j++)
                {
                    returnMatrix[i, j] = inputMatrix[i, j] * inputValue;
                }
            }

            return returnMatrix;
        }

        public static Matrix operator /(Matrix inputMatrix, double inputValue)
        {
            Matrix returnMatrix = new Matrix(inputMatrix.Length);

            for (int i = 0; i < inputMatrix.Length; i++)
            {
                for (int j = 0; j < inputMatrix.Length; j++)
                {
                    returnMatrix[i, j] = inputMatrix[i, j] / inputValue;
                }
            }

            return returnMatrix;
        }

        public static Matrix operator +(Matrix inputMatrixA, Matrix inputMatrixB)
        {
            if (inputMatrixA.Length != inputMatrixB.Length)
                throw new ArgumentException("Diferent Length");

            Matrix returnMatrix = new Matrix(inputMatrixA.Length);

            for (int i = 0; i < inputMatrixA.Length; i++)
            {
                for (int j = 0; j < inputMatrixB.Length; j++)
                {
                    returnMatrix[i, j] = inputMatrixA[i, j] + inputMatrixB[i, j];
                }
            }

            return returnMatrix;
        }

        public static Matrix operator -(Matrix inputMatrixA, Matrix inputMatrixB)
        {
            if (inputMatrixA.Length != inputMatrixB.Length)
                throw new ArgumentException("Diferent Length");

            Matrix returnMatrix = new Matrix(inputMatrixA.Length);

            for (int i = 0; i < inputMatrixA.Length; i++)
            {
                for (int j = 0; j < inputMatrixB.Length; j++)
                {
                    returnMatrix[i, j] = inputMatrixA[i, j] - inputMatrixB[i, j];
                }
            }

            return returnMatrix;
        }

        #endregion

        #region Private Methods

        private Matrix cutMatrixCross(Matrix inputMatrix, int row, int column)
        {
            List<double> returnMatrixList = new List<double>();

            for (int i = 0; i < inputMatrix.Length; i++)
            {
                for (int j = 0; j < inputMatrix.Length; j++)
                {
                    if (i != row && j != column)
                    {
                        returnMatrixList.Add(inputMatrix[i, j]);
                    }
                }
            }

            return arrayToMatrix(returnMatrixList.ToArray());
        }

        private Matrix arrayToMatrix(double[] inputArray)
        {

            int matrixLength = (int)Math.Sqrt(inputArray.Length);
            double[,] returnMatrix = new double[matrixLength, matrixLength];
            int k = 0;

            for (int i = 0; i < matrixLength; i++)
            {
                for (int j = 0; j < matrixLength; j++)
                {
                    returnMatrix[i, j] = inputArray[k];
                    k++;
                }

            }

            return new Matrix(returnMatrix);
        }

        private double[,] copyArray(double[,] inputArray)
        {
            int matrixLength = (int)Math.Sqrt(inputArray.Length);
            double[,] returnMatrix = new double[matrixLength, matrixLength];

            for (int i = 0; i < matrixLength; i++)
            {
                for (int j = 0; j < matrixLength; j++)
                {
                    returnMatrix[i, j] = inputArray[i, j];
                }
            }

            return returnMatrix;
        }

        #endregion
    }
}
