using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModbusTools.Domain
{
    [DataContract]
    public class Block
    {
        public Block(string previousHash, int difficulty, string miner, int rewards) 
        {
            this.PreviousHash = previousHash;
            this.Difficulty = difficulty;
            this.Miner = miner;
            this.Rewards = rewards;
        }

        [DataMember]
        public int Index { get; set; }

        /// <summary>
        /// 挖掘礦工
        /// </summary>
        [DataMember]
        public string Miner { get; set; }

        /// <summary>
        /// 獎勵
        /// </summary>
        [DataMember]
        public int Rewards { get; set; }

        /// <summary>
        /// 該區塊產生時的時間戳
        /// </summary>
        [DataMember]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 交易紀錄
        /// </summary>
        [DataMember]
        public IEnumerable<Transaction> Transactions { get; set; }

        /// <summary>
        /// 當前難度
        /// </summary>
        [DataMember]
        public int Difficulty { get; set; }

        /// <summary>
        /// 礦工找到能夠解開鎖的鑰匙
        /// </summary>
        [DataMember]
        public int Nonce { get; set; }

        /// <summary>
        /// 這個區塊的哈希值
        /// </summary>
        [DataMember]
        public string Hash { get; set; }

        /// <summary>
        /// 前個區塊的哈希值
        /// </summary>
        [DataMember]
        public string PreviousHash { get; set; }
    }
}