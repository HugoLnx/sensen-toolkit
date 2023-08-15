using System;
using UnityEngine;

namespace SensenToolkit
{
    [System.Serializable]
    public class SerializableTexture2D
    {
        [field: SerializeField] public string Base64 { get; set; }
        private Texture2D _texture;
        private Sprite _sprite;
        public Texture2D Texture
        {
            get {
                ResolveAttributes();
                return _texture;
            }
        }
        public Sprite Sprite
        {
            get {
                ResolveAttributes();
                return _sprite;
            }
        }
        public bool IsValid =>
            !string.IsNullOrEmpty(Base64)
            && Texture != null
            && Sprite != null;

        public SerializableTexture2D(Texture2D texture)
        {
            _texture = texture;
            _sprite = SpriteFromTexture(texture);
            Base64 = Texture2DToBase64(texture);
        }

        public SerializableTexture2D(string base64)
        {
            _texture = Base64ToTexture2D(base64);
            _sprite = SpriteFromTexture(Texture);
            Base64 = base64;
        }

        private void ResolveAttributes()
        {
            _texture = _texture != null ? _texture : Base64ToTexture2D(Base64);
            _sprite = _sprite != null ? _sprite : SpriteFromTexture(_texture);
        }

        public static string Texture2DToBase64(Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            return Convert.ToBase64String(bytes);
        }

        public static Texture2D Base64ToTexture2D(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Texture2D texture = new(1, 1);
            texture.LoadImage(bytes);
            return texture;
        }

        private static Sprite SpriteFromTexture(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
    }
}
