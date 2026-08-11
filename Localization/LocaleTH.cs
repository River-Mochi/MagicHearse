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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "ทำความสะอาดอัตโนมัติ" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "จัดการเอง" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "ขั้นสูง" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "สถานะ" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "ข้อมูลม็อด" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "ลิงก์" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "ดีบัก" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "เปิดการทำความสะอาดแบบเวทมนตร์" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "นำศพที่ต้องใช้รถศพขนส่งออกโดยอัตโนมัติ\n" +
                    "การทำความสะอาดแบบเวทมนตร์และการจัดการเองใช้พร้อมกันไม่ได้ ให้เลือกอย่างใดอย่างหนึ่ง\n" +
                    "ปิดช่องทำเครื่องหมายทั้งหมดเพื่อปิดม็อดโดยไม่ต้องลบม็อด\n" +
                    "หมายเหตุทางเทคนิค: ต้องเป็น IsDead = true และ WaitingForHearse = true"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "รีเซ็ตสุสานที่เต็ม" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**ทำให้สุสานที่เต็มว่างลง** เพื่อไม่ให้ถูกบล็อกด้วยไอคอน เต็ม\n" +
                    "การทำความสะอาดแบบเวทมนตร์จะนำศพส่วนใหญ่ออกก่อนฝัง — ตัวเลือกนี้ยังคงล้างสุสานที่ **เต็มอยู่แล้ว** ได้\n" +
                    "<[ ] ค่าเริ่มต้น ปิด>.\n" +
                    "เปิดตัวเลือกนี้เฉพาะเมื่อคุณต้องการให้โหมดทำความสะอาดแบบเวทมนตร์ล้างสุสานที่เต็มอยู่แล้วด้วย\n" +
                    "เมื่อล้างจนว่างแล้ว โดยปกติไม่จำเป็นต้องเปิดตัวเลือกนี้ค้างไว้ ตราบใดที่ยังเปิดการทำความสะอาดแบบเวทมนตร์อยู่"
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "ผู้จัดการงานศพ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "จัดการและปรับระบบงานศพปกติของเกมด้วยตนเอง\n" +
                    "**ค่าปรับสเกล:** อัตรา, จำนวนรถ, พื้นที่เก็บ\n" +
                    "ตัวเลือกเสริม: **เพิ่มจำนวนคนงาน** ด้วย"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "การประมวลผลของฌาปนสถาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**ความเร็วการประมวลผลของฌาปนสถาน**\n" +
                    "ค่ายิ่งสูงยิ่งเผาศพได้เร็วและคืนพื้นที่เก็บของสถานที่ได้เร็วขึ้น\n" +
                    "**100%** = ค่าเริ่มต้นของเกม"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "จำนวนรถศพทั้งหมด" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**จำนวนรถศพสูงสุด** ต่อสถานที่\n" +
                    "**100%** = ค่าเริ่มต้นของเกม\n" +
                    "**[หมายเหตุ]** รถศพมากเกินไปอาจกระทบการจราจร ขึ้นอยู่กับอัตราการเสียชีวิต"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "ความเร็วรถศพ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**เพิ่มความเร็วขับขี่สูงสุดที่อนุญาตของรถศพ**\n" +
                    "**100%** = ค่าเริ่มต้นของเกม\n" +
                    "<ยังคงใช้ขีดจำกัดความเร็วของถนน>.\n" +
                    "\n" +
                    "ยังปรับอัตราเร่ง/เบรกแบบนุ่มนวล เพื่อให้ความเร็วสูงสุดใหม่ไม่ทำให้รถออกตัวหรือหยุดอย่างรุนแรง\n" +
                    "หมายเหตุ: แม้จะเพิ่มความเร็วสูงสุดของรถศพ ความเร็วที่วิ่งจริงยังขึ้นอยู่กับ:\n" +
                    "ความเร็วสูงสุดที่รถอนุญาต, ขีดจำกัดความเร็วถนน, ความเร็วปลอดภัยของ AI เกม (ทางโค้ง, ถนนเสียหาย) และการจราจร"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "หน่วงการแจ้งเตือนการเสียชีวิต (นาที)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "นี่คือเวลารวมที่รถศพมีเพื่อไปถึงอาคารก่อนที่ไอคอนปัญหา **รอรถศพ** จะปรากฏ\n" +
                    "**3 นาที** ใกล้เคียงค่าเริ่มต้นของเกมประมาณ 2.5 นาทีจำลอง\n" +
                    "เพิ่มค่านี้ได้เพื่อให้รถศพมีเวลาที่เหมาะสมขึ้นในการเดินทางให้เสร็จก่อนที่ไอคอนการเสียชีวิตจะปรากฏ\n" +
                    "หมายเหตุ:\n" +
                    "- <แนะนำ: 10 นาที>. เมืองที่รถติดมากอาจลองตั้งให้สูงกว่านี้\n" +
                    "- ดูรายงานสถานะด้านล่างเพื่อดูว่ามีกี่กรณีที่เกินเวลา\n" +
                    "- ไอคอนที่มองเห็นอยู่แล้วจะไม่ถูกซ่อนเมื่อเพิ่มค่านี้ครั้งแรก และจะคงอยู่จนกว่ารถศพจะจัดการหรืออาคารถูกทุบทิ้ง\n" +
                    "- ปล่อยให้การส่งรถที่มีอยู่เสร็จตามปกติ หรือใช้ช่อง <ทำความสะอาดแบบเวทมนตร์ [x]> หนึ่งครั้งเพื่อเริ่มใหม่อย่างรวดเร็วด้วยตารางเวลาใหม่"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "ความจุสุสาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**ความจุสำหรับเก็บศพของสุสาน** สำหรับอาคารหลัก\n" +
                    "เพิ่มความจุเพื่อให้สุสานที่เต็มกลับมารับศพได้อีกครั้ง\n" +
                    "จะไม่ส่งรถศพเพิ่ม เว้นแต่พื้นที่ไม่พอเป็นสาเหตุที่ทำให้สถานที่หยุดทำงาน\n" +
                    "**100%** = ค่าเริ่มต้นของเกม"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "รีเซ็ตสุสานอัตโนมัติ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**ทำให้สุสานที่เต็มว่างลง** เพื่อไม่ให้ถูกบล็อกด้วยไอคอน เต็ม เหนืออาคาร\n" +
                    "ไม่ต้องลบและสร้างสุสานที่เต็มใหม่อีกต่อไป\n" +
                    "ปิดตัวเลือกนี้เพื่อใช้ **อัตราหมุนเวียนสุสาน** แบบค่อยเป็นค่อยไปแทน\n" +
                    "<[ ✓ ] ค่าเริ่มต้น เปิด>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "อัตราหมุนเวียนสุสาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**ค่อย ๆ คืนหลุมศพที่ถูกใช้งานให้ว่างอีกครั้ง**\n" +
                    "ค่ายิ่งสูงยิ่งทำให้พื้นที่หลุมศพกลับมาใช้ได้เร็วกว่าเกมปกติ\n" +
                    "หากสุสานยังเต็มบ่อยเกินไปแม้ตั้ง 500%,\n" +
                    "ให้เปิด **[รีเซ็ตสุสานอัตโนมัติ]** แทน\n" +
                    "**100%** = อัตราเริ่มต้นของเกมสำหรับการนำหลุมศพกลับมาใช้ใหม่"
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "ปรับจำนวนคนงาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "สวิตช์ความเข้ากันได้:\n" +
                    "**เปิด [✓]** เพื่อเพิ่มจำนวนคนงาน\n" +
                    "**[o_o]** ปล่อยไว้ที่ ปิด หากต้องการให้ **ConfigXML** หรือม็อดอื่นควบคุมคนงานของบริการงานศพ"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "คนงานสูงสุด" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**เพิ่มจำนวนคนงานสูงสุด** ที่อนุญาต\n" +
                    "**100%** = ค่าเริ่มต้นของเกม"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "รีเซ็ตแถบเลื่อน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "ตั้งแถบเลื่อนเปอร์เซ็นต์เป็น **100%** และเวลาหน่วงการแจ้งเตือนการเสียชีวิตเป็น **3 นาที**" },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "ต้องการรถศพ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**กำลังรอ** = ผู้เสียชีวิตทั้งหมดที่ยังอยู่นอกอาคารและกำลังรอการรับ\n" +
                    "**เกินเวลา** = ผู้ที่กำลังรอและพ้นเวลาหน่วงการแจ้งเตือนที่เลือกไว้แล้ว\n" +
                    " - หากมีจำนวนเกินเวลามาก ให้ลองเพิ่มเวลาที่ หน่วงการแจ้งเตือนการเสียชีวิต"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "ปริมาณ" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**ยอดรวมรายเดือน** จากสถิติของเกม\n" +
                    "**สูงสุด/เดือน** = การประมวลผลของฌาปนสถาน + การหมุนเวียนสุสานตามประสิทธิภาพปัจจุบัน\n" +
                    "นี่คือจำนวนศพสูงสุดที่สถานบริการงานศพที่ทำงานอยู่ทั้งหมดสามารถรองรับได้ต่อเดือน"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "ทรัพยากร" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**ความจุของอาคารที่ทำงานอยู่:** รถศพทั้งหมด, อาคาร, คนงานสูงสุด\n" +
                    "\n" +
                    "**หมายเหตุ:**\n" +
                    "▪ รถศพ: กำลังใช้งาน-ไม่ได้จอด / (รถศพทั้งหมด*)\n" +
                    "▪ *รถศพทั้งหมด:\n" +
                    "== รวมรถศพที่อยู่ระหว่างบำรุงรักษา (เช่น งบบริการต่ำ), \n" +
                    "== ไม่รวมรถศพของอาคารที่ถูกปิดใช้งาน\n" +
                    "▪ การสแกนสถานะจะทำงานเฉพาะตอนเปิดหน้าตัวเลือก (หรือเมื่อใช้แถบเลื่อน) ไม่ได้ทำงานทุกเฟรมในเมือง จึงแทบไม่มีผลต่อประสิทธิภาพ :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "ยังไม่ได้โหลดสถานะ" },
                { "MH_STATUS_NO_CITY_LOADED", "ยังไม่ได้โหลดเมือง" },
                { "MH_STATUS_STATS_NOT_AVAIL", "ไม่มีเมือง... ¯\\_(ツ)_/¯ ...ไม่มีสถิติ" },

                { "MH_STATUS_LINE1_V2", "{0} กำลังรอ | {1} เกินเวลา | {2} เสียชีวิต/เดือน" },
                { "MH_STATUS_LINE2_V2", "{0} สูงสุด/เดือน" },
                { "MH_STATUS_LINE3", "{0} / {1} รถศพ | {2} / {3} อาคาร | {4} คนงานสูงสุด" },
                { "MH_STATUS_UPDATED", "อัปเดต {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "แนะนำตอนนี้: การประมวลผลฌาปนสถาน ~{0}%" },
                { "MH_STATUS_PROCESSING_MORE", "แนะนำตอนนี้: การประมวลผลฌาปนสถาน 500% + เพิ่มสถานที่ที่ทำงานอยู่" },
                { "MH_STATUS_PROCESSING_NONE", "แนะนำ: เปิด/เพิ่มฌาปนสถาน" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "สุสาน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "แสดง **หลุมศพที่ใช้อยู่**, สถานที่สุสานที่ทำงานอยู่ และจำนวนครั้งที่รีเซ็ตสุสานเต็มในเซสชันนี้\n" +
                    "สถานะจะถูกล้างเมื่อรีบูตหรือเปลี่ยนเมือง"
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} หลุมศพที่ใช้ | {2} สถานที่ | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "รีเซ็ต {0} ครั้ง" },
                { "MH_STATUS_RESET_PLURAL", "รีเซ็ต {0} ครั้ง" },
                { "MH_STATUS_CEMETERY_NONE", "ไม่มีในเซสชันนี้" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+อีก {0}" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "ม็อด" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "ชื่อที่แสดงของม็อดนี้" },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "เวอร์ชัน" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "เวอร์ชันปัจจุบัน" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "เปิดหน้า Paradox Mods ของผู้สร้าง" },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "รายงานบันทึก" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "เขียนรายงานบริการงานศพโดยละเอียดและจุดที่น่าจะมีปัญหาไปยัง MagicHearse.log" },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "เปิดบันทึก" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "เปิด **Logs/MagicHearse.log** หากมีไฟล์อยู่\n" +
                    "หากยังไม่พบไฟล์ จะเปิดโฟลเดอร์ Logs แทน"
                },
            };
        }

        public void Unload()
        { }
    }
}
