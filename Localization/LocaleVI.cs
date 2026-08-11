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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Hành động" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Giới thiệu" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Dọn dẹp tự động" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Tự quản lý" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Nâng cao" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Trạng thái" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Thông tin mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Liên kết" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "Gỡ lỗi" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Bật dọn dẹp ma thuật" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Tự động xóa thi thể cần được vận chuyển (bằng xe tang).\n" +
                    "Dọn dẹp ma thuật và tự quản lý không thể bật cùng lúc; hãy chọn một trong hai.\n" +
                    "Tắt tất cả ô chọn để vô hiệu hóa mod mà không cần gỡ mod.\n" +
                    "Lưu ý kỹ thuật: phải có IsDead = true và WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Đặt lại nghĩa trang đầy" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Dọn trống mọi nghĩa trang đã đầy** để không bị chặn bởi biểu tượng ĐẦY.\n" +
                    "Dọn dẹp ma thuật xóa hầu hết thi thể trước khi chôn cất — tùy chọn này vẫn dọn mọi nghĩa trang **đã đầy từ trước**.\n" +
                    "<[ ] Mặc định TẮT>.\n" +
                    "Chỉ bật tùy chọn này nếu chế độ dọn dẹp ma thuật cũng cần dọn trống các nghĩa trang đã đầy.\n" +
                    "Sau khi nghĩa trang được dọn trống, thông thường không cần tiếp tục bật tùy chọn này miễn là dọn dẹp ma thuật vẫn được bật."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Quản lý dịch vụ tang lễ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Tự quản lý mọi thứ.\n" +
                    "**Điều chỉnh hệ số:** tốc độ, đội xe, sức chứa.\n" +
                    "Tùy chọn: cũng có thể **tăng số nhân công**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Xử lý tại lò hỏa táng" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Tốc độ xử lý của lò hỏa táng.**\n" +
                    "Giá trị cao hơn sẽ hỏa táng thi thể và giải phóng chỗ chứa của cơ sở sớm hơn.\n" +
                    "**100%** = mặc định của game gốc."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Tổng số xe tang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Số xe tang tối đa** cho mỗi cơ sở.\n" +
                    "**100%** = mặc định của game gốc.\n" +
                    "**[Lưu ý]** Quá nhiều xe tang có thể ảnh hưởng giao thông, tùy theo tỷ lệ tử vong."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Tốc độ xe tang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Tăng tốc độ tối đa của xe tang**.\n" +
                    "**100%** = mặc định của game gốc.\n" +
                    "<Giới hạn tốc độ đường vẫn áp dụng>.\n\n" +
                    "Đồng thời điều chỉnh nhẹ gia tốc và phanh để tốc độ tối đa mới không gây tăng tốc hoặc dừng quá gắt.\n" +
                    "Lưu ý: dù tốc độ tối đa của xe tang được tăng, tốc độ chạy thực tế vẫn chịu ảnh hưởng bởi:\n" +
                    "mức tối đa cho phép của xe, giới hạn đường, tốc độ an toàn của AI trong game (khúc cua, đường hư hỏng) và giao thông."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Độ trễ cảnh báo xe tang (phút)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "**Số phút mô phỏng trước khi biểu tượng chờ xe tang xuất hiện.**\n" +
                    "**3 phút** gần với giá trị mặc định khoảng 2,5 phút.\n" +
                    "Chỉ cảnh báo xe tang thay đổi; cảnh báo xe cứu thương giữ nguyên cài đặt game.\n" +
                    "Tăng giá trị này không ẩn các biểu tượng đang hiển thị."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Sức chứa nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Sức chứa của tòa nhà nghĩa trang chính**.\n" +
                    "Sức chứa lớn hơn giúp nghĩa trang đầy có thể tiếp tục nhận thi thể.\n" +
                    "Tùy chọn này không điều thêm xe tang, trừ khi cơ sở bị chặn vì thiếu chỗ.\n" +
                    "**100%** = mặc định của game gốc."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Tự động đặt lại nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Dọn trống nghĩa trang** khi đầy để không bị biểu tượng ĐẦY phía trên công trình chặn hoạt động.\n" +
                    "Không còn phải xóa và xây lại nghĩa trang đã đầy.\n" +
                    "Tắt tùy chọn này để dùng **Tốc độ giải phóng phần mộ** dần dần.\n" +
                    "<[ ✓ ] Mặc định BẬT>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Tốc độ giải phóng phần mộ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Dần giải phóng các phần mộ đang được sử dụng.**\n" +
                    "Giá trị cao hơn giúp phần mộ trống trở lại nhanh hơn game gốc.\n" +
                    "Nếu ở mức 500% mà nghĩa trang vẫn đầy quá thường xuyên, hãy bật **[Tự động đặt lại nghĩa trang]**.\n" +
                    "**100%** = mặc định của game gốc."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Điều chỉnh nhân công" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Chế độ tương thích:\n" +
                    "**Bật [✓]** để tăng số nhân công.\n" +
                    "**[o_o]** Để TẮT nếu muốn **ConfigXML** hoặc mod khác điều khiển nhân công dịch vụ tang lễ."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Số nhân công tối đa" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Tăng số nhân công tối đa** được phép.\n" +
                    "**100%** = mặc định của game gốc."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Đặt lại thanh trượt" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Đưa tỷ lệ về **100%** và độ trễ cảnh báo xe tang về **3 phút**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Cần xe tang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Cư dân đã chết đang chờ** xe tang đến nhận."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Số lượng" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Tổng theo tháng** từ thống kê game.\n" +
                    "**Xử lý tối đa/tháng** = xử lý tại lò hỏa táng cộng với giải phóng phần mộ theo hiệu suất hiện tại.\n" +
                    "Đây là số thi thể tối đa mà tất cả cơ sở tang lễ đang hoạt động có thể xử lý mỗi tháng."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Cơ sở" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Công suất hoạt động của công trình:** tổng xe tang, công trình và nhân công tối đa.\n\n" +
                    "**Ghi chú:**\n" +
                    "▪ Xe tang: đang hoạt động, không đỗ / (Tổng* xe tang)\n" +
                    "▪ *Tổng xe tang:\n" +
                    "== bao gồm xe tang đang bảo trì (ví dụ: ngân sách dịch vụ thấp), \n" +
                    "== không bao gồm xe tang của công trình bị tắt.\n" +
                    "▪ Quét trạng thái chỉ chạy khi menu Tùy chọn đang mở (hoặc khi dùng thanh trượt); " +
                    "không chạy mỗi khung hình trong thành phố, nên gần như không ảnh hưởng hiệu năng :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Chưa tải trạng thái." },
                { "MH_STATUS_NO_CITY_LOADED", "Chưa tải thành phố." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Không có thành phố... ¯\\_(ツ)_/¯ ...Không có thống kê" },

                { "MH_STATUS_LINE1", "{0} đang chờ | {1} ca tử vong/tháng | cập nhật {2}" },
                { "MH_STATUS_LINE2", "{0} xử lý tối đa/tháng | {1}/{2} phần mộ đã dùng" },
                { "MH_STATUS_LINE3", "{0} / {1} xe tang | {2} / {3} công trình | {4} nhân công tối đa" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "Đề xuất hiện tại: xử lý tại lò hỏa táng ~{0}%" },
                { "MH_STATUS_PROCESSING_MORE", "Đề xuất hiện tại: xử lý tại lò hỏa táng 500% + thêm cơ sở đang hoạt động" },
                { "MH_STATUS_PROCESSING_NONE", "Đề xuất: bật/thêm lò hỏa táng" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Nghĩa trang được tự động dọn trong phiên này** bằng tùy chọn Đặt lại nghĩa trang đầy.\n" +
                    "Hiển thị tổng số lần đặt lại và số nghĩa trang khác nhau.\n" +
                    "Xóa khi khởi động lại hoặc khi đổi thành phố."
                },

                { "MH_STATUS_LINE4", "lần đặt lại: {0} · nghĩa trang: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "không có trong phiên này" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} mục nữa" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Tên hiển thị của mod này." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Phiên bản" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Phiên bản hiện tại." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Mở trang mod Paradox của tác giả." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Báo cáo nhật ký" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "Ghi báo cáo dịch vụ mai táng chi tiết và các vấn đề có thể xảy ra vào MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Mở log" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Mở **Logs/MagicHearse.log** nếu có.\n" +
                    "Nếu chưa tìm thấy file, sẽ mở thư mục Logs." },
            };
        }

        public void Unload()
        { }
    }
}
