namespace Presentation.WinForms.ClientApp
{
    using System.Windows.Forms;

    public static class CrossThreadingHelper
    {
        public static string GetTextThreadSafely<TControl>(this TControl source)
            where TControl : TextBoxBase 
        {
            if (source.InvokeRequired)
            {
                string text = string.Empty;
                source.Invoke((MethodInvoker) (() => text = source.GetTextThreadSafely()));
                return text;
            }
            else
            {
                return source.Text;
            }
        }

        public static string GetTextThreadSafely(this ComboBox source)
        {
            if (source.InvokeRequired)
            {
                string text = string.Empty;
                source.Invoke((MethodInvoker)(() => text = source.GetTextThreadSafely()));
                return text;
            }
            else
            {
                return source.Text;
            }
        }
    }
}