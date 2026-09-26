namespace RSoft.MacroPad.BLL.Infrasturture.Model
{
    public enum MouseButton
    {
        [MouseValues(1,0)]
        [VirtualKeyMap(VirtualKey.LButton)]
        Left,

        [MouseValues(4, 0)]
        [VirtualKeyMap(VirtualKey.MButton)]
        Middle,

        [MouseValues(2, 0)]
        [VirtualKeyMap(VirtualKey.RButton)]
        Right,

        [MouseValues(0, 1)]
        [VirtualKeyMap(VirtualKey.Up)]
        ScrollUp,

        [MouseValues(0, 255)]
        [VirtualKeyMap(VirtualKey.Down)]
        ScrollDown
    }
}
