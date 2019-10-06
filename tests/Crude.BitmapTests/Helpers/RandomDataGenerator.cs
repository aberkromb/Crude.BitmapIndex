using System;
using System.Collections.Generic;
using Bogus;

namespace BitmapTests.Helpers
{
    public class Generator
    {
        public static List<MessageData> CreateRandomData(int count) => ConfigureFaker().Generate(count);

        public static IEnumerable<MessageData> CreateRandomDataLazy(int count) => ConfigureFaker().GenerateLazy(count);

        private static Faker<MessageData> ConfigureFaker() =>
            new Faker<MessageData>()
                .RuleFor(o => o.Id, f => f.IndexFaker)
                .RuleFor(o => o.Server, f => f.Name.FirstName())
                .RuleFor(o => o.Application, f => f.Name.LastName())
                .RuleFor(o => o.Exchange, f => f.Name.JobTitle())
                .RuleFor(o => o.MessageDate, f => f.Date.BetweenOffset(DateTime.Now.AddMonths(-1), DateTime.Now))
                .RuleFor(o => o.MessageType, f => f.Lorem.Word())
                .RuleFor(o => o.MessageRoutingKey, f => f.Lorem.Word())
                .RuleFor(o => o.Message, f => f.Lorem.Sentence(300))
                .RuleFor(o => o.Exception, f => f.Lorem.Word())
                .RuleFor(o => o.Ttl, f => f.Random.Int())
                .RuleFor(o => o.Persistent, f => f.Random.Bool())
                .RuleFor(o => o.AdditionalHeaders, f => f.Lorem.Sentence(4));
    }

    public class MessageData
    {
        public virtual int Id { get; set; }
        public virtual string Server { get; set; }
        public virtual string Application { get; set; }
        public virtual string Exchange { get; set; }
        public virtual DateTimeOffset MessageDate { get; set; }
        public virtual string MessageType { get; set; }
        public virtual string MessageRoutingKey { get; set; }
        public virtual string Message { get; set; }
        public virtual string Exception { get; set; }
        public virtual int? Ttl { get; set; }
        public virtual bool Persistent { get; set; }
        public virtual string AdditionalHeaders { get; set; }
    }
}