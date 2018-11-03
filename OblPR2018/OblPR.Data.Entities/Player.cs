using System;

namespace OblPR.Data.Entities
{
    [Serializable]
    public class Player
    {

        public Guid Id { get; private set; }

        private bool _active;
        public Player(string nickname, string image)
        {
            _active = true;
            Id = Guid.NewGuid();
            Nick = nickname;
            Image = image;
        }

        public string Nick { get; }
        public string Image { get; } = "";

        public void Deactivate()
        {
            this._active = false;
        }

        public bool IsActive()
        {
            return _active;
        }

        public override string ToString()
        {
            return Nick;
        }
    }
}
