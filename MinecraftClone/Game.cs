using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using Vector2 = OpenTK.Mathematics.Vector2;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace MinecraftClone;
internal class Game : GameWindow
{
    // Constantes
    private static int _width;
    private static int _height;

    // Vertices de um quadrado
    List<Vector3> vertices = new List<Vector3>()
    {
    // front face
        new Vector3(-0.5f, 0.5f, 0.5f), // topleft vert
        new Vector3(0.5f, 0.5f, 0.5f), // topright vert
        new Vector3(0.5f, -0.5f, 0.5f), // bottomright vert
        new Vector3(-0.5f, -0.5f, 0.5f), // bottomleft vert
        // right face
        new Vector3(0.5f, 0.5f, 0.5f), // topleft vert
        new Vector3(0.5f, 0.5f, -0.5f), // topright vert
        new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
        new Vector3(0.5f, -0.5f, 0.5f), // bottomleft vert
        // back face
        new Vector3(0.5f, 0.5f, -0.5f), // topleft vert
        new Vector3(-0.5f, 0.5f, -0.5f), // topright vert
        new Vector3(-0.5f, -0.5f, -0.5f), // bottomright vert
        new Vector3(0.5f, -0.5f, -0.5f), // bottomleft vert
        // left face
        new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
        new Vector3(-0.5f, 0.5f, 0.5f), // topright vert
        new Vector3(-0.5f, -0.5f, 0.5f), // bottomright vert
        new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert
        // top face
        new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
        new Vector3(0.5f, 0.5f, -0.5f), // topright vert
        new Vector3(0.5f, 0.5f, 0.5f), // bottomright vert
        new Vector3(-0.5f, 0.5f, 0.5f), // bottomleft vert
        // bottom face
        new Vector3(-0.5f, -0.5f, 0.5f), // topleft vert
        new Vector3(0.5f, -0.5f, 0.5f), // topright vert
        new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
        new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert

    };


    List<Vector2> coords = new List<Vector2>()
    {
        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(0f, 0f),

        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(0f, 0f),

        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(0f, 0f),

        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(0f, 0f),

        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(0f, 0f),

        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(0f, 0f),
    };

    uint[] indices =
    {
        // first face
        // top triangle
        0, 1, 2,
        // bottom triangle
        2, 3, 0,

        4, 5, 6,
        6, 7, 4,

        8, 9, 10,
        10, 11, 8,

        12, 13, 14,
        14, 15, 12,

        16, 17, 18,
        18, 19, 16,

        20, 21, 22,
        22, 23, 20
    };

    List<Vector4> colors = new List<Vector4>()
    {
        new Vector4(1.0f, 0.0f, 0.7f, 1.0f),
        new Vector4(0.0f, 0.7f, 1.0f, 1.0f),
        new Vector4(0.7f, 0.0f, 1.0f, 1.0f),
        new Vector4(0.0f, 1.0f, 0.7f, 1.0f)

    };


    // Vars Render Pipeline
    int vertexArrayObject;
    int vertexBufferObject;
    int elementBufferObject;
    Shader shader;
    Texture texturizer;
    Camera camera;

    // transformation variables

    float yRot = 0f;


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
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes,
                        vertices.ToArray(), BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(vertexArrayObject, 0);



        //-----------------------------------------  Binding com alvo
        // Vincula uma textura nomeada a um alvo de texturização

        var textVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, textVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, coords.Count * Vector2.SizeInBytes,
            coords.ToArray(), BufferUsageHint.StaticCopy);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(vertexArrayObject, 1);



        // ElementBufferObject 
        elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint),
                        indices, BufferUsageHint.StaticDraw);

        GL.Enable(EnableCap.DepthTest);

        camera = new(_width, _height, Vector3.Zero);
        CursorState = CursorState.Grabbed;

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

        GL.ClearColor(Color4.LightSkyBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


        // Desenhar o Quadrado
        shader.Use();
        GL.BindVertexArray(vertexArrayObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BindTexture(TextureTarget.Texture2D, texturizer.TextureId);


        // Transformacao de Matrizes (4 x 4, rotacao, tranformacao escala, projecao) 
        //var radianos = MathHelper.DegreesToRadians(60.0f);

        Matrix4 model = Matrix4.Identity;
        Matrix4 view = camera.GetViewMatrix();

        Matrix4 proj = camera.GetProjectionMatrix();

        model = Matrix4.CreateRotationY(yRot);
        yRot += 0.001f;

        Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -3f);
        model *= translation;

        var modelLocation = GL.GetUniformLocation(shader.ProgramID, "model");
        var viewLocation = GL.GetUniformLocation(shader.ProgramID, "view");
        var projLocation = GL.GetUniformLocation(shader.ProgramID, "proj");

        GL.UniformMatrix4(modelLocation, true, ref model);
        GL.UniformMatrix4(viewLocation, true, ref view);
        GL.UniformMatrix4(projLocation, true, ref proj);


        GL.DrawElements(PrimitiveType.Triangles, indices.Length * 6, DrawElementsType.UnsignedInt, 0);


        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindTexture(TextureTarget.Texture2D, 0);
        GL.BindVertexArray(0);

        SwapBuffers();

    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        MouseState mouse = MouseState;
        KeyboardState input = KeyboardState;

        camera.Update(input, mouse, args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();
    }
}
