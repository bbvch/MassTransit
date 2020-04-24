namespace MassTransit.Tests
{
    using System;
    using System.Threading.Tasks;
    using GreenPipes;
    using NUnit.Framework;
    using TestFramework.Messages;
    using Util;


    [TestFixture]
    public class An_improperly_configured_endpoint
    {
        [Test]
        public void Should_throw_for_message_retry()
        {
            Assert.That(() => Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("Hello", e =>
                {
                    e.UseMessageRetry(r =>
                    {
                    });

                    e.Consumer<Consumer>();
                });
            }), Throws.TypeOf<ConfigurationException>());
        }

        [Test]
        public void Should_throw_for_retry()
        {
            Assert.That(() => Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("Hello", e =>
                {
                    e.UseRetry(r =>
                    {
                    });

                    e.Consumer<Consumer>();
                });
            }), Throws.TypeOf<ConfigurationException>());
        }

        [Test]
        public void Should_throw_for_consumer_retry()
        {
            Assert.That(() => Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("Hello", e =>
                {
                    e.Consumer<Consumer>(cc =>
                    {
                        cc.UseRetry(r =>
                        {
                        });
                    });
                });
            }), Throws.TypeOf<ConfigurationException>());
        }

        [Test]
        public void Should_throw_for_consumer_retry_extra()
        {
            try
            {
                Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    cfg.ReceiveEndpoint("Hello", e =>
                    {
                        e.Consumer<Consumer>(cc =>
                        {
                            cc.UseRetry(r =>
                            {
                            });
                        });
                    });
                });
            }
            catch (Exception exception)
            {
                Assert.That(exception, Is.TypeOf<ConfigurationException>());
            }
        }

        [Test]
        public void Should_throw_for_consumer_message_retry()
        {
            Assert.That(() => Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("Hello", e =>
                {
                    e.Consumer<Consumer>(cc =>
                    {
                        cc.Message<PingMessage>(m => m.UseRetry(r =>
                        {
                        }));
                    });
                });
            }), Throws.TypeOf<ConfigurationException>());
        }

        [Test]
        public void Should_throw_for_consumer_invalid_message()
        {
            Assert.That(() => Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("Hello", e =>
                {
                    e.Consumer<Consumer>(cc =>
                    {
                        cc.Message<PongMessage>(m =>
                        {
                        });
                    });
                });
            }), Throws.TypeOf<ArgumentException>());
        }


        class Consumer :
            IConsumer<PingMessage>
        {
            public Task Consume(ConsumeContext<PingMessage> context)
            {
                return TaskUtil.Completed;
            }
        }
    }
}
