using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace AspNetCoreIdentityApp.web.TagHelpers
{
    public class UserPictureThumbnailTagHelper:TagHelper
    {
        public string? PictureUrl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            if (string.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/userpictures/default.png");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/userpictures/{PictureUrl}");
            }

        }
    }
}
