using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;

namespace MinecraftClone;
internal class Camera
{
    // Campos
    private const float Speed = 8f;
    private const float Sensitivity = 5f;
    private const float ProjectionAngle = 45.0f;


    private float _fovy = MathHelper.DegreesToRadians(ProjectionAngle);
    private float _screenWidth;
    private float _screenHeight;
    private float _screenAspectRatio;

    public Vector3 Position;

    // variaveis normalizadas - dão a direção
    Vector3 Up = Vector3.UnitY;
    Vector3 Right = Vector3.UnitX;
    Vector3 Forward = -Vector3.UnitZ;

    private float _pitch;
    private float _yaw = -90.0f;
    private bool _firstMove = true;
    private Vector2 _lastPosition;

    public Camera(float width, float height, Vector3 position)
    {
        _screenAspectRatio = width / height;
        _screenWidth = width;
        _screenHeight = height;
        Position = position;
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + Forward, Up);
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(_fovy, _screenAspectRatio, 0.1f, 100.0f);
    }


    private void UpdateVectors()
    {
        if (_pitch > 89.0f)
        {
            _pitch = 89.0f;
        }
        if (_pitch < -89.0f)
        {
            _pitch = -89.0f;
        }


        var rotationYaw = Matrix3.CreateFromAxisAngle(Vector3.UnitY, _yaw);
        var rotationPitch = Matrix3.CreateFromAxisAngle(Vector3.UnitX, _pitch);
        var matrixRotacao = Matrix3.Mult(rotationPitch, rotationYaw);
        rotationYaw.Transpose();
        rotationPitch.Transpose();

        var rotation = Matrix3.Mult(rotationYaw, rotationPitch);

        Forward = rotation * new Vector3(Vector3.UnitZ);
        Forward = Vector3.Normalize(Forward);

        Right = Vector3.Normalize(Vector3.Cross(Forward, Vector3.UnitY));
        Up = Vector3.Normalize(Vector3.Cross(Right, Forward));

    }

    public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs args)
    {

        if (input.IsKeyDown(Keys.W))
        {
            Position += Forward * Speed * (float)args.Time;
        }
        if (input.IsKeyDown(Keys.A))
        {
            Position -= Right * Speed * (float)args.Time;
        }
        if (input.IsKeyDown(Keys.S))
        {
            Position -= Forward * Speed * (float)args.Time;
        }
        if (input.IsKeyDown(Keys.D))
        {
            Position += Right * Speed * (float)args.Time;
        }
        if (input.IsKeyDown(Keys.Space))
        {
            Position.Y += Speed * (float)args.Time;
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            Position.Y -= Speed * (float)args.Time;
        }

        if (_firstMove)
        {
            _lastPosition = new Vector2(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            float deltaX = mouse.X - _lastPosition.X;
            float deltaY = mouse.Y - _lastPosition.Y;

            _lastPosition = new Vector2(mouse.X, mouse.Y);

            _yaw += deltaX * Sensitivity * (float)args.Time;
            _pitch -= deltaY * Sensitivity * (float)args.Time;

        }

        UpdateVectors();

    }

    public void Update(KeyboardState input, MouseState mouse, FrameEventArgs args)
    {
        InputController(input, mouse, args);
    }
}
