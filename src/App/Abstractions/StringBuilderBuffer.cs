using System.Text;

namespace Debianet.Abstractions;

internal static class StringBuilderBuffer
{
    private static readonly StringBuilder _buffer = new StringBuilder(4096);

    public static StringBuilder GetBuffer()
    {
        return _buffer;
    }

    public static string GetStringAndClear(int lines)
    {
        if (lines < 0)
        {
            string result = _buffer.ToString();
            _buffer.Clear();
            return result;
        }

        for (int i=0; i<_buffer.Length; i++)
        {
            if (_buffer[i] == '\n')
            {
                lines--;
                if (lines == 0)
                {
                    string result = _buffer.ToString(0, i);
                    _buffer.Clear();
                    return result;
                }
            }
        }

        string remaining = _buffer.ToString();
        _buffer.Clear();
        return remaining;
    }
}
