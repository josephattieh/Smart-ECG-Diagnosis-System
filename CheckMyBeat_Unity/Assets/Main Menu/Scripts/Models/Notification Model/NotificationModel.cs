namespace Models
{
    public class NotificationModel
    {
        public NotificationSprite imageType;
        public string title;
        public string description;
        public NotificationModel(NotificationSprite type, string title, string desc)
        {
            this.imageType = type;
            this.title = title;
            this.description = desc;
        }
    }
}