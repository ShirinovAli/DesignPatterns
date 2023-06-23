using System.Text;
using TemplatePattern.WebApp.Models;

namespace TemplatePattern.WebApp.UserCards
{
    public abstract class UserCardTemplate
    {
        protected AppUser AppUser { get; set; }

        public void SetUser(AppUser user)
        {
            AppUser = user;
        }
        public string Build()
        {
            if (AppUser == null) throw new ArgumentNullException(nameof(AppUser));

            StringBuilder stringBuilder = new();
            stringBuilder.Append("<div class='card'>");
            stringBuilder.Append("</div>");


            // ToDo: continue code ...
            return "";
        }
        protected abstract string SetFooter();
        protected abstract string SetFeature();
    }
}
