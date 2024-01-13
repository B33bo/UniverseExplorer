using UnityEngine;

namespace Universe
{
    public class GridDisplay : MonoBehaviour
    {
        private SpriteRenderer sp;

        public bool Enabled
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        private void Awake()
        {
            Enabled = false;
            sp = GetComponent<SpriteRenderer>();
        }

        public void EnableDevCommand()
        {
            Btools.DevConsole.DevCommands.Register("grid", "enable/disable the grid background", x =>
            {
                if (x.Length > 1)
                    Enabled = bool.Parse(x[1]);
                else
                    Enabled = !Enabled;
                return Enabled.ToString();
            });

            Btools.DevConsole.DevCommands.RegisterVar(new Btools.DevConsole.DevConsoleVariable("grid_enabled", "is the grid enabled", typeof(bool),
                () => Enabled,
                x => Enabled = bool.Parse(x)));
        }

        private void Update()
        {
            transform.position = GetPos();
        }

        private Vector2 GetPos()
        {
            Vector2 normalPos = transform.parent.position;
            normalPos.x = Mathf.Round(normalPos.x);
            normalPos.y = Mathf.Round(normalPos.y);
            return normalPos;
        }
    }
}
