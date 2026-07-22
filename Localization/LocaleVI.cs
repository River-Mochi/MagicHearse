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
            string title = Mod.ModName;

            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title = title + " (" + Mod.ModVersion + ")";
            }
            return new Dictionary<string, string>

            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), title },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(MHSetting.ActionsTab), "Hành động" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.AboutTab), "Giới thiệu" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AutoCleanGrp), "Dọn dẹp tự động" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.SelfManageGrp), "Tự quản lý" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AdvancedGrp), "Nâng cao" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.StatusGrp), "Trạng thái" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutInfoGrp), "Thông tin mod" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.AboutLinksGrp), "Liên kết" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Bật dọn dẹp ma thuật" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "**Tự động xóa cư dân đã chết** đang chờ xe tang.\n" +
                    "Tắt cả hai ô chọn để vô hiệu hóa mod mà không cần gỡ mod."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Quản lý dịch vụ tang lễ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Tự quản lý mọi thứ.\n" +
                    "**Điều chỉnh hệ số:** tốc độ, đội xe, sức chứa.\n" +
                    "Tùy chọn: cũng có thể **tăng số nhân công**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Tốc độ xử lý" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Tốc độ xử lý của cơ sở** (hỏa táng)\n" +
                    "**100%** = mặc định của game gốc."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Quy mô đội xe" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Số xe tang tối đa** cho mỗi cơ sở.\n" +
                    "**100%** = mặc định của game gốc.\n" +
                    "**[o_o]** Quá nhiều xe tang có thể ảnh hưởng giao thông, tùy theo tỷ lệ tử vong."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Sức chứa nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Sức chứa của tòa nhà nghĩa trang chính**.\n" +
                    "**100%** = mặc định của game gốc."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Tự dọn khi đầy" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Tự động dọn trống nghĩa trang** ngay khi đầy.\n" +
                    "Số phần mộ đang dùng được đặt lại về 0 — giống như xây lại, nhưng tức thì và tự động.\n" +
                    "Kết hợp với thanh trượt **Sức chứa nghĩa trang**: đặt quy mô nghĩa trang rồi để chúng tái sử dụng, nên không cần phá nghĩa trang đã đầy.\n" +
                    "Mặc định BẬT khi **Quản lý dịch vụ tang lễ** đang hoạt động."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Tốc độ xe tang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Tăng tốc độ tối đa của xe tang**.\n" +
                    "**100%** = mặc định của game gốc.\n" +
                    "<Giới hạn tốc độ đường vẫn áp dụng>.\n\n" +
                    "Đồng thời điều chỉnh nhẹ gia tốc và phanh để tốc độ tối đa mới không gây tăng tốc hoặc dừng quá gắt.\n" +
                    "Lưu ý: dù tốc độ tối đa của xe tang được tăng, tốc độ chạy thực tế về cơ bản vẫn phụ thuộc vào:\n" +
                    "(tốc độ tối đa của xe, giới hạn đường, tốc độ an toàn của AI, giao thông)"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Điều khiển nhân công tối đa" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Chế độ tương thích:\n" +
                    "**Bật [✓]** để tăng số nhân công.\n" +
                    "**[o_o]** Để TẮT nếu muốn **ConfigXML** hoặc mod khác điều khiển nhân công dịch vụ tang lễ."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Nhân công tối đa" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Tăng số nhân công tối đa** được phép.\n" +
                    "**100%** = mặc định của game gốc."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Đặt lại thanh trượt" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Đưa tất cả thanh trượt về **100%** (mặc định game gốc)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Cần xe tang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Cư dân đã chết đang chờ** xe tang đến nhận."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Số lượng" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Tổng theo tháng** từ thống kê game.\n" +
                    "**Hỏa táng tối đa/tháng** = mục Xử lý/tháng trong bảng thông tin của game.\n" +
                    "Đây là số thi thể tối đa mà các lò hỏa táng có thể xử lý mỗi tháng."
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
                { "MH_STATUS_LINE2", "{0} hỏa táng tối đa/tháng | {1}/{2} phần mộ đã dùng" },
                { "MH_STATUS_LINE3", "{0} / {1} xe tang | {2} / {3} công trình | {4} nhân công tối đa" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Nghĩa trang" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Nghĩa trang được tự động dọn trong phiên này** bằng tùy chọn Tự dọn khi đầy.\n" +
                    "Hiển thị tổng số lần đặt lại và số nghĩa trang khác nhau.\n" +
                    "Xóa khi khởi động lại hoặc khi đổi thành phố."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)), "Những nghĩa trang nào đã được dọn và số lần của từng nơi (tên × số lần)." },

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
            };
        }

        public void Unload()
        { }
    }
}
