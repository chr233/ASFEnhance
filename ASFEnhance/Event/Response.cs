namespace Chrxw.ASFEnhance.Event
{
    internal class Response
    {
        //秋促徽章信息
        internal class SummerBadgeResponse
        {
            //提名一项奖项
            public bool VoteOne { get; }
            //提名全部奖项
            public bool VoteAll { get; }
            //玩一款提名游戏
            public bool PlayOne { get; }
            //评测一款提名游戏
            public bool ReviewOne { get; }
            public SummerBadgeResponse(bool VoteOne = false, bool VoteAll = false, bool PlayOne = false, bool ReviewOne = false)
            {
                this.VoteOne = VoteOne;
                this.VoteAll = VoteAll;
                this.PlayOne = PlayOne;
                this.ReviewOne = ReviewOne;
            }
        }
    }
}
