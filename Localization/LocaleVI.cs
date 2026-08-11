// <copyright file="LocaleVI.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleVI.cs
// Vietnamese vi-VN locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Vietnamese localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleVI : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Vietnamese locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleVI(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Vietnamese localization entries for this mod.</summary>
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            string title = Mod.kModName;

            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title = title + " (" + Mod.ModVersion + ")";
            }
            return new Dictionary<string, string>

            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), title },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Thao tác" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Giới thiệu" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Dọn tự động" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Tự quản lý" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Nâng cao" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Trạng thái" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Thông tin mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Liên kết" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Gỡ lỗi" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Bật Dọn Ma Thuật" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Tự động xóa thi thể cần được xe tang vận chuyển.\n" +
                    "Dọn Ma Thuật và tự quản lý loại trừ lẫn nhau; hãy chọn một trong hai.\n" +
                    "Tắt tất cả ô chọn để vô hiệu hóa mod mà không cần gỡ mod.\n" +
                    "Ghi chú kỹ thuật: phải có IsDead = true và WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Đặt lại nghĩa trang đầy" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Làm trống nghĩa trang đầy** để không bị chặn bởi biểu tượng ĐẦY.\n" +
                    "Dọn Ma Thuật loại bỏ phần lớn thi thể trước khi chôn — tùy chọn này vẫn làm trống mọi nghĩa trang **đã đầy**.\n" +
                    "<[ ] Mặc định TẮT>.\n" +
                    "Chỉ bật tùy chọn này nếu chế độ Dọn Ma Thuật cũng cần làm trống những nghĩa trang đã đầy.\n" +
                    "Sau khi đã làm trống, thường không cần để tùy chọn này bật nếu Dọn Ma Thuật vẫn được bật."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Quản lý tang lễ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Tự quản lý và tối ưu các hệ thống tang lễ thông thường của trò chơi.\n" +
                    "**Giá trị tỷ lệ:** tốc độ, đội xe, lưu trữ.\n" +
                    "Tùy chọn: **tăng cả số nhân công**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Xử lý lò hỏa táng" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Tốc độ xử lý của lò hỏa táng.**\n" +
                    "Giá trị cao hơn sẽ hỏa táng thi thể nhanh hơn và giải phóng chỗ chứa của cơ sở sớm hơn.\n" +
                    "**100%** = giá trị mặc định của trò chơi."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Tổng số xe tang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Số xe tang tối đa** cho mỗi cơ sở.\n" +
                    "**100%** = giá trị mặc định của trò chơi.\n" +
                    "**[Lưu ý]** Quá nhiều xe tang có thể ảnh hưởng giao thông tùy theo tỷ lệ tử vong."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Tốc độ xe tang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Tăng tốc độ lái tối đa được phép của xe tang**.\n" +
                    "**100%** = giá trị mặc định của trò chơi.\n" +
                    "<Giới hạn tốc độ đường vẫn được áp dụng>.\n" +
                    "\n" +
                    "Đồng thời điều chỉnh tăng tốc/phanh nhẹ nhàng để tốc độ tối đa mới không gây tăng tốc hoặc dừng quá đột ngột.\n" +
                    "Lưu ý: ngay cả khi tăng tốc độ tối đa của xe tang, tốc độ thực tế vẫn bị ảnh hưởng bởi:\n" +
                    "tốc độ tối đa của xe, giới hạn tốc độ đường, tốc độ an toàn của AI trong game (khúc cua, hư hỏng đường) và giao thông."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Độ trễ cảnh báo tử vong (phút)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "Đây là tổng thời gian xe tang có để đến một tòa nhà trước khi biểu tượng vấn đề **đang chờ xe tang** xuất hiện.\n" +
                    "**3 phút** gần với mặc định của trò chơi là khoảng 2,5 phút mô phỏng.\n" +
                    "Có thể tăng giá trị này để xe tang có thời gian hợp lý hơn hoàn thành chuyến đi trước khi biểu tượng tử vong xuất hiện.\n" +
                    "Lưu ý:\n" +
                    "- <Đề xuất: 10 phút>. Hãy thử cao hơn với thành phố tắc nghẽn nặng.\n" +
                    "- Xem báo cáo Trạng thái phía dưới để biết có bao nhiêu trường hợp quá hạn.\n" +
                    "- Các biểu tượng đang hiển thị sẽ không bị ẩn khi tăng giá trị này lần đầu; chúng vẫn còn cho đến khi xe tang xử lý hoặc tòa nhà bị phá.\n" +
                    "- Để các chuyến điều xe hiện tại hoàn tất tự nhiên hoặc dùng ô <Dọn Ma Thuật [x]> một lần để nhanh chóng bắt đầu lại với lịch thời gian mới."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Sức chứa nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Sức chứa của nghĩa trang** cho tòa nhà chính.\n" +
                    "Sức chứa lớn hơn giúp nghĩa trang đầy có thể nhận thi thể trở lại.\n" +
                    "Không điều thêm xe tang trừ khi thiếu chỗ đang làm cơ sở bị chặn.\n" +
                    "**100%** = giá trị mặc định của trò chơi."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Tự động đặt lại nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Làm trống nghĩa trang đầy** để không bị chặn bởi biểu tượng ĐẦY phía trên tòa nhà.\n" +
                    "Không còn cần xóa và xây lại nghĩa trang đầy.\n" +
                    "Tắt tùy chọn này để dùng **Tốc độ luân chuyển nghĩa trang** dần dần.\n" +
                    "<[ ✓ ] Mặc định BẬT>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Tốc độ luân chuyển nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Dần giải phóng các ngôi mộ đang được sử dụng.**\n" +
                    "Giá trị cao hơn giúp chỗ mộ có thể sử dụng lại nhanh hơn trò chơi gốc.\n" +
                    "Nếu nghĩa trang vẫn đầy quá thường xuyên ở mức 500%,\n" +
                    "hãy bật **[Tự động đặt lại nghĩa trang]** thay thế.\n" +
                    "**100%** = tốc độ mặc định của trò chơi để tái sử dụng mộ."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Điều chỉnh nhân công" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Công tắc tương thích:\n" +
                    "**Bật [✓]** để tăng số nhân công.\n" +
                    "**[o_o]** Để TẮT nếu muốn **ConfigXML** hoặc mod khác kiểm soát nhân công dịch vụ tang lễ."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Nhân công tối đa" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Tăng số nhân công tối đa** được phép.\n" +
                    "**100%** = giá trị mặc định của trò chơi."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Đặt lại thanh trượt" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Đặt các thanh trượt phần trăm về **100%** và độ trễ cảnh báo tử vong về **3 phút**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Cần xe tang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Đang chờ** = tất cả công dân đã chết vẫn ở bên ngoài và đang chờ được thu gom.\n" +
                    "**Quá hạn** = công dân đang chờ đã hết thời gian trễ thông báo được chọn.\n" +
                    " - Nếu có nhiều trường hợp quá hạn, hãy cân nhắc tăng Độ trễ cảnh báo tử vong."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Khối lượng" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Tổng theo tháng** từ số liệu của trò chơi.\n" +
                    "**Tối đa/tháng** = xử lý lò hỏa táng cộng luân chuyển nghĩa trang theo hiệu suất hiện tại.\n" +
                    "Đây là số thi thể tối đa mà tất cả cơ sở tang lễ đang hoạt động có thể xử lý mỗi tháng."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Tài sản" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Năng lực của tòa nhà đang hoạt động:** tổng xe tang, tòa nhà, nhân công tối đa.\n" +
                    "\n" +
                    "**Ghi chú:**\n" +
                    "▪ Xe tang: Đang hoạt động-không đỗ / (Tổng* xe tang)\n" +
                    "▪ *Tổng xe tang:\n" +
                    "== gồm xe tang đang bảo trì (ví dụ ngân sách dịch vụ thấp), \n" +
                    "== không gồm xe tang của tòa nhà bị vô hiệu hóa.\n" +
                    "▪ Quét trạng thái chỉ chạy khi trang Tùy chọn đang mở (hoặc khi dùng thanh trượt); không chạy mỗi khung hình trong thành phố nên hầu như không ảnh hưởng hiệu năng :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Chưa tải trạng thái." },
                { "MH_STATUS_NO_CITY_LOADED", "Chưa tải thành phố." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Không có thành phố... ¯\\_(ツ)_/¯ ...Không có số liệu" },

                { "MH_STATUS_LINE1_V2", "{0} đang chờ | {1} quá hạn | {2} tử vong/tháng" },
                { "MH_STATUS_LINE2_V2", "{0} tối đa/tháng" },
                { "MH_STATUS_LINE3", "{0} / {1} xe tang | {2} / {3} tòa nhà | {4} nhân công tối đa" },
                { "MH_STATUS_UPDATED", "cập nhật {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "đề xuất hiện tại: ~{0}% xử lý lò hỏa táng" },
                { "MH_STATUS_PROCESSING_MORE", "đề xuất hiện tại: 500% xử lý lò hỏa táng + thêm cơ sở hoạt động" },
                { "MH_STATUS_PROCESSING_NONE", "đề xuất: bật/thêm lò hỏa táng" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Hiển thị **số mộ đã dùng**, các cơ sở nghĩa trang đang hoạt động và số lần đặt lại nghĩa trang đầy trong phiên này.\n" +
                    "Trạng thái được xóa khi khởi động lại hoặc đổi thành phố."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} mộ đã dùng | {2} cơ sở | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} lần đặt lại" },
                { "MH_STATUS_RESET_PLURAL", "{0} lần đặt lại" },
                { "MH_STATUS_CEMETERY_NONE", "không có trong phiên này" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} nữa" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Tên hiển thị của mod này." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Phiên bản" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Phiên bản hiện tại." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Mở trang Paradox Mods của tác giả." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Báo cáo nhật ký" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "Ghi báo cáo chi tiết về dịch vụ tang lễ và các khu vực có khả năng gặp vấn đề vào MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Mở nhật ký" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Mở **Logs/MagicHearse.log** nếu tệp tồn tại.\n" +
                    "Nếu chưa tìm thấy tệp, sẽ mở thư mục Logs thay thế."
                },
            };
        }

        public void Unload()
        { }
    }
}
