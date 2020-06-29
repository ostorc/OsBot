using System;

namespace OsBot.Console.DAL
{
    public enum RemindStatus
    {
        New,
        Send,
        Failed
    }

    public class RemindNote
    {
        protected RemindNote()
        {
        }

        public RemindNote(ulong channelId, ulong authorId, ulong messageId, DateTime when)
            : this(0, channelId, authorId, messageId, when)
        {
        }

        public RemindNote(int remindNoteId, ulong channelId, ulong authorId, ulong messageId, DateTime when)
        {
            RemindNoteId = remindNoteId;
            ChannelId = channelId;
            AuthorId = authorId;
            MessageId = messageId;
            When = when;
        }

        public int RemindNoteId { get; set; }

        public ulong ChannelId { get; set; }
        public ulong AuthorId { get; set; }
        public ulong MessageId { get; set; }
        public DateTime When { get; set; }
        public RemindStatus Status { get; set; } = RemindStatus.New;
    }
}