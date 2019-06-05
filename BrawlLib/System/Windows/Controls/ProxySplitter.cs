﻿namespace System.Windows.Forms
{
    public class ProxySplitter : Control
    {
        private bool _dragging;
        private int _lastX, _lastY;

        public ProxySplitter()
        {
            Size = new Drawing.Size(5, 5);
            Dock = DockStyle.Left;
        }

        public override DockStyle Dock
        {
            get => base.Dock;
            set
            {
                base.Dock = value;
                switch (value)
                {
                    case DockStyle.Left:
                    case DockStyle.Right:
                        Cursor = Cursors.VSplit;
                        break;

                    case DockStyle.Top:
                    case DockStyle.Bottom:
                        Cursor = Cursors.HSplit;
                        break;
                }
            }
        }

        public event SplitterEventHandler Dragged;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _dragging = true;

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _dragging = false;

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var x = Cursor.Position.X;
            var y = Cursor.Position.Y;
            var xDiff = x - _lastX;
            var yDiff = y - _lastY;
            _lastX = x;
            _lastY = y;

            if (_dragging) Dragged?.Invoke(this, new SplitterEventArgs(xDiff, yDiff, Left, Top));

            base.OnMouseMove(e);
        }
    }
}