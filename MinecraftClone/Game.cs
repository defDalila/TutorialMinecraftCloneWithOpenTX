using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinecraftClone;
internal class Game : GameWindow
{
    // Constantes
    private static int _width;
    private static int _height;

    // Vertices de um retângulo
    float[] vertices =
    {
        -0.5f, 0.5f, 0.0f,  // vertice superior Esq --- (0)
        0.5f, 0.5f, 0.0f,   // vertice superior Dir --- (1)
        0.5f, -0.5f, 0.0f,   // vertice inferior Dir --- (2)
        -0.5f, -0.5f, 0.0f // vertice infefior Esq --- (3)
    };

    // indices dos vertices para desenhar 2 triangulos e formar o retagulo
    uint[] indices =
    {
        0, 1, 2, // Triangulo Superior
        2, 3, 0
    };

    // Vars Render Pipeline
    int VertexArrayObject;
    int VertexBufferObject;
    int ElementBufferObject;
    Shader ShaderProgram;


    public Game(int width, int height) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { ClientSize = (width, height) })
    {
        // Centraliza a Janela no monitor
        this.CenterWindow(new Vector2i(width, height));
        _width = width;
        _height = height;
        Title = "Clone Minecraft com OpenTK/OpenGL";

        ShaderProgram = new Shader("default.vert", "default.frag");
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

        VertexArrayObject = GL.GenVertexArray();
        VertexBufferObject = GL.GenBuffer();

        // VertexBufferObject
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                      vertices, BufferUsageHint.StaticDraw);

        // VertexArrayObject
        GL.BindVertexArray(VertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(VertexArrayObject, 0);

        // unbinding VertexBufferObject e VertexArrayObject
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);

        // ElementBufferObject 
        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint),
                      indices, BufferUsageHint.StaticDraw);

        // unbinding ElementBufferObject
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteBuffer(VertexBufferObject);
        GL.DeleteVertexArray(VertexArrayObject);
        GL.DeleteBuffer(ElementBufferObject);
        ShaderProgram.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(Color4.LightBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Desenhar o Quadrado
        ShaderProgram.Use();
        GL.BindVertexArray(VertexArrayObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();

    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();
    }
}
