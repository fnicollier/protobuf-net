using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace ProtoBuf.unittest.Issues
{
    [TestFixture]
    public class SO27563871
    {
        [Test]
        public void SerializeDeserialize()
        {
            var a = new A
            {
                Name = "Hello"
            };
            A clone;

            a.Add(new B { Id = 1 });
            a.Add(new B { Id = 2 });

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, a);
                stream.Seek(0, SeekOrigin.Begin);
                clone = Serializer.Deserialize<A>(stream);
            }
            Assert.IsNotNull(clone);
            Assert.IsNotEmpty(clone);
            Assert.IsNotNull(clone.Name);
        }

        [ProtoContract(ImplicitFields = ImplicitFields.AllFields), ProtoInclude(10, typeof(B)), ProtoInclude(100, typeof(List<B>))]
        public sealed class A : List<B>
        {
            public string Name { get; set; }
        }

        [ProtoContract(ImplicitFields = ImplicitFields.AllFields), ProtoInclude(1, typeof(C<B>))]
        public class B : C<B>
        {
            public int Id { get; set; }
        }

        public class C<T>
        {

        }
    }
}
