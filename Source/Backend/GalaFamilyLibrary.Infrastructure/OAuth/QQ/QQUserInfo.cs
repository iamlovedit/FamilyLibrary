namespace GalaFamilyLibrary.Infrastructure.OAuth.QQ
{
    public class QQUserInfo
    {
        // 返回码
        public int ret { get; set; }

        // 如果ret<0，会有相应的错误信息提示
        // 返回数据全部用UTF-8编码
        public string msg { get; set; }

        // 判断是否有数据丢失
        // 0或者不返回：没有数据丢失，可以缓存
        // 1：有部分数据丢失或错误，不要缓存
        public int is_lost { get; set; }

        // 用户在QQ空间的昵称
        public string nickname { get; set; }

        // 大小为30x30像素的QQ空间头像URL
        public string figureurl { get; set; }

        // 大小为50x50像素的QQ空间头像URL
        public string figureurl_1 { get; set; }

        // 大小为100x100像素的QQ空间头像URL
        public string figureurl_2 { get; set; }

        // 大小为40x40像素的QQ头像URL
        public string figureurl_qq_1 { get; set; }

        // 大小为100x100像素的QQ头像URL
        // 需要注意，不是所有的用户都拥有QQ的100x100的头像，但40x40像素则是一定会有
        public string figureurl_qq_2 { get; set; }

        // 性别。如果获取不到则默认返回"男"
        public string gender { get; set; }

        // 性别类型。默认返回2
        public int gender_type { get; set; }

        // 省
        public string province { get; set; }

        // 市
        public string city { get; set; }

        // 年
        public int year { get; set; }

        // 星座
        public string constellation { get; set; }

        // 标识用户是否为黄钻用户
        public int is_yellow_vip { get; set; }

        // 黄钻等级
        public int yellow_vip_level { get; set; }

        // 是否为年费黄钻用户
        public int is_yellow_year_vip { get; set; }
    }
}