namespace CM
{
    using System.Linq;
    using System.Net;
    using System.Windows.Forms;
    using static System.Windows.Forms.Control;

    public class StartSession
    {
        // Reset all textboxes except the timer interval and the percentage
        public void ClearTextBoxes(ControlCollection controls)
        {
            foreach (TextBox tb in controls.OfType<TextBox>())
            {
                if (tb.Name != "TextBoxTimeInterval" && tb.Name != "TextBoxWarnPercentage")
                {
                    tb.Text = "0";
                }
            }

            foreach (Control c in controls)
            {
                this.ClearTextBoxes(c.Controls);
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        
    }
}
