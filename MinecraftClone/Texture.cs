using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace MinecraftClone;
internal class Texture
{
    public int TextureId;
    private static string _path = "../../../Textures/";
    TextureTarget target2D = TextureTarget.Texture2D;
    ImageResult ImageTex;

    public Texture(string file)
    {
        //----------------------------------------- ID de Ativacao        
        // Gera identificador nomeado de textura
        TextureId = GL.GenTexture();
        // Seleciona a unidade de textura ativa
        GL.ActiveTexture(TextureUnit.Texture0);

        //-----------------------------------------  Binding com alvo
        // Vincula uma textura nomeada a um alvo de texturização
        GL.BindTexture(TextureTarget.Texture2D, TextureId);

        //------------------------------------------ Parametros;

        var wrapRepeat = (int)TextureWrapMode.Repeat;

        GL.TexParameter(target2D, TextureParameterName.TextureWrapS, wrapRepeat);
        GL.TexParameter(target2D, TextureParameterName.TextureWrapT, wrapRepeat);

        var minNear = (int)TextureMinFilter.Nearest;
        GL.TexParameter(target2D, TextureParameterName.TextureMinFilter, minNear);

        var magNear = (int)TextureMagFilter.Nearest;
        GL.TexParameter(target2D, TextureParameterName.TextureMagFilter, magNear);


        //------------------------------------------  Textura;
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageTex = LoadTexture(file);
        CreateTexImage2D(ImageTex);

    }


    private static ImageResult LoadTexture(string fileName)
    {
        string filePath = _path + fileName;


        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult imageTex;

        using (var stream = File.OpenRead(filePath))
        {
            imageTex = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        }

        return imageTex;
    }

    public static void CreateTexImage2D(ImageResult img)
    {

        GL.TexImage2D
        (
              target: TextureTarget.Texture2D,
              level: 0,
              internalformat: PixelInternalFormat.Rgba,
              width: img.Width,
              height: img.Height,
              border: 0,
              format: PixelFormat.Rgba,
              type: PixelType.UnsignedByte,
              pixels: img.Data

        );
    }
}


