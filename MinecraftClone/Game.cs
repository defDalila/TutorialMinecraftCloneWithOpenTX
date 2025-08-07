using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using System.Reflection.Emit;

namespace MinecraftClone;
internal class Game : GameWindow
{
    // Constantes
    private static int _width;
    private static int _height;

    // Vertices de um quadrado
    float[] vertices =
    {
        -0.5f, 0.5f, 0.0f,  // vertice superior Esq --- (0)
        0.5f, 0.5f, 0.0f,   // vertice superior Dir --- (1)
        0.5f, -0.5f, 0.0f,   // vertice inferior Dir --- (2)
        -0.5f, -0.5f, 0.0f // vertice infefior Esq --- (3)
    };

    float[] coords =
    {
        0.0f, 1.0f,
        1.0f, 1.0f,
        1.0f, 0.0f,
        0.0f, 0.0f
    };

    // indices dos vertices para desenhar 2 triangulos e formar o retagulo
    uint[] indices =
    {
        0, 1, 2, // Triangulo Superior
        2, 3, 0
    };

    float[] colors =
    {
        1.0f, 0.0f, 0.7f,
        0.0f, 0.7f, 1.0f,
        0.7f, 0.0f, 1.0f,
        0.0f, 1.0f, 0.7f
    };

    // Vars Render Pipeline
    int vertexArrayObject;
    int vertexBufferObject;
    int elementBufferObject;
    Shader shader;
    Texture texturizer;


    public Game(int width, int height) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { ClientSize = (width, height) })
    {
        // Centraliza a Janela no monitor
        this.CenterWindow(new Vector2i(width, height));
        _width = width;
        _height = height;
        Title = "Clone Minecraft com OpenTK/OpenGL";

        shader = new Shader("d.vert", "d.frag");
        texturizer = new Texture("dirtyBlock.png");
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);

        _width = e.Width;
        _height = e.Height;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        vertexArrayObject = GL.GenVertexArray();
        vertexBufferObject = GL.GenBuffer();

        // VertexBufferObject


        GL.BindVertexArray(vertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                      vertices, BufferUsageHint.StaticDraw);       
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(vertexArrayObject, 0);



        //-----------------------------------------  Binding com alvo
        // Vincula uma textura nomeada a um alvo de texturização

        var textVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, textVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, coords.Length * sizeof(float),
            coords, BufferUsageHint.StaticCopy);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(vertexArrayObject, 1);



        // ElementBufferObject 
        elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint),
                      indices, BufferUsageHint.StaticDraw);

    }

    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteBuffer(vertexBufferObject);
        GL.DeleteVertexArray(vertexArrayObject);
        GL.DeleteBuffer(elementBufferObject);
        shader.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(Color4.LightBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit);


        // Desenhar o Quadrado
        shader.Use();
        GL.BindVertexArray(vertexArrayObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BindTexture(TextureTarget.Texture2D, texturizer.TextureId);
        

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindTexture(TextureTarget.Texture2D, 0);
        GL.BindVertexArray(0);

        SwapBuffers();

    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();
    }
}
