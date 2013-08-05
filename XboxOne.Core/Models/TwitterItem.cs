using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxOne.Core.Models
{
    public class TwitterItem
    {
        private string author;
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                if (author != value)
                {
                    author = value;
                }
            }
        }

        private string tweet;
        public string Tweet
        {
            get
            {
                return tweet;
            }
            set
            {
                if (tweet != value)
                {
                    tweet = value;
                }
            }
        }

        private string publishDate;
        public string PublishDate
        {
            get
            { return publishDate; }
            set
            {
                if (publishDate != value)
                {
                    publishDate = value;
                }
            }
        }

        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                }
            }
        }

        private string avatarUrl;
        public string AvatarUrl
        {
            get { return avatarUrl; }
            set
            {
                if (avatarUrl != value)
                {
                    avatarUrl = value;
                }
            }
        } 

    }
}
