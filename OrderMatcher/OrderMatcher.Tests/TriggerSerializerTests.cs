﻿using System;
using Xunit;

namespace OrderMatcher.Tests
{
    public class TriggerSerializerTests
    {
        [Fact]
        public void Serialize_Doesnotthrowexception_Min()
        {
            var bytes = OrderTriggerSerializer.Serialize(new OrderTrigger { OrderId = ulong.MinValue, Timestamp = long.MinValue });
        }

        [Fact]
        public void Serialize_Doesnotthrowexception_Max()
        {
            var bytes = OrderTriggerSerializer.Serialize(new OrderTrigger { OrderId = ulong.MaxValue, Timestamp = long.MaxValue });
        }

        [Fact]
        public void Serialize_ThrowsExecption_IfNullPassed()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => OrderTriggerSerializer.Serialize(null));
            Assert.Equal("orderTrigger", ex.ParamName);
        }

        [Fact]
        public void Deserialize_ThrowsExecption_IfNullPassed()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => OrderTriggerSerializer.Deserialize(null));
            Assert.Equal("bytes", ex.ParamName);
        }

        [Fact]
        public void Deserialize_ThrowsExecption_IfMessageIsLessThan35Bytes()
        {
            var bytes = new byte[22];
            Exception ex = Assert.Throws<Exception>(() => OrderTriggerSerializer.Deserialize(bytes));
            Assert.Equal("Order Trigger Message must be of Size : 23", ex.Message);
        }

        [Fact]
        public void Deserialize_ThrowsExecption_IfMessageIsGreaterThan35Bytes()
        {
            var bytes = new byte[24];
            Exception ex = Assert.Throws<Exception>(() => OrderTriggerSerializer.Deserialize(bytes));
            Assert.Equal("Order Trigger Message must be of Size : 23", ex.Message);
        }

        [Fact]
        public void Deserialize_ThrowsExecption_IfMessageIsNothaveValidType()
        {
            var bytes = new byte[23];
            Exception ex = Assert.Throws<Exception>(() => OrderTriggerSerializer.Deserialize(bytes));
            Assert.Equal("Invalid Message", ex.Message);
        }

        [Fact]
        public void Deserialize_ThrowsExecption_IfVersionIsNotSet()
        {
            var bytes = new byte[23];
            bytes[4] = (byte)MessageType.OrderTrigger;
            Exception ex = Assert.Throws<Exception>(() => OrderTriggerSerializer.Deserialize(bytes));
            Assert.Equal("version mismatch", ex.Message);
        }

        [Fact]
        public void Deserialize_Doesnotthrowexception_Min()
        {
            var bytes = OrderTriggerSerializer.Serialize(new OrderTrigger { OrderId = ulong.MinValue, Timestamp = long.MinValue });
            var messageLength = BitConverter.ToInt32(bytes, 0);
            Assert.Equal(23, messageLength);
            var orderTrigger = OrderTriggerSerializer.Deserialize(bytes);
            Assert.Equal(ulong.MinValue, orderTrigger.OrderId);
            Assert.Equal(long.MinValue, orderTrigger.Timestamp);
        }

        [Fact]
        public void Deserialize_Doesnotthrowexception_Max()
        {
            var bytes = OrderTriggerSerializer.Serialize(new OrderTrigger { OrderId = ulong.MaxValue, Timestamp = long.MaxValue });
            var messageLength = BitConverter.ToInt32(bytes, 0);
            Assert.Equal(23, messageLength);
            var orderTrigger = OrderTriggerSerializer.Deserialize(bytes);
            Assert.Equal(ulong.MaxValue, orderTrigger.OrderId);
            Assert.Equal(long.MaxValue, orderTrigger.Timestamp);
        }

        [Fact]
        public void Deserialize_Doesnotthrowexception()
        {
            var bytes = OrderTriggerSerializer.Serialize(new OrderTrigger { OrderId = 12345678, Timestamp = 404 });
            var messageLength = BitConverter.ToInt32(bytes, 0);
            Assert.Equal(23, messageLength);
            var orderTrigger = OrderTriggerSerializer.Deserialize(bytes);
            Assert.Equal((ulong)12345678, orderTrigger.OrderId);
            Assert.Equal(404, orderTrigger.Timestamp);
        }
    }
}
