using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public class DreidelLetters : BaseDrawableComponent
    {
        private const int k_TrianglesNum = 34;
        private const int k_VerticesNum = 52;
        private const int k_NLetterStartIndex = 0;
        private const int k_HLetterStartIndex = 10;
        private const int k_PLetterStartIndex = 21;
        private const int k_GLetterStartIndex = 38;

        private const int k_NLetterIndicesStartInd = 0;
        private const int k_HLetterIndicesStartInd = 24;
        private const int k_PLetterIndicesStartInd = 42;
        private const int k_GLetterIndicesStartInd = 72;        
        private readonly Color r_Color = Color.Black;

        private const float k_LetterSpace = .01f;
        protected const float k_ZFactorCoordinate = 3;

        private VertexPositionColor[] m_Vertices;
        private int[] m_Indices;

        /// <summary>
        /// Gets the number of triangles the letters have
        /// </summary>
        public override int     TriangleNum
        {
            get { return k_TrianglesNum; }
        }

        public DreidelLetters(Game i_Game)
            : base(i_Game)
        {
        }

        /// <summary>
        /// Initialize the component vertices and buffers
        /// </summary>
        protected override void     LoadContent()
        {
            base.LoadContent();

            createVertices();
            createIndices();
            InitBuffers();
        }

        /// <summary>
        /// Creates the component vertices
        /// </summary>
        private void createVertices()
        {
            m_Vertices = new VertexPositionColor[k_VerticesNum];

            createLetters();

            this.VerticesNum = k_VerticesNum;
        }

        /// <summary>
        /// Creates the component indices that the index buffer uses
        /// </summary>
        private void createIndices()
        {
            createLettersIndices();

            BufferIndices = m_Indices ;
        }

        /// <summary>
        /// Initialize the VertexBuffer and IndexBuffer components.
        /// </summary>
        public override void InitBuffers()
        {
            VertexBuffer vBuffer;
            IndexBuffer iBuffer;

            vBuffer = new VertexBuffer(
                this.GraphicsDevice,
                VertexPositionColor.SizeInBytes * m_Vertices.Length,
                BufferUsage.WriteOnly);

            vBuffer.SetData<VertexPositionColor>(m_Vertices, 0, m_Vertices.Length);

            iBuffer = new IndexBuffer(
                this.GraphicsDevice,
                typeof(int),
                this.BufferIndices.Length,
                BufferUsage.WriteOnly);

            iBuffer.SetData<int>(this.BufferIndices);

            this.ComponentVertexBuffer = vBuffer;
            this.ComponentIndexBuffer = iBuffer;
        }

        /// <summary>
        /// Initialize the letters vertices
        /// </summary>
        private void    createLetters()
        {
            createNLetter();
            createHLetter();
            createPLetter();
            createGLetter();
        }

        /// <summary>
        /// Create the letters indices
        /// </summary>
        private void    createLettersIndices()
        {
            // TODO: Change to constant

            m_Indices = new int[101];

            initNLetterIndices();
            initHLetterIndices();
            initPLetterIndices();
            initGLetterIndices();
        }

        /// <summary>
        /// Initialize the "�" letter and adds it to the cube elements
        /// </summary>
        private void    createNLetter()
        {
            m_Vertices[k_NLetterStartIndex] = new VertexPositionColor(
                new Vector3(-2, -2.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 1] = new VertexPositionColor(
                new Vector3(-2, -1.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 2] = new VertexPositionColor(
                new Vector3(1, -1.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 3] = new VertexPositionColor(
                new Vector3(2, -1.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 4] = new VertexPositionColor(
                new Vector3(2, -2.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 5] = new VertexPositionColor(
                new Vector3(1, 1.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 6] = new VertexPositionColor(
                new Vector3(1, 2.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 7] = new VertexPositionColor(
                new Vector3(2, 2.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 8] = new VertexPositionColor(
                new Vector3(0, 2.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);
            m_Vertices[k_NLetterStartIndex + 9] = new VertexPositionColor(
                new Vector3(0, 1.5f, k_ZFactorCoordinate + k_LetterSpace),
                r_Color);

            // TODO: Remove

            /*Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                3,
                new VertexPositionColor(m_NLetterCoordinates[0], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[1], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[3], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[4], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_NLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[7], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[4], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                1,
                new VertexPositionColor(m_NLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[6], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[7], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_NLetterCoordinates[9], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[8], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[6], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[5], Color.Black)));*/
        }

        /// <summary>
        /// Initialize the "�" letter coordinates
        /// </summary>
        private void    initNLetterIndices()
        {
            m_Indices[k_NLetterIndicesStartInd] = k_NLetterStartIndex;
            m_Indices[k_NLetterIndicesStartInd + 1] = k_NLetterStartIndex + 1;
            m_Indices[k_NLetterIndicesStartInd + 2] = k_NLetterStartIndex + 2;
            m_Indices[k_NLetterIndicesStartInd + 3] = k_NLetterStartIndex;
            m_Indices[k_NLetterIndicesStartInd + 4] = k_NLetterStartIndex + 2;
            m_Indices[k_NLetterIndicesStartInd + 5] = k_NLetterStartIndex + 3;
            m_Indices[k_NLetterIndicesStartInd + 6] = k_NLetterStartIndex;
            m_Indices[k_NLetterIndicesStartInd + 7] = k_NLetterStartIndex + 3;
            m_Indices[k_NLetterIndicesStartInd + 8] = k_NLetterStartIndex + 4;

            m_Indices[k_NLetterIndicesStartInd + 9] = k_NLetterStartIndex + 2;
            m_Indices[k_NLetterIndicesStartInd + 10] = k_NLetterStartIndex + 5;
            m_Indices[k_NLetterIndicesStartInd + 11] = k_NLetterStartIndex + 7;
            m_Indices[k_NLetterIndicesStartInd + 12] = k_NLetterStartIndex + 2;
            m_Indices[k_NLetterIndicesStartInd + 13] = k_NLetterStartIndex + 7;
            m_Indices[k_NLetterIndicesStartInd + 14] = k_NLetterStartIndex + 4;

            m_Indices[k_NLetterIndicesStartInd + 15] = k_NLetterStartIndex + 5;
            m_Indices[k_NLetterIndicesStartInd + 16] = k_NLetterStartIndex + 6;
            m_Indices[k_NLetterIndicesStartInd + 17] = k_NLetterStartIndex + 7;

            m_Indices[k_NLetterIndicesStartInd + 18] = k_NLetterStartIndex + 9;
            m_Indices[k_NLetterIndicesStartInd + 19] = k_NLetterStartIndex + 8;
            m_Indices[k_NLetterIndicesStartInd + 20] = k_NLetterStartIndex + 6;
            m_Indices[k_NLetterIndicesStartInd + 21] = k_NLetterStartIndex + 9;
            m_Indices[k_NLetterIndicesStartInd + 22] = k_NLetterStartIndex + 6;
            m_Indices[k_NLetterIndicesStartInd + 23] = k_NLetterStartIndex + 5;
        }

        /// <summary>
        /// Initialize the "�" letter and adds it to the cube elements
        /// </summary>
        private void    createHLetter()
        {
            m_Vertices[k_HLetterStartIndex] = new VertexPositionColor( 
                new Vector3(-2, -2.5f, -k_ZFactorCoordinate - k_LetterSpace),
                r_Color);
            m_Vertices[k_HLetterStartIndex + 1] = new VertexPositionColor( 
                new Vector3(-1, -2.5f, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 2] = new VertexPositionColor( 
                new Vector3(-2, 2.5f, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 3] = new VertexPositionColor( 
                new Vector3(-1, 1.5f, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 4] = new VertexPositionColor( 
                new Vector3(-1, 2.5f, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 5] = new VertexPositionColor( 
                new Vector3(2, 2.5f, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 6] = new VertexPositionColor( 
                new Vector3(2, 1.5f, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 7] = new VertexPositionColor( 
                new Vector3(1, 0, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 8] = new VertexPositionColor( 
                new Vector3(2, 0, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 9] = new VertexPositionColor( 
                new Vector3(1, -2.5f, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);
            m_Vertices[k_HLetterStartIndex + 10] = new VertexPositionColor( 
                new Vector3(2, -2.5f, -k_ZFactorCoordinate - k_LetterSpace), 
                r_Color);

            /*Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_HLetterCoordinates[1], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[4], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[0], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_HLetterCoordinates[6], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[4], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[3], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_HLetterCoordinates[10], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[8], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[7], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[9], Color.Black)));*/
        }

        /// <summary>
        /// Initialize the "�" letter coordinates
        /// </summary>
        private void    initHLetterIndices()
        {
            m_Indices[k_HLetterIndicesStartInd] = k_HLetterStartIndex + 1;
            m_Indices[k_HLetterIndicesStartInd + 1] = k_HLetterStartIndex + 4;
            m_Indices[k_HLetterIndicesStartInd + 2] = k_HLetterStartIndex + 2;
            m_Indices[k_HLetterIndicesStartInd + 3] = k_HLetterStartIndex + 1;
            m_Indices[k_HLetterIndicesStartInd + 4] = k_HLetterStartIndex + 2;
            m_Indices[k_HLetterIndicesStartInd + 5] = k_HLetterStartIndex;

            m_Indices[k_HLetterIndicesStartInd + 6] = k_HLetterStartIndex + 6;
            m_Indices[k_HLetterIndicesStartInd + 7] = k_HLetterStartIndex + 5;
            m_Indices[k_HLetterIndicesStartInd + 8] = k_HLetterStartIndex + 4;
            m_Indices[k_HLetterIndicesStartInd + 9] = k_HLetterStartIndex + 6;
            m_Indices[k_HLetterIndicesStartInd + 10] = k_HLetterStartIndex + 4;
            m_Indices[k_HLetterIndicesStartInd + 11] = k_HLetterStartIndex + 3;

            m_Indices[k_HLetterIndicesStartInd + 12] = k_HLetterStartIndex + 10;
            m_Indices[k_HLetterIndicesStartInd + 13] = k_HLetterStartIndex + 8;
            m_Indices[k_HLetterIndicesStartInd + 14] = k_HLetterStartIndex + 7;
            m_Indices[k_HLetterIndicesStartInd + 15] = k_HLetterStartIndex + 10;
            m_Indices[k_HLetterIndicesStartInd + 16] = k_HLetterStartIndex + 7;
            m_Indices[k_HLetterIndicesStartInd + 17] = k_HLetterStartIndex + 9;
        }

        /// <summary>
        /// Initialize the "�" letter and adds it to the cube elements
        /// </summary>
        private void    createGLetter()
        {
            m_Vertices[k_GLetterStartIndex] = new VertexPositionColor(
                new Vector3(-3 - k_LetterSpace, -2.5f, -2), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 1] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, 0, -2), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 2] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, 0, -1), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 3] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, -1, -1), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 4] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, -2.5f, -1),
                r_Color);
            m_Vertices[k_GLetterStartIndex + 5] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, -2.5f, 1), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 6] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, -1, 1), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 7] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, 0, 1), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 8] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, 1.5f, 1), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 9] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, 1.5f, -2), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 10] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, 2.5f, -2), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 11] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, 2.5f, 1), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 12] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, 2.5f, 2), 
                r_Color);
            m_Vertices[k_GLetterStartIndex + 13] = new VertexPositionColor( 
                new Vector3(-3 - k_LetterSpace, -2.5f, 2), 
                r_Color);

            /*Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_GLetterCoordinates[0], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[1], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[4], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_GLetterCoordinates[3], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[7], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[6], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_GLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[11], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[12], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[13], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_GLetterCoordinates[9], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[10], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[11], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[8], Color.Black)));*/
        }

        /// <summary>
        /// Initialize the "�" letter coordinates
        /// </summary>
        private void    initGLetterIndices()
        {
            m_Indices[k_GLetterIndicesStartInd] = k_GLetterStartIndex;
            m_Indices[k_GLetterIndicesStartInd + 1] = k_GLetterStartIndex + 1;
            m_Indices[k_GLetterIndicesStartInd + 2] = k_GLetterStartIndex + 2;
            m_Indices[k_GLetterIndicesStartInd + 3] = k_GLetterStartIndex;
            m_Indices[k_GLetterIndicesStartInd + 4] = k_GLetterStartIndex + 2;
            m_Indices[k_GLetterIndicesStartInd + 5] = k_GLetterStartIndex + 4;

            m_Indices[k_GLetterIndicesStartInd + 6] = k_GLetterStartIndex + 3;
            m_Indices[k_GLetterIndicesStartInd + 7] = k_GLetterStartIndex + 2;
            m_Indices[k_GLetterIndicesStartInd + 8] = k_GLetterStartIndex + 7;
            m_Indices[k_GLetterIndicesStartInd + 9] = k_GLetterStartIndex + 3;
            m_Indices[k_GLetterIndicesStartInd + 10] = k_GLetterStartIndex + 7;
            m_Indices[k_GLetterIndicesStartInd + 11] = k_GLetterStartIndex + 6;

            m_Indices[k_GLetterIndicesStartInd + 12] = k_GLetterStartIndex + 5;
            m_Indices[k_GLetterIndicesStartInd + 13] = k_GLetterStartIndex + 11;
            m_Indices[k_GLetterIndicesStartInd + 14] = k_GLetterStartIndex + 12;
            m_Indices[k_GLetterIndicesStartInd + 15] = k_GLetterStartIndex + 5;
            m_Indices[k_GLetterIndicesStartInd + 16] = k_GLetterStartIndex + 12;
            m_Indices[k_GLetterIndicesStartInd + 17] = k_GLetterStartIndex + 13;

            m_Indices[k_GLetterIndicesStartInd + 18] = k_GLetterStartIndex + 9;
            m_Indices[k_GLetterIndicesStartInd + 19] = k_GLetterStartIndex + 10;
            m_Indices[k_GLetterIndicesStartInd + 20] = k_GLetterStartIndex + 11;
            m_Indices[k_GLetterIndicesStartInd + 21] = k_GLetterStartIndex + 9;
            m_Indices[k_GLetterIndicesStartInd + 22] = k_GLetterStartIndex + 11;
            m_Indices[k_GLetterIndicesStartInd + 23] = k_GLetterStartIndex + 8;         
        }

        /// <summary>
        /// Initialize the "�" letter and adds it to the cube elements
        /// </summary>
        private void    createPLetter()
        {
            m_Vertices[k_PLetterStartIndex] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, -2.5f, k_ZFactorCoordinate - 1),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 1] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, -1.5f, k_ZFactorCoordinate - 1),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 2] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, -1.5f, k_ZFactorCoordinate - 2),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 3] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, -2.5f, k_ZFactorCoordinate - 2),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 4] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, -1.5f, -k_ZFactorCoordinate + 2),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 5] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, -1.5f, -k_ZFactorCoordinate + 1),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 6] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, -2.5f, -k_ZFactorCoordinate + 1),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 7] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, .5f, k_ZFactorCoordinate - 1),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 8] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, .5f, k_ZFactorCoordinate - 2),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 9] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, .5f, 0),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 10] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, 0, 0),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 11] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, 0, k_ZFactorCoordinate - 2),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 12] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, 2.5f, -k_ZFactorCoordinate + 1),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 13] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, 2.5f, k_ZFactorCoordinate - 1),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 14] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, 1.5f, k_ZFactorCoordinate - 1),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 15] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, 1.5f, -k_ZFactorCoordinate + 2),
                r_Color);
            m_Vertices[k_PLetterStartIndex + 16] = new VertexPositionColor(
                new Vector3(3 + k_LetterSpace, 2.5f, -k_ZFactorCoordinate + 2),
                r_Color);          
        }

        /// <summary>
        /// Initialize the "�" letter coordinates
        /// </summary>
        private void    initPLetterIndices()
        {
            m_Indices[k_PLetterIndicesStartInd] = k_PLetterStartIndex;
            m_Indices[k_PLetterIndicesStartInd + 1] = k_PLetterStartIndex + 1;
            m_Indices[k_PLetterIndicesStartInd + 2] = k_PLetterStartIndex + 5;
            m_Indices[k_PLetterIndicesStartInd + 3] = k_PLetterStartIndex;
            m_Indices[k_PLetterIndicesStartInd + 4] = k_PLetterStartIndex + 5;
            m_Indices[k_PLetterIndicesStartInd + 5] = k_PLetterStartIndex + 6;

            m_Indices[k_PLetterIndicesStartInd + 6] = k_PLetterStartIndex + 1;
            m_Indices[k_PLetterIndicesStartInd + 7] = k_PLetterStartIndex + 7;
            m_Indices[k_PLetterIndicesStartInd + 8] = k_PLetterStartIndex + 8;
            m_Indices[k_PLetterIndicesStartInd + 9] = k_PLetterStartIndex + 1;
            m_Indices[k_PLetterIndicesStartInd + 10] = k_PLetterStartIndex + 8;
            m_Indices[k_PLetterIndicesStartInd + 11] = k_PLetterStartIndex + 2;

            m_Indices[k_PLetterIndicesStartInd + 12] = k_PLetterStartIndex + 11;
            m_Indices[k_PLetterIndicesStartInd + 13] = k_PLetterStartIndex + 8;
            m_Indices[k_PLetterIndicesStartInd + 14] = k_PLetterStartIndex + 9;
            m_Indices[k_PLetterIndicesStartInd + 15] = k_PLetterStartIndex + 11;
            m_Indices[k_PLetterIndicesStartInd + 16] = k_PLetterStartIndex + 9;
            m_Indices[k_PLetterIndicesStartInd + 17] = k_PLetterStartIndex + 10;

            m_Indices[k_PLetterIndicesStartInd + 18] = k_PLetterStartIndex + 4;
            m_Indices[k_PLetterIndicesStartInd + 19] = k_PLetterStartIndex + 16;
            m_Indices[k_PLetterIndicesStartInd + 20] = k_PLetterStartIndex + 12;
            m_Indices[k_PLetterIndicesStartInd + 21] = k_PLetterStartIndex + 4;
            m_Indices[k_PLetterIndicesStartInd + 22] = k_PLetterStartIndex + 12;
            m_Indices[k_PLetterIndicesStartInd + 23] = k_PLetterStartIndex + 5;

            m_Indices[k_PLetterIndicesStartInd + 24] = k_PLetterStartIndex + 14;
            m_Indices[k_PLetterIndicesStartInd + 25] = k_PLetterStartIndex + 13;
            m_Indices[k_PLetterIndicesStartInd + 26] = k_PLetterStartIndex + 16;
            m_Indices[k_PLetterIndicesStartInd + 27] = k_PLetterStartIndex + 14;
            m_Indices[k_PLetterIndicesStartInd + 28] = k_PLetterStartIndex + 16;
            m_Indices[k_PLetterIndicesStartInd + 29] = k_PLetterStartIndex + 15;
            /*          

          Add(new TriangleHolder<VertexPositionColor>(
              Game,
              VertexPositionColor.VertexElements,
              2,
              new VertexPositionColor(m_PLetterCoordinates[4], Color.Black),
              new VertexPositionColor(m_PLetterCoordinates[16], Color.Black),
              new VertexPositionColor(m_PLetterCoordinates[12], Color.Black),
              new VertexPositionColor(m_PLetterCoordinates[5], Color.Black)));

          Add(new TriangleHolder<VertexPositionColor>(
              Game,
              VertexPositionColor.VertexElements,
              2,
              new VertexPositionColor(m_PLetterCoordinates[14], Color.Black),
              new VertexPositionColor(m_PLetterCoordinates[13], Color.Black),
              new VertexPositionColor(m_PLetterCoordinates[16], Color.Black),
              new VertexPositionColor(m_PLetterCoordinates[15], Color.Black)));*/
        }

    }
}