// <copyright file="LocaleTH.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleTH.cs
// Thai th-TH locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Thai localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleTH : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Thai locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleTH(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Thai localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "การทำงาน" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "เกี่ยวกับ" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "ล้างอัตโนมัติ" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "จัดการเอง" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "ขั้นสูง" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "สถานะ" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "ข้อมูลม็อด" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "ลิงก์" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "ดีบัก" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "เปิดใช้ Magic Clean" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "นำศพผู้เสียชีวิตที่ต้องขนส่ง (รถขนศพ) ออกโดยอัตโนมัติ\n" +
                    "Magic Clean และการจัดการเองไม่สามารถใช้พร้อมกันได้ โปรดเลือกอย่างใดอย่างหนึ่ง\n"+
                    "ปิดช่องทำเครื่องหมายทั้งหมดเพื่อปิดใช้งานม็อดโดยไม่ต้องลบออก\n"+
                    "หมายเหตุทางเทคนิค: ต้องเป็น IsDead = true และ WaitingForHearse = true"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "รีเซ็ตสุสานที่เต็ม" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**ทำให้สุสานทุกแห่งที่เต็มว่างลง** เพื่อไม่ให้ถูกปิดกั้นด้วยไอคอนเต็ม\n" +
                    "Magic Clean จะนำศพส่วนใหญ่ออกก่อนฝัง แต่ตัวเลือกนี้ยังคงล้างสุสานที่ **เต็มอยู่แล้ว**\n" +
                    "<[ ] ปิดเป็นค่าเริ่มต้น>.\n" +
                    "เปิดใช้ตัวเลือกนี้เฉพาะเมื่อต้องการให้โหมด Magic Clean ล้างสุสานที่เต็มอยู่แล้วด้วย\n" +
                    "เมื่อสุสานว่างแล้ว โดยปกติไม่จำเป็นต้องเปิดตัวเลือกนี้ค้างไว้ ตราบใดที่ยังเปิดใช้ Magic Clean อยู่"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "ผู้จัดการงานศพ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "จัดการและปรับปรุงระบบจัดการผู้เสียชีวิตตามปกติของเกมโดยอัตโนมัติ\n" +
                    "**ปรับค่า:** อัตราการดำเนินการ จำนวนรถ และความจุ\n" +
                    "ตัวเลือกเสริม: **เพิ่มคนงาน** ด้วย"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "การดำเนินการของฌาปนสถาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**ความเร็วในการดำเนินการของฌาปนสถาน**\n" +
                    "ค่าที่สูงขึ้นจะเผาศพและเพิ่มพื้นที่จัดเก็บของสถานบริการได้เร็วขึ้น\n" +
                    "**100%** = ค่าเริ่มต้นของเกมดั้งเดิม"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "จำนวนรถขนศพทั้งหมด" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**จำนวนรถขนศพสูงสุด** ต่อสถานบริการ\n" +
                    "**100%** = ค่าเริ่มต้นของเกมดั้งเดิม\n" +
                    "**[หมายเหตุ]** รถขนศพจำนวนมากเกินไปอาจส่งผลต่อการจราจร ขึ้นอยู่กับอัตราการเสียชีวิต"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "ความเร็วรถขนศพ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**เพิ่มความเร็วสูงสุดที่รถขนศพได้รับอนุญาตให้ใช้**\n" +
                    "**100%** = ค่าเริ่มต้นของเกมดั้งเดิม\n" +
                    "<ยังคงจำกัดตามความเร็วของถนน>\n\n" +
                    "ปรับอัตราเร่ง/การเบรกด้วย (อย่างนุ่มนวล) เพื่อไม่ให้ความเร็วสูงสุดใหม่ทำให้รถพุ่งตัวหรือหยุดอย่างรุนแรง\n" +
                    "หมายเหตุ: แม้เพิ่มความเร็วสูงสุดของรถขนศพแล้ว ความเร็วจริงยังได้รับอิทธิพลจาก:\n" +
                    "ความเร็วสูงสุดที่รถได้รับอนุญาต ขีดจำกัดความเร็วของถนน ความเร็วปลอดภัยจาก AI ของเกม (ทางโค้ง ความเสียหายของถนน) และการจราจร"

                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "ความจุสุสาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**ความจุของสุสาน** สำหรับอาคารหลัก\n" +
                    "ความจุที่มากขึ้นช่วยให้สุสานที่เต็มกลับมารับการเก็บศพได้อีกครั้ง\n" +
                    "ไม่ได้ส่งรถขนศพเพิ่ม เว้นแต่พื้นที่ไม่พอเป็นสาเหตุที่ปิดกั้นสถานบริการ\n" +
                    "**100%** = ค่าเริ่มต้นของเกมดั้งเดิม"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "รีเซ็ตสุสานอัตโนมัติ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**ทำให้สุสานว่างลง** เมื่อเต็ม เพื่อไม่ให้ถูกปิดกั้นด้วยไอคอนเต็มเหนืออาคาร\n" +
                    "ไม่จำเป็นต้องลบและสร้างสุสานที่เต็มใหม่อีกต่อไป\n" +
                    "ปิดตัวเลือกนี้เพื่อใช้ **อัตราการหมุนเวียนพื้นที่สุสาน** แบบค่อยเป็นค่อยไปแทน\n" +
                    "<[ ✓ ] เปิดเป็นค่าเริ่มต้น>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "อัตราการหมุนเวียนพื้นที่สุสาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**ค่อย ๆ ทำให้พื้นที่หลุมฝังศพที่ถูกใช้งานกลับมาว่างอีกครั้ง**\n" +
                    "ค่าที่สูงขึ้นจะทำให้พื้นที่กลับมาว่างได้เร็วกว่าเกมดั้งเดิม\n" +
                    "หากตั้งไว้ที่ 500% แล้วสุสานยังเต็มบ่อยเกินไป ให้เปิด **[รีเซ็ตสุสานอัตโนมัติ]** แทน\n" +
                    "**100%** = ค่าเริ่มต้นของเกมดั้งเดิม"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "ปรับจำนวนคนงาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "ตัวเลือกความเข้ากันได้:\n" +
                    "**เปิดใช้ [✓]** เพื่อเพิ่มจำนวนคนงาน\n" +
                    "**[o_o]** ปล่อยให้ปิด หากต้องการให้ **ConfigXML** หรือม็อดอื่นควบคุมคนงานบริการจัดการผู้เสียชีวิต"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "จำนวนคนงานสูงสุด" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**เพิ่มจำนวนคนงานสูงสุด** ที่อนุญาต\n" +
                    "**100%** = ค่าเริ่มต้นของเกมดั้งเดิม"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "รีเซ็ตแถบเลื่อน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "ตั้งค่าแถบเลื่อนทั้งหมดกลับเป็น **100%** (ค่าเริ่มต้นของเกมดั้งเดิม)" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "รอรถขนศพ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**ผู้เสียชีวิตที่กำลังรอ** รถขนศพมารับ"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "ปริมาณ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "**ยอดรวมรายเดือน** จากสถิติของเกม\n" +
                     "**รองรับสูงสุด/เดือน** = การดำเนินการของฌาปนสถานรวมกับการหมุนเวียนพื้นที่สุสานตามประสิทธิภาพปัจจุบัน\n" +
                     "นี่คือจำนวนศพสูงสุดที่สถานบริการงานศพที่เปิดใช้งานทั้งหมดรองรับได้ต่อเดือน"
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "ทรัพยากร" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**ความจุของอาคารที่เปิดใช้งาน:** รถขนศพทั้งหมด อาคาร และคนงานสูงสุด\n\n" +
                    "**หมายเหตุ:**\n" +
                    "▪ รถขนศพ: กำลังใช้งาน-ไม่ได้จอด / (รถขนศพทั้งหมด*)\n" +
                    "▪ *รถขนศพทั้งหมด:\n" +
                    "== รวมรถขนศพที่กำลังซ่อมบำรุง (เช่น งบบริการต่ำ)\n" +
                    "== ไม่รวมรถขนศพของอาคารที่ปิดใช้งาน\n" +
                    "▪ การสแกนสถานะจะทำงานเฉพาะขณะเปิดหน้าตัวเลือก (หรือเมื่อใช้แถบเลื่อน) " +
                    "ไม่ทำงานทุกเฟรมในเมือง จึงแทบไม่ส่งผลต่อประสิทธิภาพ :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "ยังไม่ได้โหลดสถานะ" },
                { "MH_STATUS_NO_CITY_LOADED", "ยังไม่ได้โหลดเมือง" },
                { "MH_STATUS_STATS_NOT_AVAIL", "ไม่มีเมือง... ¯\\_(ツ)_/¯ ...ไม่มีสถิติ" },

                { "MH_STATUS_LINE1", "รอรับ {0} | เสียชีวิต {1}/เดือน | อัปเดต {2}" },
                { "MH_STATUS_LINE2", "รองรับสูงสุด {0}/เดือน | ใช้หลุมศพ {1}/{2}" },
                { "MH_STATUS_LINE3", "รถขนศพ {0} / {1} | อาคาร {2} / {3} | คนงานสูงสุด {4}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "คำแนะนำตอนนี้: การดำเนินการของฌาปนสถาน ~{0}%" },
                { "MH_STATUS_PROCESSING_MORE", "คำแนะนำตอนนี้: การดำเนินการของฌาปนสถาน 500% + เปิดใช้สถานบริการเพิ่ม" },
                { "MH_STATUS_PROCESSING_NONE", "คำแนะนำ: เปิดใช้/เพิ่มเมรุ" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "สุสาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**สุสานที่ถูกทำให้ว่างอัตโนมัติในเซสชันนี้** ด้วยตัวเลือกทำให้ว่างเมื่อเต็ม\n" +
                    "แสดงจำนวนการรีเซ็ตทั้งหมดและจำนวนสุสานที่ไม่ซ้ำกัน\n" +
                    "ล้างข้อมูลเมื่อเริ่มเกมใหม่หรือเปลี่ยนเมือง"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "แสดงสุสานที่ถูกทำให้ว่าง และจำนวนครั้งของแต่ละแห่ง (ชื่อ × จำนวนครั้ง)" },

                { "MH_STATUS_LINE4", "รีเซ็ต: {0} · สุสาน: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "ยังไม่มีในเซสชันนี้" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+อีก {0}" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "ม็อด" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "ชื่อที่แสดงของม็อดนี้" },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "เวอร์ชัน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "เวอร์ชันปัจจุบัน" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "เปิดหน้าม็อด Paradox ของผู้สร้าง" },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "รายงานบันทึก" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "เขียนรายงานระบบจัดการศพแบบละเอียดและจุดที่อาจมีปัญหาไปยัง MagicHearse.log" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "เปิดไฟล์บันทึก" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "เปิด **Logs/MagicHearse.log** หากมีไฟล์นี้\n" +
                    "หากยังไม่พบไฟล์ จะเปิดโฟลเดอร์ Logs แทน" },
            };
        }

        public void Unload()
        { }
    }
}
